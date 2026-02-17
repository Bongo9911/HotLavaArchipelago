using HotLavaArchipelagoPlugin.Models.Game;
using Klei.HotLava.Rewards;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class WorldUnlockItem : UnlockableItem
    {
        /// <summary>
        /// The internal name for the world
        /// </summary>
        public string InternalName { get; set; }

        public WorldUnlockItem(long id, WorldInfo world)
            : base(id, world.UnlockableId, "World Unlock - " + world.Name)
        {
            InternalName = world.InternalName;
        }

        public override RewardVisualization? GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            RewardVisualization? rewardVisualization = base.GetRewardVisualization(giftDropVisualization);

            if (rewardVisualization == null)
            {
                //TODO: Custom
            }

            return rewardVisualization;
        }
    }
}
