using Klei.HotLava;

namespace HotLavaArchipelagoPlugin.Extensions
{
    internal static class LevelMetaDataExtensionMethods
    {
        public static string GetWorldName(this LevelMetaData levelMetaData)
        {
            return levelMetaData.name.Replace("_meta_data", "");
        }
    }
}
