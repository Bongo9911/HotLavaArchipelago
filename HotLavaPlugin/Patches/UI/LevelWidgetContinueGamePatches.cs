using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Models.Options;
using HotLavaArchipelagoPlugin.GameData;
using HotLavaArchipelagoPlugin.Models.Game;
using Klei.HotLava;
using Klei.HotLava.Character;
using Klei.HotLava.Game;
using System.Linq;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.UI
{
    [HarmonyPatch]
    internal class LevelWidgetContinueGamePatches
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method("Klei.HotLava.UI.LevelWidget:ContinueGame");
        }

        /// <summary>
        /// Overrides what world the continue button on the main menu takes the player to
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        public static bool Prefix(object __instance)
        {
            if (Multiworld.Connected)
            {
                MethodInfo continueGameMethod = AccessTools.Method("Klei.HotLava.Continue.Serialization:HasContinueFile");

                if (!(bool)continueGameMethod.Invoke(null, []))
                {
                    FieldInfo statisticsField = typeof(Klei.HotLava.Profiles.Data).GetField("s_PlayerStatistics", BindingFlags.NonPublic | BindingFlags.Static);
                    PlayerStatistics playerStatistics = (PlayerStatistics)statisticsField.GetValue(null);
                    WorldInfo? worldInfo = Worlds.AllWorlds.FirstOrDefault(w => w.InternalName == playerStatistics.m_LastKleiLevel);

                    if (worldInfo != null && !Multiworld.HasReceivedItem(worldInfo.ItemId))
                    {
                        WorldSelect startWorld = Multiworld.Instance.SlotData.StartWorld;
                        WorldInfo? startWorldInfo = Worlds.AllWorlds.FirstOrDefault(w => w.WorldOptionId == startWorld);

                        if (startWorldInfo == null) return true;

                        Singleton<GameSystem>.Instance.RequestLoadLevel(startWorldInfo.InternalName);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
