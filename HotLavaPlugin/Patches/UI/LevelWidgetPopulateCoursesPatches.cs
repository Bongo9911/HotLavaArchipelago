using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Models.Options;
using HotLavaArchipelagoPlugin.Extensions;
using HotLavaArchipelagoPlugin.GameData;
using HotLavaArchipelagoPlugin.Models.Game;
using Klei.HotLava;
using Klei.HotLava.Game;
using System.Linq;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.UI
{
    [HarmonyPatch]
    internal class LevelWidgetPopulateCoursesPatches
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method("Klei.HotLava.UI.LevelWidget:PopulateCourses");
        }

        /// <summary>
        /// Overrides the list of courses for "Continue" on the main menu if the player has not unlocked the world the continue is associated to
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="level"></param>
        /// <param name="course_index"></param>
        /// <returns></returns>
        public static bool Prefix(object __instance, ref LevelMetaData level, ref sbyte course_index)
        {
            if (Multiworld.Connected)
            {
                string levelName = level.GetWorldName();
                WorldInfo? worldInfo = Worlds.AllWorlds.FirstOrDefault(w => w.InternalName == levelName);

                Plugin.Logger.LogInfo("Checking if player has world: " + levelName);

                if (worldInfo != null && !Multiworld.HasReceivedItem(worldInfo.ItemId))
                {
                    WorldSelect startWorld = Multiworld.Instance.SlotData.StartWorld;
                    WorldInfo? startWorldInfo = Worlds.AllWorlds.FirstOrDefault(w => w.WorldOptionId == startWorld);

                    if (startWorldInfo == null) return true;

                    MethodInfo getLevelMetaData = typeof(Info).GetMethod("GetLevelMetaData", BindingFlags.Static | BindingFlags.NonPublic);
                    LevelMetaData startLevel = (LevelMetaData)getLevelMetaData.Invoke(null, [startWorldInfo.InternalName]);

                    if (startLevel != null)
                    {
                        level = startLevel;
                        course_index = -1;
                    }
                }
            }

            return true;
        }
    }
}
