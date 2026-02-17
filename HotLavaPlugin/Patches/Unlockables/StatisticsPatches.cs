using Archipelago.MultiClient.Net.Models;
using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Locations;
using Klei.HotLava;
using Klei.HotLava.Online;
using Klei.HotLava.Unlockables;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.Unlockables
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

                //Don't recheck locations that have already been checked
                if (location != null && !Multiworld.ArchipelagoSession.Locations.AllLocationsChecked.Contains(location.LocationID))
                {
                    Plugin.Logger.LogInfo("Sending AP Check for: " + location.LocationID);
                    Multiworld.SendLocationCheck(location.LocationID);

                    ScoutedItemInfo? scoutedItemInfo = Multiworld.GetItemForLocation(location.LocationID);
                    if (scoutedItemInfo != null)
                    {
                        Plugin.Logger.LogInfo("Queueing scouted itme: " + scoutedItemInfo.ItemDisplayName);
                        Multiworld.QueueAwardItem(scoutedItemInfo);
                    }
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

        /// <summary>
        /// Overrides the number of stars that the player has collected in the UI
        /// </summary>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch("ComputeUnlockedChallengesCount")]
        [HarmonyPrefix]
        public static bool ComputeUnlockedChallengesCount_Prefix(ref uint __result)
        {
            if (Multiworld.ArchipelagoSession != null)
            {
                uint totalUnlocked = 999;

                typeof(Statistics).GetField("s_UnlockedChallengeCount", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, totalUnlocked);
                typeof(Statistics).GetField("s_ComputedChallengeCount", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, true);

                __result = totalUnlocked;

                Player player = (Player)typeof(Player).GetMethod("GetPlayer", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, [DistributionPlatform.LocalUser]);

                if (player != null)
                {
                    typeof(Player).GetField("m_TotalChallengesUnlocked", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(player, (ushort)totalUnlocked);
                }

                return false;
            }

            return true;
        }
    }
}
