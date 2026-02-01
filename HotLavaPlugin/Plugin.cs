using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HotLavaArchipelagoPlugin.Models.Game;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace HotLavaArchipelagoPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger = new ManualLogSource(string.Empty);

    public static ConfigEntry<string> ConfigArchipelagoHost;
    public static ConfigEntry<int> ConfigArchipelagoPort;
    public static ConfigEntry<string> ConfigArchipelagoPlayerName;
    public static ConfigEntry<string> ConfigArchipelagoPassword;

    internal async Task Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        Logger.LogInfo($"Harmony patches applied!");

        Logger.LogInfo(JsonConvert.SerializeObject(Worlds.AllWorlds));

        ConfigArchipelagoHost = Config.Bind("Archipelago", "Host", "archipelago.gg", "The host name of the Archipelago server");
        ConfigArchipelagoPort = Config.Bind("Archipelago", "Port", 38281, "The port for the Archipelago server");
        ConfigArchipelagoPlayerName = Config.Bind("Archipelago", "PlayerName", "Player", "Your slot name in your YAML file");
        ConfigArchipelagoPassword = Config.Bind("Archipelago", "Password", string.Empty, "The password for connecting to the server, if one is required");

        //GameObject gameObj = GuidDictionary.LoadAsset<GameObject>(new Guid("5ecb96c9d45e3a64a8618a5715a26b20"));
    }
}
