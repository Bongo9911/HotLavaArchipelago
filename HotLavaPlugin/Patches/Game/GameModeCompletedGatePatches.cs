using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Extensions;
using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava;
using Klei.HotLava.Game;
using Klei.HotLava.Gameplay;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    /// <summary>
    /// Applies patches to the logic surrounding force fields
    /// </summary>
    [HarmonyPatch(typeof(GameModeCompletedGate))]
    internal class GameModeCompletedGatePatches
    {
        private static long Counter = 0;

        [HarmonyPatch(nameof(GameModeCompletedGate.UpdateRequirements))]
        [HarmonyPrefix]
        public static bool UpdateRequirements_Prefix(GameModeCompletedGate __instance)
        {
            long barrierID = Counter++;

            LevelMetaData? currentLevel = LevelHelper.GetCurrentLevelMetaData();

            Plugin.Logger.LogInfo("[" + barrierID + "] World: " + currentLevel?.name.Replace("_meta_data", ""));
            Plugin.Logger.LogInfo("[" + barrierID + "] Name: " + __instance.name);
            Plugin.Logger.LogInfo("[" + barrierID + "] Position: " + __instance.transform.position.x + ", " + __instance.transform.position.y + ", " + __instance.transform.position.z);

            foreach (GameModeContainer gameModeContainer in __instance.m_RequiredGameModes)
            {
                Plugin.Logger.LogInfo("[" + barrierID + "] Level: " + currentLevel?.GetTranslatedName(gameModeContainer.m_GameMode));
            }

            if (Multiworld.ArchipelagoSession != null)
            {
                ForceFieldItem? forceFieldItem = Items.GetItems<ForceFieldItem>()
                    .FirstOrDefault(m => m.InternalWorldName == currentLevel?.GetWorldName() && m.Position == __instance.transform.position);

                if (forceFieldItem != null)
                {
                    bool isForceFieldUnlocked = Multiworld.ArchipelagoSession.Items.AllItemsReceived.Any(m => m.ItemId == forceFieldItem.Id);

                    __instance.ClearList();

                    GameModeCompletedRequirement completedRequirement = UnityEngine.Object.Instantiate<GameModeCompletedRequirement>(__instance.m_RequirementTemplate, __instance.m_RequirementsList.transform);
                    completedRequirement.m_GameModeName.text = "Unlock via Archipelago";
                    completedRequirement.m_RequirementIcon.interactable = isForceFieldUnlocked;
                    completedRequirement.gameObject.SetActive(true);

                    FieldInfo m_RowsField = typeof(GameModeCompletedGate).GetField("m_Rows", BindingFlags.NonPublic | BindingFlags.Instance);
                    List<GameModeCompletedRequirement> m_Rows = (List<GameModeCompletedRequirement>)m_RowsField.GetValue(__instance);
                    m_Rows.Add(completedRequirement);

                    __instance.OnEnableConditionsMet(isForceFieldUnlocked);

                    //if (isForceFieldUnlocked)
                    //{
                    //    //TODO: check if this animation has already been played
                    //    //__instance.PlayDeactivationAnimation();
                    //    __instance.OnEnableConditionsMet(true);
                    //}
                    //else
                    //{
                    //    __instance.OnEnableConditionsMet(false);
                    //}

                    return false;
                }
            }

            return true;
        }
    }
}
