using HotLavaArchipelagoPlugin.Enums;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Locations
{
    internal class StarLocation : UnlockableLocation
    {
        public StarType StarType { get; }
        public StarLocation(long locationId, string unlockableId, string unlockableDescription, StarType starType)
            : base(locationId, unlockableId, unlockableDescription)
        {
            StarType = starType;
        }
    }
}
