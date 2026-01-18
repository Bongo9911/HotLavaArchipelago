using HarmonyLib;
using Klei.HotLava.Game;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(GameModeTweak))]
    internal class GameModeTweakPatches
    {
        [HarmonyPatch(nameof(GameModeTweak.IsUnlocked))]
        [HarmonyPrefix]
        public static bool IsUnlocked_Prefix(ref bool __result)
        {
            //Allow performing game mode tweaks out of order (e.g. using Buddy on later stages early)
            __result = true;
            return false;
        }
    }
}
