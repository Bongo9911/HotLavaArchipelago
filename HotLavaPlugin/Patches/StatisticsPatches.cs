using HarmonyLib;
using HotLavaPlugin.Archipelago.Data;
using HotLavaPlugin.Archipelago.Models.Locations;
using Klei.HotLava.Unlockables;

namespace HotLavaPlugin.Patches
{
    [HarmonyPatch(typeof(Statistics))]
    internal class StatisticsPatches
    {
        [HarmonyPatch(nameof(Statistics.UnlockUnlockable))]
        [HarmonyPostfix]
        public static void UnlockUnlockable_PostFix(Unlockable unlockable, bool display)
        {
            Plugin.Logger.LogInfo("Unlocking: " + unlockable.ToString());

            if (Plugin.ArchipelagoSession != null)
            {
                Location? location = Locations.GetUnlockableLocation(unlockable.m_Key.m_Value);

                if (location != null)
                {
                    Plugin.Logger.LogInfo("Sending AP Check");
                    Plugin.ArchipelagoSession.Locations.CompleteLocationChecks(location.LocationID);
                }
            }
        }

        [HarmonyPatch(nameof(Statistics.HasUnlockedUnlockable))]
        [HarmonyPrefix]
        public static bool HasUnlockedUnlockable_PreFix(Unlockable unlockable, ref bool __result)
        {
            if (Plugin.ArchipelagoSession != null)
            {
                Location? location = Locations.GetUnlockableLocation(unlockable.m_Key.m_Value);

                // If unlockable is not controlled by AP or has been checked by AP, return true
                if (location == null || Plugin.ArchipelagoSession.Locations.AllLocationsChecked.Contains(location.LocationID))
                {
                    __result = true;
                }
                else
                {
                    __result = false;
                }
            }
            else
            {
                __result = false;
            }

            return false;

        }
    }
}
