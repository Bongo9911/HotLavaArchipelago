using HotLavaArchipelagoPlugin.Factories;
using Klei.HotLava.Character.Progression;
using Klei.HotLava.Rewards;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class XpShardItem : Item
    {
        public XpShardItem(long id) : base(id, "XP Shard") { }

        /// <inheritdoc/>
        public override void GrantItem()
        {
            CharacterStatistics.HitShard();
        }

        /// <inheritdoc/>
        public override RewardVisualization GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            return RewardVisualizationFactory.FromImage(giftDropVisualization, Properties.Resources.XPShard);
        }
    }
}
