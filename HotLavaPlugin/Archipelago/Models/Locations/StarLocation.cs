using HotLavaArchipelagoPlugin.Enums;
using HotLavaArchipelagoPlugin.Models.Game;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Locations
{
    internal class StarLocation : UnlockableLocation
    {
        /// <summary>
        /// The type of star
        /// </summary>
        public StarType StarType { get; }

        public StarLocation(long locationId, StarInfo star)
            : base(locationId, star.UnlockableId, star.ToString())
        {
            StarType = star.StarType;
        }
    }
}
