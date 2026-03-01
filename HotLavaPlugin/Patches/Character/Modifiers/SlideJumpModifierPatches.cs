using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Gameplay.Modifiers;
using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava.Character;
using Klei.HotLava.Character.Modifiers;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace HotLavaArchipelagoPlugin.Patches.Character.Modifiers
{
    [HarmonyPatch(typeof(SlideJumpModifier))]
    internal class SlideJumpModifierPatches
    {
        [HarmonyPatch(nameof(SlideJumpModifier.FixedUpdate))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> FixedUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getForwardVelocityMultiplier = AccessTools.Method(typeof(SlideJumpModifierPatches), nameof(SlideJumpModifierPatches.GetForwardVelocityMultiplier));

            return new CodeMatcher(instructions).MatchForward(false, new CodeMatch(OpCodes.Ldc_R4, 0.85f))
                 .Set(OpCodes.Call, getForwardVelocityMultiplier)
                 .InstructionEnumeration();
        }

        /// <summary>
        /// Disables the ability to use Slide Jump if it has not been unlocked yet
        /// </summary>
        /// <returns>True if unlocked, else false</returns>
        [HarmonyPatch(nameof(SlideJumpModifier.FixedUpdate))]
        [HarmonyPrefix]
        public static bool FixedUpdate_Prefix()
        {
            return !Multiworld.Connected || Multiworld.HasReceivedItem(Items.SlideJump);
        }

        public static float GetForwardVelocityMultiplier()
        {
            PlayerController? player = HotLavaPlayerHelper.GetLocalPlayer();

            float multiplier = 0.85f;

            if (player == null) return multiplier;

            if (Multiworld.Connected && player.Modifier is AbilityRandomizerModifier && Multiworld.HasReceivedItem(Items.SlideJump))
            {
                multiplier = 1f;
            }

            return multiplier;
        }
    }
}
