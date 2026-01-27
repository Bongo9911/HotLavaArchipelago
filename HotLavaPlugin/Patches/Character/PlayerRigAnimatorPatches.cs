using HarmonyLib;
using Klei.HotLava.Character;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    [HarmonyPatch(typeof(PlayerRigAnimator))]
    internal class PlayerRigAnimatorPatches
    {
        [HarmonyPatch(nameof(PlayerRigAnimator.SetRocketJump))]
        [HarmonyPrefix]
        public static bool SetRocketJump_Prefix(PlayerRigAnimator __instance, bool to)
        {
            Animator animator = (Animator)typeof(PlayerRigAnimator).GetField("m_Animator", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

            if (animator == null || !animator.isInitialized)
                return false;

            typeof(PlayerRigAnimator).GetField("m_RocketJump", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, to);

            return false;
        }
    }
}
