using HotLavaArchipelagoPlugin.Factories;
using Klei.HotLava.Rewards;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class AbilityItem : Item
    {
        private readonly byte[] _icon;

        public AbilityItem(long id, string name, byte[] icon) : base(id, name)
        {
            _icon = icon;
        }

        public override RewardVisualization? GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            return RewardVisualizationFactory.FromImage(giftDropVisualization, _icon);
        }

        public override void GrantItem()
        {
            //TODO: What should this do?
        }
    }
}
