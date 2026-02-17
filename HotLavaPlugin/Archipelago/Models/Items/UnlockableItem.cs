using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava.Rewards;
using Klei.HotLava.Unlockables;
using System.Linq;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class UnlockableItem : Item
    {
        /// <summary>
        /// The identifier for the unlockable
        /// </summary>
        public string UnlockableId { get; }

        public UnlockableItem(long id, string unlockabledId, string name) : base(id, name)
        {
            UnlockableId = unlockabledId;
        }

        /// <inheritdoc/>
        public override void GrantItem()
        {
            Unlockable unlockable = Statistics.AllUnlockables.FirstOrDefault(u => u.m_Key.m_Value == UnlockableId);

            Statistics.UnlockUnlockable(unlockable, true);
        }

        /// <inheritdoc/>
        public override RewardVisualization? GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            Unlockable? unlockable = UnlockableHelper.GetUnlockableById(UnlockableId);

            if (unlockable != null)
            {
                return unlockable.LoadVisualization();
            }
            else
            {
                return null;
            }
        }
    }
}
