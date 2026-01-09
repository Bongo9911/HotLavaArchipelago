using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Locations;
using Klei.HotLava.Unlockables;

namespace HotLavaArchipelagoPlugin.Patches
{
    [HarmonyPatch(typeof(Statistics))]
    internal class StatisticsPatches
    {
        [HarmonyPatch(nameof(Statistics.UnlockUnlockable))]
        [HarmonyPostfix]
        public static void UnlockUnlockable_PostFix(Unlockable unlockable, bool display)
        {
            Plugin.Logger.LogInfo("Unlocking: " + unlockable.ToString());

            if (Multiworld.ArchipelagoSession != null)
            {
                Location? location = Locations.GetUnlockableLocation(unlockable.m_Key.m_Value);

                if (location != null)
                {
                    Plugin.Logger.LogInfo("Sending AP Check");
                    Multiworld.ArchipelagoSession.Locations.CompleteLocationChecks(location.LocationID);
                    Multiworld.CheckGoalCompleted();
                }
            }
        }

        [HarmonyPatch(nameof(Statistics.HasUnlockedUnlockable))]
        [HarmonyPrefix]
        public static bool HasUnlockedUnlockable_PreFix(Unlockable unlockable, ref bool __result)
        {
            if (Multiworld.ArchipelagoSession != null)
            {
                Location? location = Locations.GetUnlockableLocation(unlockable.m_Key.m_Value);

                if (location != null)
                {
                    __result = Multiworld.ArchipelagoSession.Locations.AllLocationsChecked.Contains(location.LocationID);
                    //Don't use built in logic to check if unlocked
                    return false;
                }
            }

            return true;
        }
    }
}
