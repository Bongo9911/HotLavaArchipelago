using HarmonyLib;
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

            //Remove speed loss from Slide Jumping (TODO: Make this only be when using Ability Randomizer?)
            return new CodeMatcher(instructions).MatchForward(false, new CodeMatch(OpCodes.Ldc_R4, 0.85f))
                 .Set(OpCodes.Call, getForwardVelocityMultiplier)
                 .InstructionEnumeration();
        }

        public static float GetForwardVelocityMultiplier()
        {
            return 1f;
        }
    }
}
