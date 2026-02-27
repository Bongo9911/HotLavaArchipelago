using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using Klei.HotLava.Character.Modifiers;

namespace HotLavaArchipelagoPlugin.Patches.Character.Modifiers
{
    [HarmonyPatch(typeof(LungeModifier))]
    internal class LungeModifierPatches
    {
        /// <summary>
        /// Disables the ability to use Vault Jump if it has not been unlocked yet
        /// </summary>
        /// <returns>True if unlocked, else false</returns>
        [HarmonyPatch(nameof(LungeModifier.FixedUpdate))]
        [HarmonyPrefix]
        public static bool FixedUpdate_Prefix()
        {
            return !Multiworld.Connected || Multiworld.HasReceivedItem(Items.VaultJump);
        }
    }
}
