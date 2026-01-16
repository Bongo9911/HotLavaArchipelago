using Archipelago.MultiClient.Net.Models;
using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava.Inventory;
using Klei.HotLava.Rewards;
using Klei.HotLava.UI;
using Klei.HotLava.Unlockables;
using System;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches
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

                    string title = string.Empty;
                    RewardVisualization? rewardVisualization = null;

                    //Item is for this world
                    if (itemInfo.IsReceiverRelatedToActivePlayer)
                    {
                        Item? item = Items.GetItem(itemInfo.ItemId);

                        if (item != null && item is UnlockableItem unlockableItem)
                        {
                            Unlockable? unlockable = UnlockableHelper.GetUnlockableById(unlockableItem.UnlockableId);

                            if (unlockable != null)
                            {
                                //TODO: customize this text (Right now for world unlocks it tells you you are getting this for hitting a certain rank)

                                title = string.Format(STRINGS.UI.INGAME.CHARACTER_CANVAS.AWARD_SCREEN.UNLOCK_TITLE_FMT, unlockable.ToString());
                                rewardVisualization = unlockable.LoadVisualization();
                            }
                        }
                    }

                    if (rewardVisualization == null)
                    {
                        //Top text
                        title = "Found Item";

                        ItemMetaDataEntry itemMetaDataEntry = ScriptableObject.CreateInstance<ItemMetaDataEntry>();
                        itemMetaDataEntry.m_Category = eItemCategory.DECAL;

                        Texture2D texture = new Texture2D(256, 256);
                        texture.LoadImage(Properties.Resources.ArchipelagoLogo);
                        itemMetaDataEntry.m_Sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                        rewardVisualization = __instance.m_GiftDropData.ItemViz[(int)eItemCategory.DECAL].ShallowCopy();
                        rewardVisualization.Load(itemMetaDataEntry);
                        //Bottom text
                        rewardVisualization.m_ScratchDescription = itemInfo.ItemDisplayName + " for " + itemInfo.Player.Name + " (" + itemInfo.LocationGame + ")";
                    }

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
