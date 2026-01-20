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
            Texture2D texture = new Texture2D(256, 256);
            texture.LoadImage(image);

            ItemMetaDataEntry itemMetaDataEntry = ScriptableObject.CreateInstance<ItemMetaDataEntry>();
            itemMetaDataEntry.m_Category = eItemCategory.DECAL;
            itemMetaDataEntry.m_Sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            RewardVisualization rewardVisualization = giftDropVisualization.ItemViz[(int)eItemCategory.DECAL].ShallowCopy();
            rewardVisualization.Load(itemMetaDataEntry);

            return rewardVisualization;
        }

        public static RewardVisualization GetArchipelagoReward(GiftDropVisualization giftDropVisualization)
        {
            return FromImage(giftDropVisualization, Properties.Resources.ArchipelagoLogo);
        }
    }
}
