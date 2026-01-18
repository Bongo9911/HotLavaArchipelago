using HarmonyLib;
using Klei.HotLava.Character;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    /// <summary>
    /// Modifies functions ran on the player controller
    /// </summary>
    [HarmonyPatch(typeof(PlayerController))]
    internal class PlayerControllerPatches
    {
        [HarmonyPatch("StartCrouching")]
        [HarmonyPrefix]
        public static bool StartCrouching_Prefix(PlayerController __instance)
        {
            //TODO: check if crouching is unlocked
            //return false;
            return true;
        }
    }
}
