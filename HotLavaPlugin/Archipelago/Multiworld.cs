using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.MessageLog.Parts;
using Archipelago.MultiClient.Net.Models;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Archipelago.Models.Locations;
using HotLavaArchipelagoPlugin.Enums;
using HotLavaArchipelagoPlugin.Extensions;
using HotLavaArchipelagoPlugin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotLavaArchipelagoPlugin.Archipelago
{
    internal class Multiworld
    {
        internal static ArchipelagoSession? ArchipelagoSession;
        private static DeathLinkService? DeathLinkService = null;
        private static string PlayerName = "Unknown";
        private static Dictionary<long, ScoutedItemInfo> ScoutedItems = new Dictionary<long, ScoutedItemInfo>();
        private static Queue<ScoutedItemInfo> QueuedAwardItems = new Queue<ScoutedItemInfo>();

        public static async Task ParseArchipelagoConnectMessage(string message)
        {
            string[] split = message.Split(" ");

            if (split.Length < 3)
            {
                UIHelper.ShowPopup("Not enough arguments provided. Format: /apconnect <roomUrl> <playerName> [<password>]");
            }

            string roomUrl = split[1];

            if (!roomUrl.Contains("://"))
            {
                roomUrl = "ws://" + roomUrl;
            }

            Uri roomUri;
            try
            {
                roomUri = new Uri(roomUrl);
            }
            catch (Exception)
            {
                UIHelper.ShowPopup("Failed to parse provided URL for room");
                return;
            }

            PlayerName = split[2];
            string? password = split.Length > 3 ? split[3] : null;

            try
            {
                ArchipelagoSession = ArchipelagoSessionFactory.CreateSession(roomUri.Host, roomUri.Port);

                ArchipelagoSession.MessageLog.OnMessageReceived += OnMessageReceived;
                ArchipelagoSession.Items.ItemReceived += OnItemReceived;
                ArchipelagoSession.Locations.CheckedLocationsUpdated += OnLocationsChecked;

                //DeathLinkService = ArchipelagoSession.CreateDeathLinkService();

                //DeathLinkService.OnDeathLinkReceived += OnDeathLinkReceived;
                //DeathLinkService.EnableDeathLink();

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
                    ArchipelagoSession = null;
                    DeathLinkService = null;
                }
                else
                {
                    await ScoutAllLocations();

                    Plugin.Logger.LogInfo("Successfully logged in to Archipelago");
                    Plugin.Logger.LogInfo("Archipelago Seed: " + ArchipelagoSession.RoomState.Seed);
                    UIHelper.ShowPopup("Successfully connected to Archipelago");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowPopup("Failed to connect to Archipelago due to an unknown error");
                Plugin.Logger.LogError("Error connecting to Archipelago: " + ex.ToString());
            }
        }

        private static async Task ScoutAllLocations()
        {
            ScoutedItems = await ArchipelagoSession!.Locations
                .ScoutLocationsAsync(HintCreationPolicy.None, Locations.AllLocations.Keys.ToArray());
        }

        private static void OnDeathLinkReceived(DeathLink deathLink)
        {
            //TODO: kill player
        }

        private static void OnLocationsChecked(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
        {
            foreach (long locationId in newCheckedLocations)
            {
                Location? location = Locations.GetLocation(locationId);
                if (location != null)
                {
                    //TODO: This may crash the game if called when not in profile, need to add preventative measures
                    location.CheckLocation();
                    CheckGoalCompleted();
                }
            }
        }

        private static void OnItemReceived(ReceivedItemsHelper helper)
        {
            ItemInfo receivedItem = helper.PeekItem();

            Plugin.Logger.LogInfo("Received Item: " + receivedItem.ItemName);

            UIHelper.SendNotificationMessage("Received <color=#8e7cc3>" + receivedItem.ItemDisplayName + "</color> from <color=#ffd966>"
                + receivedItem.Player.Name + "</color> (<color=#93c47d>" + receivedItem.LocationDisplayName + "</color>)");

            Item? item = Items.GetItem(receivedItem.ItemId);

            if (item != null)
            {
                item.GrantItem();
            }

            //TODO: should we only call this and save the return value?
            helper.DequeueItem();
        }

        /// <summary>
        /// Called when a message is received from the Archipelago Server
        /// https://archipelagomw.github.io/Archipelago.MultiClient.Net/docs/helpers/events.html#messageloghelper
        /// </summary>
        /// <param name="logMessage">The message received from the server</param>
        public static void OnMessageReceived(LogMessage logMessage)
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

        public static void SendLocationCheck(long locationId)
        {
            if (ArchipelagoSession == null) return;

            ArchipelagoSession.Locations.CompleteLocationChecks(locationId);
            CheckGoalCompleted();
        }

        /// <summary>
        /// Checks if the player has collected all "Complete the Course" stars
        /// </summary>
        public static void CheckGoalCompleted()
        {
            if (ArchipelagoSession == null) return;

            bool shouldRelease = Locations.AllLocations
                .Values
                .Where(l => l is StarLocation)
                .Select(l => (StarLocation)l)
                .Where(l => l.StarType == StarType.CourseComplete)
                .Where(l => ArchipelagoSession.Locations.AllLocations.Contains(l.LocationID)) //Filter out disabled location checks
                .All(l => ArchipelagoSession.Locations.AllLocationsChecked.Contains(l.LocationID));

            if (shouldRelease)
            {
                ArchipelagoSession.SetGoalAchieved();
            }
        }

        public static void SendDeath(string deathReason)
        {
            if (DeathLinkService == null) return;

            DeathLinkService.SendDeathLink(new DeathLink(PlayerName, deathReason));
        }

        /// <summary>
        /// Gets the scouted item info for the specified location
        /// </summary>
        /// <param name="locationId">The ID of the location</param>
        /// <returns>The item info</returns>
        public static ScoutedItemInfo? GetItemForLocation(long locationId)
        {
            return ScoutedItems.GetValueOrDefault(locationId);
        }

        public static void QueueAwardItem(ScoutedItemInfo scoutedItemInfo)
        {
            if (!QueuedAwardItems.Contains(scoutedItemInfo))
            {
                QueuedAwardItems.Enqueue(scoutedItemInfo);
            }
        }

        public static ScoutedItemInfo? PopAwardsQueue()
        {
            if (QueuedAwardItems.TryDequeue(out ScoutedItemInfo dequeuedItem))
                return dequeuedItem;
            else
                return null;
        }
    }
}
