using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Klei.HotLava;
using Klei.HotLava.Game;
using Klei.HotLava.UI;
using Klei.L10n;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HotLavaPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private async Task Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        Logger.LogInfo($"Harmony patches applied!");

        ArchipelagoSession session = ArchipelagoSessionFactory.CreateSession("localhost", 38281);

        session.MessageLog.OnMessageReceived += OnMessageReceived;

        //https://github.com/ArchipelagoMW/Archipelago/blob/main/docs/network%20protocol.md
        string gameName = "Hot Lava";
        string playerName = "Bongo9911";
        ItemsHandlingFlags itemsHandlingFlags = ItemsHandlingFlags.AllItems;
        Version minArchipelagoVersion = new Version(0, 6, 5);
        string[] tags = ["DeathLink"];
        string? uuid = null;
        string? password = null; //Will likely need a place to enter this
        bool requestSlotData = true;

        await session.ConnectAsync();

        LoginResult loginResult = await session.LoginAsync(
            gameName, // Name of the game implemented by this client, SHOULD match what is used in the world implementation
            playerName, // Name of the slot to connect as (a.k.a player name)
            itemsHandlingFlags,
            minArchipelagoVersion, // Minimum Archipelago API specification version which this client can successfuly interface with
            tags,
            uuid, // Unique identifier for this player/client, if null randomly generated
            password, // Password that was set when the room was created
            requestSlotData // If the LoginResult should contain the slot data
        );
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
