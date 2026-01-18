using HotLavaArchipelagoPlugin.Extensions;
using Klei.HotLava;
using Klei.HotLava.Game;
using Klei.L10n;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Helpers
{
    internal static class LevelHelper
    {
        public static LevelMetaData? GetCurrentLevelMetaData()
        {
            PropertyInfo property = typeof(Info).GetProperty("CurrentLevelMetaData", BindingFlags.NonPublic | BindingFlags.Static);
            return (LevelMetaData?)property.GetValue(null);
        }

        /// <summary>
        /// Gets the internal name of the world the player is currently located in
        /// </summary>
        /// <returns>The name of the world, if they are in one, else null</returns>
        public static string? GetCurrentWorldName()
        {
            LevelMetaData? levelMetaData = GetCurrentLevelMetaData();
            return levelMetaData?.GetWorldName();
        }

        public static string? GameModeToValidStringKeyElement(GameMode mode)
        {
            int num = mode.m_ID.IndexOf(".");
            return num > -1 ? "GAMEMODE_" + LocConversions.NameToValidStringKeyElement(mode.m_ID.Substring(num + 1)) : null;
        }
    }
}
