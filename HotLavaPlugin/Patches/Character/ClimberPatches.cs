using HarmonyLib;
using Klei.HotLava;
using Klei.HotLava.Character;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    [HarmonyPatch(typeof(Climber))]
    internal class ClimberPatches
    {
        [HarmonyPatch(nameof(Climber.AttachHandhold))]
        [HarmonyPrefix]
        public static bool AttachHandhold_Prefix(Climber __instance, Vector3 contact_point, Handhold handhold)
        {
            //Prevent using horizontal swings
            if (handhold.m_Type == Handhold.eType.HORIZONTAL_SWING)
            {
                return false;
            }
            return true;
        }
    }
}
