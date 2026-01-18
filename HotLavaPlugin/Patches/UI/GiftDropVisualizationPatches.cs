using HarmonyLib;
using Klei.HotLava.Rewards;

namespace HotLavaArchipelagoPlugin.Patches
{
    [HarmonyPatch(typeof(GiftDropVisualization))]
    internal class GiftDropVisualizationPatches
    {
        //[HarmonyPatch(nameof(GiftDropVisualization.BuildVisualizationForItem))]
        //[HarmonyPostfix]
        //public static void BuildVisualizationForItem_Postfix(GiftDropVisualization __instance, ItemMetaDataEntry item)
        //{
        //    Plugin.Logger.LogInfo("START ITEM");
        //    Plugin.Logger.LogInfo("Category: " + item.m_Category);
        //    Plugin.Logger.LogInfo("Pretty name: " + item.PrettyName);
        //    Plugin.Logger.LogInfo("Description: " + item.Description);
        //    Plugin.Logger.LogInfo("Rarity: " + item.m_Rarity);
        //    Plugin.Logger.LogInfo("Resource: " + item.m_Rarity);
        //    Plugin.Logger.LogInfo("Hash: " + item.m_Hash);
        //    Plugin.Logger.LogInfo("Taunt emotion: " + item.m_TauntEmotion);
        //    Plugin.Logger.LogInfo("Card Type: " + item.m_CardType);
        //    Plugin.Logger.LogInfo("Texture: " + item.m_ExternalTexture?.name);
        //    Plugin.Logger.LogInfo("END ITEM");
        //}
    }
}
