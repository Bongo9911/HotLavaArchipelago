using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using Klei.HotLava.UI;
using System.Linq;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.UI
{
    [HarmonyPatch(typeof(WorldInfoLevelSelect))]
    internal class WorldInfoLevelSelectPatches
    {
        [HarmonyPatch(nameof(WorldInfoLevelSelect.SetLocked))]
        [HarmonyPrefix]
        public static void SetLocked(WorldInfoLevelSelect __instance, ref WorldInfoLevelSelect.LockState locked, ref string info_text)
        {
            FieldInfo field = __instance.GetType().GetField("m_LevelName", BindingFlags.NonPublic | BindingFlags.Instance);
            string levelName = (string)field.GetValue(__instance);

            Plugin.Logger.LogInfo("Level Name: " + levelName);

            if (Multiworld.ArchipelagoSession != null)
            {
                WorldUnlockItem? worldUnlockItem = Items.AllItems.Values
                    .Where(m => m is WorldUnlockItem)
                    .Select(m => (WorldUnlockItem)m)
                    .FirstOrDefault(m => m.InternalName == levelName);

                if (worldUnlockItem != null && Multiworld.ArchipelagoSession.Items.AllItemsReceived.Any(m => m.ItemId == worldUnlockItem.Id))
                {
                    locked = WorldInfoLevelSelect.LockState.Unlocked;
                    info_text = string.Empty;
                }
                else
                {
                    locked = WorldInfoLevelSelect.LockState.ProgressionLocked;
                    info_text = "  Locked ";
                }
            }
        }
    }
}
