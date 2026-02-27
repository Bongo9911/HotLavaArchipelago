using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Locations;
using HotLavaArchipelagoPlugin.Enums;
using Klei.HotLava.Game;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(CollectibleForLevel))]
    internal class CollectibleForLevelPatches
    {
        [HarmonyPatch("OnEnable")]
        [HarmonyPrefix]
        public static bool OnEnable_Prefix(CollectibleForLevel __instance)
        {
            if (Multiworld.Connected && __instance.m_Unlockable != null)
            {
                Location? location = Locations.GetUnlockableLocation(__instance.m_Unlockable.m_Key.m_Value);

                if (location is StarLocation starLocation)
                {
                    if (starLocation.StarType == StarType.GoldenPin || starLocation.StarType == StarType.Comic)
                    {
                        //TODO: Check if player has unlocked collectibles
                        //if (true)
                        //{
                        //    __instance.gameObject.SetActive(false);
                        //    return false;
                        //}
                    }

                    __instance.m_XRayMode = true;
                }
            }

            return true;
        }
    }
}
