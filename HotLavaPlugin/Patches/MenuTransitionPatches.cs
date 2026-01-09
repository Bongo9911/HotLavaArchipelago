using HarmonyLib;
using Klei.HotLava;
using Klei.HotLava.UI;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches
{
    [HarmonyPatch(typeof(MenuTransition))]
    internal class MenuTransitionPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPrefix]
        public static void Start_Prefix(MenuTransition __instance)
        {
            Plugin.Logger.LogInfo(__instance.name + " transition from " + __instance.m_FromMenu.name);

            FieldInfo menuScreenField = typeof(MenuTransition).GetField("m_Screen", BindingFlags.NonPublic | BindingFlags.Instance);
            MenuScreen menuScreen = (MenuScreen)menuScreenField.GetValue(__instance);

            Plugin.Logger.LogInfo("Menu Screen name: " + menuScreen.name);
            Plugin.Logger.LogInfo("Menu Screen type: " + menuScreen.GetType().ToString());

            //TOOD: connect to Archipelago when stage selection menu is shown
        }
    }
}
