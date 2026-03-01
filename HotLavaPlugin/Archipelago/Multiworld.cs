using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.MessageLog.Parts;
using Archipelago.MultiClient.Net.Models;
using BepInEx;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Archipelago.Models.Locations;
using HotLavaArchipelagoPlugin.Archipelago.Models.Options;
using HotLavaArchipelagoPlugin.Enums;
using HotLavaArchipelagoPlugin.Extensions;
using HotLavaArchipelagoPlugin.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotLavaArchipelagoPlugin.Archipelago
{
    internal class Multiworld
    {
        private static Multiworld? _instance = null;

        public static Multiworld Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new NullReferenceException("Multiworld has not been initialized");
                }
                return _instance;
            }
        }
        /// <summary>
        /// Whether there is an active archipelago connection
        /// </summary>
        public static bool Connected
        {
            get
            {
                return _instance != null;
            }
        }

        internal ArchipelagoSession ArchipelagoSession;
        internal SlotData SlotData = new SlotData();

        private DeathLinkService? DeathLinkService = null;
        private string PlayerName = "Unknown";
        private Dictionary<long, ScoutedItemInfo> ScoutedItems = new Dictionary<long, ScoutedItemInfo>();

        private Queue<ScoutedItemInfo> QueuedAwardItems = new Queue<ScoutedItemInfo>();

        public Multiworld(ArchipelagoSession archipelagoSession)
        {
            ArchipelagoSession = archipelagoSession;
        }

        public static async Task Connect(string message)
        {
            try
            {
                string host = Plugin.ConfigArchipelagoHost.Value;
                int port = Plugin.ConfigArchipelagoPort.Value;

                ArchipelagoSession archipelagoSession = ArchipelagoSessionFactory.CreateSession(host, port);

                Multiworld multiworld = new Multiworld(archipelagoSession);

                await multiworld.ConnectAsync();

                _instance = multiworld;
            }
            catch (Exception ex)
            {
                UIHelper.ShowPopup("Failed to connect to Archipelago due to an unknown error");
                Plugin.Logger.LogError("Error connecting to Archipelago: " + ex.ToString());
            }
        }

        private async Task ConnectAsync()
        {
            PlayerName = Plugin.ConfigArchipelagoPlayerName.Value;
            string password = Plugin.ConfigArchipelagoPassword.Value;

            ArchipelagoSession.MessageLog.OnMessageReceived += OnMessageReceived;
            ArchipelagoSession.Items.ItemReceived += OnItemReceived;
            ArchipelagoSession.Locations.CheckedLocationsUpdated += OnLocationsChecked;

            await ArchipelagoSession.ConnectAsync();

            Plugin.Logger.LogInfo("Connected to Archipelago");

            //https://github.com/ArchipelagoMW/Archipelago/blob/main/docs/network%20protocol.md
            const string gameName = "Hot Lava";
            const ItemsHandlingFlags itemsHandlingFlags = ItemsHandlingFlags.AllItems;
            Version minArchipelagoVersion = new Version(0, 6, 5);
            string[] tags = ["DeathLink"];
            const string? uuid = null;
            const bool requestSlotData = true;

            LoginResult loginResult = await ArchipelagoSession.LoginAsync(
                gameName, // Name of the game implemented by this client, SHOULD match what is used in the world implementation
                PlayerName, // Name of the slot to connect as (a.k.a player name)
                itemsHandlingFlags,
                minArchipelagoVersion, // Minimum Archipelago API specification version which this client can successfuly interface with
                tags,
                uuid, // Unique identifier for this player/client, if null randomly generated
                password, // Password that was set when the room was created
                requestSlotData // If the LoginResult should contain the slot data
            );

            if (loginResult is LoginFailure loginFailure)
            {
                string errors = "";

                foreach (string error in loginFailure.Errors)
                {
                    errors += "\r\n" + error;
                }

                if (string.IsNullOrEmpty(errors))
                {
                    errors = "\r\n" + "An unknown error";
                }

                UIHelper.ShowPopup("Failed to login to Archipelago due to:" + errors);
                await ArchipelagoSession.Socket.DisconnectAsync();
            }
            else
            {
                await InitializeAsync();

                Plugin.Logger.LogInfo("Successfully logged in to Archipelago");
                Plugin.Logger.LogInfo("Archipelago Seed: " + ArchipelagoSession.RoomState.Seed);
                UIHelper.ShowPopup("Successfully connected to Archipelago");
            }
        }

        private async Task InitializeAsync()
        {
            await InitializeDeathLink();
            await ScoutAllLocations();

            SlotData = await ArchipelagoSession.DataStorage.GetSlotDataAsync<SlotData>();

            Plugin.Logger.LogInfo("Data: " + JsonConvert.SerializeObject(SlotData));
        }

        private async Task InitializeDeathLink()
        {
            DeathLinkService = ArchipelagoSession.CreateDeathLinkService();
            DeathLinkService.OnDeathLinkReceived += OnDeathLinkReceived;
        }

        private async Task ScoutAllLocations()
        {
            ScoutedItems = await ArchipelagoSession.Locations
                .ScoutLocationsAsync(HintCreationPolicy.None, Locations.AllLocations.Keys.ToArray());
        }

        private void OnDeathLinkReceived(DeathLink deathLink)
        {
            try
            {
                ThreadingHelper.Instance.StartSyncInvoke(() =>
                {
                    HotLavaPlayerHelper.KillLocalPlayer();
                });
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError("Failed to kill local player for death link: " + ex.ToString());
            }
        }

        private void OnLocationsChecked(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
        {
            foreach (long locationId in newCheckedLocations)
            {
                Location? location = Locations.GetLocation(locationId);
                if (location != null)
                {
                    //TODO: This may crash the game if called when not in profile, need to add preventative measures
                    ThreadingHelper.Instance.StartSyncInvoke(() =>
                    {
                        location.CheckLocation();
                        CheckGoalCompleted();
                    });
                }
            }
        }

        private void OnItemReceived(ReceivedItemsHelper helper)
        {
            ItemInfo receivedItem = helper.DequeueItem();

            Plugin.Logger.LogInfo("Received Item: " + receivedItem.ItemName);

            string message = "Received <color=";

            if (receivedItem.Flags.HasFlag(ItemFlags.Advancement))
            {
                message += Color.Plum.ToHexColorCode();
            }
            else if (receivedItem.Flags.HasFlag(ItemFlags.NeverExclude))
            {
                message += Color.SlateBlue.ToHexColorCode();
            }
            else if (receivedItem.Flags.HasFlag(ItemFlags.Trap))
            {
                message += Color.Salmon.ToHexColorCode();
            }
            else
            {
                message += Color.Cyan.ToHexColorCode();
            }

            message += ">" + receivedItem.ItemDisplayName + "</color> from <color=";

            if (receivedItem.Player.Slot == ArchipelagoSession.Players.ActivePlayer.Slot)
            {
                message += Color.Yellow.ToHexColorCode();
            }
            else
            {
                message += Color.Magenta.ToHexColorCode();
            }

            message += ">" + receivedItem.Player.Name + "</color> (<color=" + Color.White.ToHexColorCode() + ">" + receivedItem.LocationDisplayName + "</color>)";

            UIHelper.SendNotificationMessage(message);

            Item? item = Items.GetItem(receivedItem.ItemId);

            if (item != null)
            {
                ThreadingHelper.Instance.StartSyncInvoke(() =>
                {
                    item.GrantItem();
                });
            }
        }

        /// <summary>
        /// Called when a message is received from the Archipelago Server
        /// https://archipelagomw.github.io/Archipelago.MultiClient.Net/docs/helpers/events.html#messageloghelper
        /// </summary>
        /// <param name="logMessage">The message received from the server</param>
        public void OnMessageReceived(LogMessage logMessage)
        {
            string message = string.Empty;

            foreach (MessagePart messagePart in logMessage.Parts)
            {
                if (messagePart.PaletteColor != null)
                {
                    message += "<color=" + messagePart.Color.ToHexColorCode() + ">";
                }

                message += messagePart.Text;

                if (messagePart.PaletteColor != null)
                {
                    message += "</color>";
                }
            }

            Plugin.Logger.LogInfo("Message received: " + logMessage.ToString());
            Plugin.Logger.LogInfo("Message formatted: " + message);

            UIHelper.SendChatMessage(message);
        }

        public void SendLocationCheck(long locationId)
        {
            ArchipelagoSession.Locations.CompleteLocationChecks(locationId);
            CheckGoalCompleted();
        }

        /// <summary>
        /// Checks if the player has collected all "Complete the Course" stars
        /// </summary>
        public void CheckGoalCompleted()
        {
            bool shouldRelease = Locations.AllLocations
                .Values
                .Where(l => l is StarLocation)
                .Select(l => (StarLocation)l)
                .Where(l => l.Course.World.WorldOptionId == SlotData.LastWorld && l.StarType == StarType.CourseComplete)
                .Where(l => ArchipelagoSession.Locations.AllLocations.Contains(l.LocationID)) //Filter out disabled location checks
                .All(l => ArchipelagoSession.Locations.AllLocationsChecked.Contains(l.LocationID));

            if (shouldRelease)
            {
                ArchipelagoSession.SetGoalAchieved();
            }
        }

        public void SendDeath(string deathReason)
        {
            if (DeathLinkService == null) return;

            DeathLinkService.SendDeathLink(new DeathLink(PlayerName, deathReason));
        }

        /// <summary>
        /// Gets the scouted item info for the specified location
        /// </summary>
        /// <param name="locationId">The ID of the location</param>
        /// <returns>The item info</returns>
        public ScoutedItemInfo? GetItemForLocation(long locationId)
        {
            return ScoutedItems.GetValueOrDefault(locationId);
        }

        public void QueueAwardItem(ScoutedItemInfo scoutedItemInfo)
        {
            if (!QueuedAwardItems.Contains(scoutedItemInfo))
            {
                QueuedAwardItems.Enqueue(scoutedItemInfo);
            }
        }

        public ScoutedItemInfo? PopAwardsQueue()
        {
            if (QueuedAwardItems.TryDequeue(out ScoutedItemInfo dequeuedItem))
                return dequeuedItem;
            else
                return null;
        }

        /// <summary>
        /// Checks whether the player has received the requested item
        /// </summary>
        /// <param name="itemId">The ID of the item</param>
        /// <returns></returns>
        public static bool HasReceivedItem(long itemId)
        {
            return Instance.ArchipelagoSession.Items.AllItemsReceived.Any(m => m.ItemId == itemId);
        }

        /// <summary>
        /// Checks whether the player has received the requested item
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns></returns>
        public static bool HasReceivedItem(Item item)
        {
            return HasReceivedItem(item.Id);
        }

        /// <summary>
        /// Checks whether the player has checked a location
        /// </summary>
        /// <param name="location">The location</param>
        /// <returns></returns>
        public static bool HasCheckedLocation(Location location)
        {
            return Instance.ArchipelagoSession.Locations.AllLocationsChecked.Contains(location.LocationID);
        }
    }
}
