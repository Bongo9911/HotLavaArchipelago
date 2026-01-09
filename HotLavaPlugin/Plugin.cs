using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Klei.HotLava;
using Klei.HotLava.Game;
using Klei.L10n;
using System.Reflection;
using System.Threading.Tasks;

namespace HotLavaArchipelagoPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger = new ManualLogSource(string.Empty);

    internal async Task Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        Logger.LogInfo($"Harmony patches applied!");

        //Logger.LogInfo(JsonConvert.SerializeObject(Worlds.AllWorlds));
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
