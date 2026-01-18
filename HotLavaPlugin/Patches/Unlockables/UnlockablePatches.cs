using HarmonyLib;
using Klei.HotLava.Unlockables;

namespace HotLavaArchipelagoPlugin.Patches.Unlockables
{
    [HarmonyPatch(typeof(Unlockable))]
    internal class UnlockablePatches
    {
        //[HarmonyPatch(nameof(Unlockable.LoadVisualization))]
        //[HarmonyPostfix]
        //public static void LoadVisualization_Postfix(Unlockable __instance)
        //{
        //    Plugin.Logger.LogInfo("START UNLOCKABLE");
        //    Plugin.Logger.LogInfo("Key: " + __instance.m_Key.m_Value);
        //    Plugin.Logger.LogInfo("Reward Description: " + __instance.GetType().GetField("m_RewardDescription", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance));
        //    Plugin.Logger.LogInfo("Condition: " + __instance.m_Condition?.ToString());
        //    Plugin.Logger.LogInfo("Item GUID: " + __instance.m_ItemGuid.m_Value);
        //    Plugin.Logger.LogInfo("END UNLOCKABLE");
        //}
    }
}
