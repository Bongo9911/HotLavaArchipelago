using HarmonyLib;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.Gameplay
{
    [HarmonyPatch]
    internal class HotPotatoPatches
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method("Klei.HotLava.Gameplay.HotPotato:Awake");
        }

        public static bool Prefix(object __instance)
        {
            //TODO: Can we do anything with this?
            Plugin.Logger.LogInfo("Loaded Hot Potato");
            return true;
        }
    }
}
