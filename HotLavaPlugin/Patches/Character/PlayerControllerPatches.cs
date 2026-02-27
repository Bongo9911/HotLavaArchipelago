using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Gameplay.Modifiers;
using Klei.HotLava.Character;
using Klei.HotLava.Character.Modifiers;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    /// <summary>
    /// Modifies functions ran on the player controller
    /// </summary>
    [HarmonyPatch(typeof(PlayerController))]
    internal class PlayerControllerPatches
    {
        [HarmonyPatch("StartCrouching")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> StartCrouching_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo canSlideMethod = AccessTools.Method(typeof(PlayerControllerPatches), nameof(PlayerControllerPatches.CanSlide));

            return new CodeMatcher(instructions).MatchForward(false, new CodeMatch(OpCodes.Isinst, typeof(SlideJumpModifier)))
                 .Set(OpCodes.Call, canSlideMethod)
                 .InstructionEnumeration();
        }

        public static bool CanSlide(PlayerController playerController)
        {
            bool canSlide = playerController.Modifier is SlideJumpModifier || playerController.Modifier is AbilityRandomizerModifier;

            if (canSlide && Multiworld.Connected)
            {
                canSlide &= Multiworld.HasReceivedItem(Items.SlideJump);
            }

            return canSlide;
        }
    }
}
