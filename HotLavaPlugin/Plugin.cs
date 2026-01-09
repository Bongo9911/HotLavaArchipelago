using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Archipelago.Models.Locations;
using HotLavaArchipelagoPlugin.Game;
using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava;
using Klei.HotLava.Game;
using Klei.L10n;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HotLavaArchipelagoPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static ArchipelagoSession? ArchipelagoSession;

    internal async Task Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        Logger.LogInfo($"Harmony patches applied!");

        Logger.LogInfo(JsonConvert.SerializeObject(Worlds.AllWorlds));
    }

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
            roomUrl = "http://" + roomUrl;
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

        string playerName = split[2];
        string? password = split.Length > 3 ? split[3] : null;

        try
        {
            //ArchipelagoSession = ArchipelagoSessionFactory.CreateSession("archipelago.gg", 57655);
            ArchipelagoSession = ArchipelagoSessionFactory.CreateSession(roomUri.Host, roomUri.Port);

            ArchipelagoSession.MessageLog.OnMessageReceived += OnMessageReceived;
            ArchipelagoSession.Items.ItemReceived += OnItemReceived;
            ArchipelagoSession.Locations.CheckedLocationsUpdated += OnLocationsChecked;

            await ArchipelagoSession.ConnectAsync();

            Logger.LogInfo("Connected to Archipelago");

            //https://github.com/ArchipelagoMW/Archipelago/blob/main/docs/network%20protocol.md
            string gameName = "Hot Lava";
            ItemsHandlingFlags itemsHandlingFlags = ItemsHandlingFlags.AllItems;
            Version minArchipelagoVersion = new Version(0, 6, 5);
            string[] tags = ["DeathLink"];
            string? uuid = null;
            bool requestSlotData = true;

            LoginResult loginResult = await ArchipelagoSession.LoginAsync(
                gameName, // Name of the game implemented by this client, SHOULD match what is used in the world implementation
                playerName, // Name of the slot to connect as (a.k.a player name)
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
            }
            else
            {
                Logger.LogInfo("Successfully logged in to Archipelago");
                Logger.LogInfo("Archipelago Seed: " + ArchipelagoSession.RoomState.Seed);
                UIHelper.ShowPopup("Successfully connected to Archipelago");
            }
        }
        catch (Exception ex)
        {
            UIHelper.ShowPopup("Failed to connect to Archipelago due to an unknown error");
            Logger.LogError("Error connecting to Archipelago: " + ex.ToString());
        }
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
            }
        }
    }

    private static void OnItemReceived(ReceivedItemsHelper helper)
    {
        ItemInfo receivedItem = helper.PeekItem();

        Logger.LogInfo("Received Item: " + receivedItem.ItemName);

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
        //TODO: handle colors for message parts
        UIHelper.SendChatMessage(logMessage.ToString());
    }

    public static LevelMetaData GetCurrentLevelMetaData()
    {
        PropertyInfo property = typeof(Info).GetProperty("CurrentLevelMetaData", BindingFlags.NonPublic | BindingFlags.Static);
        return (LevelMetaData)property.GetValue(null);
    }

    public static string? GameModeToValidStringKeyElement(GameMode mode)
    {
        int num = mode.m_ID.IndexOf(".");
        return num > -1 ? "GAMEMODE_" + LocConversions.NameToValidStringKeyElement(mode.m_ID.Substring(num + 1)) : null;
    }
}
