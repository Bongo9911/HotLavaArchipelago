using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using Klei.HotLava;
using Klei.HotLava.Character;
using Klei.HotLava.Character.Modifiers;
using Klei.HotLava.Game;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(HotLavaGameContainer))]
    internal class HotLavaGameContainerPatches
    {
        [HarmonyPatch(nameof(HotLavaGameContainer.OnKilled))]
        [HarmonyPrefix]
        public static void OnKilled_Postfix(PlayerController __instance, OnKilledInfo on_killed_info)
        {
            Plugin.Logger.LogInfo("Player died because: " + on_killed_info.m_Reason);

            if (Multiworld.Connected)
            {
                string deathReason = STRINGS.UI.INGAME.DEATH_REASON.GetReason(on_killed_info.m_Reason);
                if (on_killed_info.m_Player.IsMine && deathReason.Contains("%playera"))
                {
                    deathReason = deathReason.Replace("%playera", Multiworld.Instance.PlayerName).Trim();
                    Multiworld.Instance.SendDeath(deathReason);
                }
            }
        }

        [HarmonyPatch(nameof(HotLavaGameContainer.IsGameModeUnlocked))]
        [HarmonyPrefix]
        public static bool IsGameModeUnlocked_Prefix(GameMode course, ref bool __result)
        {
            if (Multiworld.Connected)
            {
                if (course.Modifier is PogoStickModifier)
                {
                    __result = Multiworld.HasReceivedItem(Items.Pogo);
                }
                else if (course.Modifier is TinyToyModifier)
                {
                    __result = Multiworld.HasReceivedItem(Items.TinyToy);
                }
                else if (course.Modifier is HoverModifier)
                {
                    __result = Multiworld.HasReceivedItem(Items.Jetpack);
                }
                else
                {
                    // Unlock all courses by default for Archipelago
                    __result = true;
                }

                return false;
            }
            return true;
        }
    }
}
