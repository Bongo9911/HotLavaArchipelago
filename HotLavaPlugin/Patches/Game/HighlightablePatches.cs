using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using Klei.HotLava;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(Highlightable))]
    internal class HighlightablePatches
    {
        [HarmonyPatch("Highlight", MethodType.Setter)]
        [HarmonyPrefix]
        public static void Highlight_Prefix(Highlightable __instance, ref bool value)
        {
            if (Multiworld.Connected)
            {
                if (__instance is Handhold handhold)
                {
                    if (handhold.m_Type == Handhold.eType.HORIZONTAL_SWING || handhold.m_Type == Handhold.eType.VERTICAL_SWING)
                    {
                        value &= Multiworld.HasReceivedItem(Items.Swing);
                    }
                    else
                    {
                        value &= Multiworld.HasReceivedItem(Items.Climb);
                    }
                }
            }
        }
    }
}
