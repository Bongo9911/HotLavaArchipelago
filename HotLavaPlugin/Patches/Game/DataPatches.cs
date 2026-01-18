using HarmonyLib;
using Klei.HotLava.Game;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(Data))]
    internal class DataPatches
    {
        [HarmonyPatch(nameof(Data.CanUserPlayLevel))]
        [HarmonyPrefix]
        public static bool CanUserPlayLevel_Prefix(Data __instance, int index, ref bool __result)
        {
            //TODO: Check if user has world unlocked
            __result = true;
            return false;
        }
    }
}
