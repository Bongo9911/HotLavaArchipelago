using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
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
            if (Multiworld.Connected)
            {
                if (handhold.m_Type == Handhold.eType.HORIZONTAL_SWING || handhold.m_Type == Handhold.eType.VERTICAL_SWING)
                {
                    return Multiworld.HasReceivedItem(Items.Swing);
                }
                else
                {
                    return Multiworld.HasReceivedItem(Items.Climb);
                }
            }
            return true;
        }
    }
}
