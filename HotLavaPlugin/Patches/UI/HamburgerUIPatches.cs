using HarmonyLib;
using HotLavaArchipelagoPlugin.UI;
using Klei.HotLava;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.UI
{
    /// <summary>
    /// Klei.HotLava.UI.HamburgerUI (the title screen's Settings/Profiles/Credits/Exit Game
    /// overlay) is declared internal, so it can't be named directly (typeof(HamburgerUI) would
    /// fail to compile here) - it has to be resolved by name at runtime and patched through its
    /// public MenuTransition base instead.
    /// </summary>
    [HarmonyPatch]
    internal class HamburgerUIPatches
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("Klei.HotLava.UI.HamburgerUI"), "Start");
        }

        [HarmonyPostfix]
        public static void Start_Postfix(MenuTransition __instance)
        {
            ModsMenuFactory.InstallArchipelagoButton(__instance);
        }
    }
}
