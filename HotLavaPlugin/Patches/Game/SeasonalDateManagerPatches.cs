using HarmonyLib;
using Klei.HotLava;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(SeasonalDateManager))]
    internal class SeasonalDateManagerPatches
    {
        [HarmonyPatch(nameof(SeasonalDateManager.IsSeasonActive))]
        [HarmonyPrefix]
        public static bool IsSeasonActive_Prefix(SeasonalDateManager.SeasonalEvents season, ref bool __result)
        {
            __result = season != SeasonalDateManager.SeasonalEvents.NONE;
            return false;
        }
    }
}
