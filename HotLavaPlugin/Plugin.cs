using BepInEx;
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

    internal async Task Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        Logger.LogInfo($"Harmony patches applied!");

        Logger.LogInfo(JsonConvert.SerializeObject(Worlds.AllWorlds));

        //GameObject gameObj = GuidDictionary.LoadAsset<GameObject>(new Guid("5ecb96c9d45e3a64a8618a5715a26b20"));
    }
}
