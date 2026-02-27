using HarmonyLib;
using Klei.HotLava;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(Highlightable))]
    internal class HighlightablePatches
    {
        [HarmonyPatch("Highlight", MethodType.Setter)]
        [HarmonyPrefix]
        public static bool Highlight_Prefix(Highlightable __instance, ref bool value)
        {
            if (__instance is Handhold handhold)
            {
                if (handhold.m_Type == Handhold.eType.HORIZONTAL_SWING)
                {
                    value = false;
                    return false;
                }
            }
            return true;
        }
    }
}
