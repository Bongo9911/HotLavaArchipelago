using Klei.HotLava.Inventory;
using Klei.HotLava.Rewards;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class CosmeticItem : Item
    {
        public string InternalName { get; }

        public CosmeticItem(long id, string name, string internalName) : base(id, name)
        {
            InternalName = internalName;
        }

        public override RewardVisualization? GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            ItemMetaDataEntry item = Klei.HotLava.Inventory.Manager.Instance.LookUpItemByName(InternalName);
            if (item != null)
            {
                return giftDropVisualization.BuildVisualizationForItem(item);
            }

            return null;
        }

        public override void GrantItem()
        {
            ItemMetaDataEntry? item = Klei.HotLava.Inventory.Manager.Instance.LookUpItemByName(InternalName);
            if (item != null)
            {
                typeof(ItemInstance).GetMethod("AddToInventory", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, [item, string.Empty]);

                ItemInstance? itemInstance = ItemInstance.TryGetItemInstance(item);

                if (itemInstance != null)
                {
                    switch (item.m_Category)
                    {
                        case eItemCategory.ACCESSORY_HEAD:
                            Klei.HotLava.Inventory.Manager.Instance.LocalCharacterLoadout.m_HeadAccessory = itemInstance.m_Ref;
                            break;
                        case eItemCategory.ACCESSORY_BACK:
                            Klei.HotLava.Inventory.Manager.Instance.LocalCharacterLoadout.m_BackAccessory = itemInstance.m_Ref;
                            break;
                        case eItemCategory.TRINKET:
                            Klei.HotLava.Inventory.Manager.Instance.LocalCharacterLoadout.m_TrinketAccessory = itemInstance.m_Ref;
                            break;
                        default:
                            break;
                    }

                    //TODO: Is there a way we can update the player's cosmetics visually while they are in a course?
                    //PlayerController? playerController = HotLavaPlayerHelper.GetLocalPlayer();

                    //if (playerController != null)
                    //{
                    //    Player player = (Player)typeof(PlayerController).GetField("m_Owner", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(playerController);
                    //    typeof(Klei.HotLava.Online.Manager).GetMethod("SetPlayerAppearance", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, [player, playerController]);
                    //    Plugin.Logger.LogInfo("Updating player appearance");

                    //    typeof(PlayerRigAccessories).GetMethod("SetLoadout", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(playerController.Rig.Accessories, [Klei.HotLava.Inventory.Manager.Instance.LocalCharacterLoadout]);
                    //}
                }
            }
        }
    }
}
