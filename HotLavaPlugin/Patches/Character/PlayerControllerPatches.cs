using FMOD.Studio;
using HarmonyLib;
using HotLavaArchipelagoPlugin.Gameplay.Modifiers;
using Klei.HotLava.Audio;
using Klei.HotLava.Character;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    /// <summary>
    /// Modifies functions ran on the player controller
    /// </summary>
    [HarmonyPatch(typeof(PlayerController))]
    internal class PlayerControllerPatches
    {
        [HarmonyPatch("StartCrouching")]
        [HarmonyPrefix]
        public static bool StartCrouching_Prefix(PlayerController __instance)
        {
            //TODO: check if crouching is unlocked
            //return false;
            return true;
        }

        [HarmonyPatch("StartCrouching")]
        [HarmonyPostfix]
        public static void StartCrouching_Postfix(PlayerController __instance)
        {
            FieldInfo m_IsSlidingFieldInfo = typeof(PlayerController).GetField("m_IsSliding", BindingFlags.Instance | BindingFlags.NonPublic);

            m_IsSlidingFieldInfo.SetValue(__instance, __instance.Modifier is ArchipelagoModifier && __instance.Grounded && __instance.GetCachedInput().y > 0f && !__instance.Surfing);
            if ((bool)m_IsSlidingFieldInfo.GetValue(__instance))
            {
                PlayerRig playerRig = (PlayerRig)typeof(PlayerController).GetField("m_PlayerRig", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);

                EventInstance eventInstance = Runtime
                    .PlayOneShotAttached(Klei.HotLava.Audio.Event.CROUCHSLIDE_2D3D, playerRig.gameObject)
                    .SetAudioParameter("3d", (!__instance.IsCameraAttached) ? 1 : 0)
                    .StartAndRelease();

                typeof(PlayerController).GetField("m_SlideInstance", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(__instance, eventInstance);
            }
        }
    }
}
