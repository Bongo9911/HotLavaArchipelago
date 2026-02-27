using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using Klei.HotLava.Character.Modifiers;

namespace HotLavaArchipelagoPlugin.Patches.Character.Modifiers
{
    [HarmonyPatch(typeof(DoubleJumpModifier))]
    internal class DoubleJumpModifierPatches
    {
        /// <summary>
        /// Disables the ability to use Double Jump if it has not been unlocked yet
        /// </summary>
        /// <returns>True if unlocked, else false</returns>
        [HarmonyPatch(nameof(DoubleJumpModifier.Update))]
        [HarmonyPrefix]
        public static bool Update_Prefix()
        {
            return !Multiworld.Connected || Multiworld.HasReceivedItem(Items.DoubleJump);
        }
    }
}
