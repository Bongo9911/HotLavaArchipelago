using HarmonyLib;
using HotLavaArchipelagoPlugin.Gameplay.Modifiers;
using Klei.HotLava.Character.Modifiers;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    [HarmonyPatch(typeof(PlayerControllerModifier))]
    internal class PlayerControllerModifierPatches
    {
        [HarmonyPatch(nameof(PlayerControllerModifier.MovesetName), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool MovesetName_Prefix(PlayerControllerModifier __instance, ref string __result)
        {
            if (__instance is ArchipelagoModifier)
            {
                __result = "Ability Randomizer";
                return false;
            }
            return true;
        }

        [HarmonyPatch(nameof(PlayerControllerModifier.MovesetDescription), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool MovesetDescription_Prefix(PlayerControllerModifier __instance, ref string __result)
        {
            if (__instance is ArchipelagoModifier)
            {
                __result = "Unlock abilities by completing location checks";
                return false;
            }
            return true;
        }
    }
}
