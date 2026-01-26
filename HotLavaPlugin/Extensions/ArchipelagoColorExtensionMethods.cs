using Archipelago.MultiClient.Net.Models;

namespace HotLavaArchipelagoPlugin.Extensions
{
    internal static class ArchipelagoColorExtensionMethods
    {
        public static string ToHexColorCode(this Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
