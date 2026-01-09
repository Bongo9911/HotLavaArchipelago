using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava;
using Klei.HotLava.Character;
using Klei.HotLava.Game;

namespace HotLavaArchipelagoPlugin.Patches
{
    [HarmonyPatch(typeof(HotLavaGameContainer))]
    internal class HotLavaGameContainerPatches
    {
        [HarmonyPatch(nameof(HotLavaGameContainer.OnKilled))]
        [HarmonyPostfix]
        public static void OnKilled_Postfix(PlayerController __instance, OnKilledInfo on_killed_info)
        {
            Plugin.Logger.LogInfo("Player died because: " + on_killed_info.m_Reason);

            string deathReason = STRINGS.UI.INGAME.DEATH_REASON.GetReason(on_killed_info.m_Reason);
            if (deathReason.Contains("%playera"))
            {
                //Should do death link
                UIHelper.SendNotificationMessage("L NERD " + deathReason.Replace("%playera ", ""));
                UIHelper.ShowPopup("L NERD DIED");
            }
        }

        [HarmonyPatch(nameof(HotLavaGameContainer.IsGameModeUnlocked))]
        [HarmonyPrefix]
        public static bool IsGameModeUnlocked_Prefix(GameMode course, ref bool __result)
        {
            if (Multiworld.ArchipelagoSession != null)
            {
                // Unlock all courses by default for Archipelago
                __result = true;
                return false;
            }
            return true;
        }
    }
}
