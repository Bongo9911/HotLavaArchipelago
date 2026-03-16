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
            ItemMetaDataEntry item = Manager.Instance.LookUpItemByName(InternalName);
            if (item != null)
            {
                return giftDropVisualization.BuildVisualizationForItem(item);
            }

            return null;
        }

        public override void GrantItem()
        {
            ItemMetaDataEntry item = Manager.Instance.LookUpItemByName(InternalName);
            if (item != null)
            {
                typeof(ItemInstance).GetMethod("AddToInventory", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, [item, string.Empty]);
            }
        }
    }
}
