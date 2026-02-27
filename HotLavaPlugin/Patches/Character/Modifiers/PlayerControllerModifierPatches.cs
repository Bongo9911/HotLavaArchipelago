using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Gameplay.Modifiers;
using Klei.HotLava.Character.Modifiers;

namespace HotLavaArchipelagoPlugin.Patches.Character.Modifiers
{
    [HarmonyPatch(typeof(PlayerControllerModifier))]
    internal class PlayerControllerModifierPatches
    {
        [HarmonyPatch(nameof(PlayerControllerModifier.MovesetName), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool MovesetName_Prefix(PlayerControllerModifier __instance, ref string __result)
        {
            if (__instance is AbilityRandomizerModifier)
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
            if (__instance is AbilityRandomizerModifier)
            {
                __result = "Unlock abilities by completing location checks";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Controls whether the player can use boost jump based on if they have unlocked it or not
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(PlayerControllerModifier.CanBhop), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool CanBhop_Prefix(PlayerControllerModifier __instance, ref bool __result)
        {
            if (__instance is AbilityRandomizerModifier || __instance is DefaultPlayerControllerModifier)
            {
                __result = !Multiworld.Connected || Multiworld.HasReceivedItem(Items.BoostJump);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Controls whether the player can crouch based on if they have unlocked it or not
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(PlayerControllerModifier.CanCrouch), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool CanCrouch_Prefix(PlayerControllerModifier __instance, ref bool __result)
        {
            if (Multiworld.Connected)
            {
                __result = Multiworld.HasReceivedItem(Items.Crouch);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Controls whether the player can surf based on if they have unlocked it or not
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(PlayerControllerModifier.CanSurf), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool CanSurf_Prefix(PlayerControllerModifier __instance, ref bool __result)
        {
            if (Multiworld.Connected)
            {
                __result = Multiworld.HasReceivedItem(Items.Surf);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Controls whether the player can wall jump based on if they have unlocked it or not
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(PlayerControllerModifier.CanWallRun), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool CanWallRun_Prefix(PlayerControllerModifier __instance, ref bool __result)
        {
            if (Multiworld.Connected)
            {
                __result = Multiworld.HasReceivedItem(Items.WallJump);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Controls whether the player can grab based on if they have unlocked it or not
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns></returns>
        //[HarmonyPatch(nameof(PlayerControllerModifier.CanGrab), MethodType.Getter)]
        //[HarmonyPrefix]
        //public static bool CanGrab_Prefix(PlayerControllerModifier __instance, ref bool __result)
        //{
        //    if (Multiworld.Connected)
        //    {
        //        bool result = Multiworld.HasReceivedItem(Items.Grab);
        //        if (result && __instance is LungeModifier)
        //        {
        //            FieldInfo IsClamberingField = typeof(LungeModifier).GetField("m_IsClambering", BindingFlags.NonPublic | BindingFlags.Instance);
        //            bool isClambering = (bool)IsClamberingField.GetValue(__instance);
        //            result &= !isClambering;
        //        }

        //        __result = result;
        //        return false;
        //    }
        //    return true;
        //}
    }
}
