using Archipelago.MultiClient.Net.Models;
using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Factories;
using Klei.HotLava.Rewards;
using Klei.HotLava.UI;
using System;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches.UI
{
    [HarmonyPatch(typeof(AwardsScreen))]
    internal class AwardsScreenPatches
    {
        [HarmonyPatch("DisplayAwards_Internal")]
        [HarmonyPrefix]
        public static bool DisplayAwards_Internal_Prefix(AwardsScreen __instance)
        {
            if (Multiworld.ArchipelagoSession != null)
            {
                try
                {
                    ScoutedItemInfo? itemInfo = Multiworld.PopAwardsQueue();

                    if (itemInfo == null)
                    {
                        typeof(AwardsScreen).GetMethod("NoAward", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, []);
                        return false;
                    }

                    //string title = string.Empty;
                    string title = "Found Item";
                    RewardVisualization? rewardVisualization = null;

                    //Item is for this world
                    if (itemInfo.IsReceiverRelatedToActivePlayer)
                    {
                        Item? item = Items.GetItem(itemInfo.ItemId);

                        if (item != null)
                        {
                            rewardVisualization = item.GetRewardVisualization(__instance.m_GiftDropData);
                        }
                    }

                    if (rewardVisualization == null)
                    {
                        rewardVisualization = RewardVisualizationFactory.GetArchipelagoReward(__instance.m_GiftDropData);
                    }

                    rewardVisualization.m_ScratchDescription = itemInfo.ItemDisplayName + " for " + itemInfo.Player.Name + " (" + itemInfo.LocationGame + ")";

                    typeof(AwardsScreen).GetField("m_UnlockTitle", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, title);
                    typeof(AwardsScreen).GetMethod("Award", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, [() => rewardVisualization]);

                    return false;
                }
                catch (Exception ex)
                {
                    Plugin.Logger.LogError(ex.ToString());
                    return false;
                }
            }

            return true;
        }
    }
}
