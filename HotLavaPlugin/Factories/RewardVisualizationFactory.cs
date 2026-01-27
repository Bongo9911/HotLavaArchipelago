using Klei.HotLava.Inventory;
using Klei.HotLava.Rewards;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Factories
{
    internal static class RewardVisualizationFactory
    {
        /// <summary>
        /// Generates a reward visualization from an image
        /// </summary>
        /// <param name="giftDropVisualization">The gift drop visualization base</param>
        /// <param name="image">The image to use for generation</param>
        /// <returns>The reward visualization</returns>
        public static RewardVisualization FromImage(GiftDropVisualization giftDropVisualization, byte[] image)
        {
            return FromSprite(giftDropVisualization, SpriteFactory.FromImage(image));
        }

        /// <summary>
        /// Generates a reward visualization from a sprite
        /// </summary>
        /// <param name="giftDropVisualization">The gift drop visualization base</param>
        /// <param name="sprite">The sprite to use for generation</param>
        /// <returns>The reward visualization</returns>
        public static RewardVisualization FromSprite(GiftDropVisualization giftDropVisualization, Sprite sprite)
        {
            ItemMetaDataEntry itemMetaDataEntry = ScriptableObject.CreateInstance<ItemMetaDataEntry>();
            itemMetaDataEntry.m_Category = eItemCategory.DECAL;
            itemMetaDataEntry.m_Sprite = sprite;

            RewardVisualization rewardVisualization = giftDropVisualization.ItemViz[(int)eItemCategory.DECAL].ShallowCopy();
            rewardVisualization.Load(itemMetaDataEntry);

            return rewardVisualization;
        }

        public static RewardVisualization GetArchipelagoReward(GiftDropVisualization giftDropVisualization)
        {
            return FromSprite(giftDropVisualization, SpriteFactory.GetArchipelagoSprite());
        }
    }
}
