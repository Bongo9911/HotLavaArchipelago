using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HotLavaPlugin.Archipelago.Data;
using HotLavaPlugin.Archipelago.Models.Items;
using HotLavaPlugin.Archipelago.Models.Locations;
using Klei.HotLava;
using Klei.HotLava.Conditions;
using Klei.HotLava.Game;
using Klei.HotLava.UI;
using Klei.HotLava.Unlockables;
using Klei.L10n;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HotLavaPlugin;

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

        ArchipelagoSession = ArchipelagoSessionFactory.CreateSession("archipelago.gg", 57655);

        ArchipelagoSession.MessageLog.OnMessageReceived += OnMessageReceived;
        ArchipelagoSession.Items.ItemReceived += OnItemReceived;
        ArchipelagoSession.Locations.CheckedLocationsUpdated += OnLocationsChecked;

        //https://github.com/ArchipelagoMW/Archipelago/blob/main/docs/network%20protocol.md
        string gameName = "Hot Lava";
        string playerName = "Bongo9911";
        ItemsHandlingFlags itemsHandlingFlags = ItemsHandlingFlags.AllItems;
        Version minArchipelagoVersion = new Version(0, 6, 5);
        string[] tags = ["DeathLink"];
        string? uuid = null;
        string? password = null; //TODO: Will likely need a place to enter this
        bool requestSlotData = true;

        await ArchipelagoSession.ConnectAsync();

        Logger.LogInfo("Connected to Archipelago");

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

        //foreach (Unlockable unlockable in Statistics.AllUnlockables)
        //{
        //    FormatUnlockableString(unlockable);
        //}

        Logger.LogInfo("Successfully logged in to Archipelago");
        Logger.LogInfo("Archipelago Seed: " + ArchipelagoSession.RoomState.Seed);

        //Grants 4 experience to player
        //CharacterStatistics.HitShard();
    }

    private void OnLocationsChecked(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
    {
        foreach (long locationId in newCheckedLocations)
        {
            Location? location = Locations.GetLocation(locationId);
            if (location != null)
            {
                //TODO: Calling this when loading into game crashes the game. Need to figure out when it's safe to call this
                //location.CheckLocation();
            }
        }
    }

    private void FormatUnlockableString(Unlockable unlockable)
    {
        if (unlockable.m_Condition is ConditionCompleteGameMode gameModeCompleteCondition)
        {
            Plugin.Logger.LogInfo("game mode complete for: " + gameModeCompleteCondition.m_LevelName + " - " + gameModeCompleteCondition.m_GameModeID + " - " + unlockable.m_Key.m_Value + " - " + unlockable.ToString());
        }
        else if (unlockable.m_Condition is ConditionCompleteLevel levelCompleteCondition)
        {
            Plugin.Logger.LogInfo("level complete for: " + levelCompleteCondition.m_LevelName + " - " + unlockable.m_Key.m_Value + " - " + unlockable.ToString());
        }
        else if (unlockable.m_Condition is ConditionUnlockedUnlockable unlockedUnlockableCondition)
        {
            Plugin.Logger.LogInfo("unlocked unlockable for: " + unlockedUnlockableCondition.m_Unlockable.ToString() + " - " + unlockable.m_Key.m_Value + " - " + unlockable.ToString());
        }
        else
        {
            Plugin.Logger.LogInfo("unlockable: " + unlockable.ToString() + " - " + unlockable.m_Key.m_Value);
        }
    }

    private void OnItemReceived(ReceivedItemsHelper helper)
    {
        ItemInfo receivedItem = helper.PeekItem();

        Logger.LogInfo("Received Item: " + receivedItem.ItemName);

        Item? item = Items.GetItem(receivedItem.ItemId);

        if (item != null)
        {
            //item.GrantItem();
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
        SendChatMessage(logMessage.ToString());
    }

    public static void SendNotificationMessage(string message)
    {
        CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
        characterCanvas?.m_PushMessages.QueueMessage(message);
    }

    public static void SendChatMessage(string message)
    {
        CharacterCanvas characterCanvas = Singleton<LevelSingleton>.Instance.m_CharacterCanvas;
        characterCanvas?.m_NetworkChat.QueueMessage(message);
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
