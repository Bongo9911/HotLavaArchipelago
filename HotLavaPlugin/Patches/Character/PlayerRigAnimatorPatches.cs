using HarmonyLib;
using Klei.HotLava.Character;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    [HarmonyPatch(typeof(PlayerRigAnimator))]
    internal class PlayerRigAnimatorPatches
    {
        // Vanilla SetRocketJump was gutted to a warning-only no-op when the rocket jump
        // animations were stripped from the character rig to save space, so m_RocketJump never
        // gets set anymore. UpdateAnimator still reads it every frame to drive the Rocket_Jump/
        // Rocket_Reload/Rocket_Fire animator parameters, so writing it back via reflection here
        // is enough to restore whatever animation states are still wired up in the rig.
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
