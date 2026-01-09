using HarmonyLib;
using Klei.HotLava;
using Klei.HotLava.Game;
using Klei.HotLava.Gameplay;
using System.Collections.Generic;
using System.Reflection;

namespace HotLavaPlugin.Patches
{
    [HarmonyPatch(typeof(GameModeCompletedGate))]
    internal class GameModeCompletedGatePatches
    {
        private static long Counter = 0;

        [HarmonyPatch(nameof(GameModeCompletedGate.UpdateRequirements))]
        [HarmonyPrefix]
        public static bool UpdateRequirements_Prefix(GameModeCompletedGate __instance)
        {
            long barrierID = Counter++;

            LevelMetaData currentLevel = Plugin.GetCurrentLevelMetaData();
            bool shouldDeactivateBarrier = false;

            foreach (GameModeContainer gameModeContainer in __instance.m_RequiredGameModes)
            {
                Plugin.Logger.LogInfo("[" + barrierID + "] World: " + currentLevel.name.Replace("_meta_data", ""));
                Plugin.Logger.LogInfo("[" + barrierID + "] Level: " + currentLevel.GetTranslatedName(gameModeContainer.m_GameMode));
                Plugin.Logger.LogInfo("[" + barrierID + "] Name: " + __instance.name);
                shouldDeactivateBarrier = shouldDeactivateBarrier || gameModeContainer.m_GameMode.m_ID == "tutorial.Course 4";
            }

            __instance.ClearList();

            GameModeCompletedRequirement completedRequirement = UnityEngine.Object.Instantiate<GameModeCompletedRequirement>(__instance.m_RequirementTemplate, __instance.m_RequirementsList.transform);
            completedRequirement.m_GameModeName.text = "Unlock via Archipelago";
            completedRequirement.m_RequirementIcon.interactable = false;
            completedRequirement.gameObject.SetActive(true);

            FieldInfo m_RowsField = typeof(GameModeCompletedGate).GetField("m_Rows", BindingFlags.NonPublic | BindingFlags.Instance);
            List<GameModeCompletedRequirement> m_Rows = (List<GameModeCompletedRequirement>)m_RowsField.GetValue(__instance);
            m_Rows.Add(completedRequirement);

            if (shouldDeactivateBarrier)
            {
                __instance.PlayDeactivationAnimation();
            }
            else
            {
                __instance.OnEnableConditionsMet(shouldDeactivateBarrier);
            }

            return false;
        }
    }
}
