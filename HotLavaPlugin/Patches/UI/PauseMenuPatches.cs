using HarmonyLib;
using Klei.HotLava.UI;

namespace HotLavaArchipelagoPlugin.Patches.UI
{
    [HarmonyPatch(typeof(PauseMenu))]
    internal class PauseMenuPatches
    {
        [HarmonyPatch(nameof(PauseMenu.InitializeCardPanels))]
        [HarmonyPostfix]
        public static void InitializeCardPanels_Postfix(PauseMenu __instance)
        {
            __instance.m_TargetCardPanel.gameObject.transform.position = new UnityEngine.Vector3(2400f, 46.66663f, 0);
        }
    }
}
