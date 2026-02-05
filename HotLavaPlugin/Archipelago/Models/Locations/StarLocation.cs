using HotLavaArchipelagoPlugin.Enums;
using HotLavaArchipelagoPlugin.Models.Game;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Locations
{
    internal class StarLocation : UnlockableLocation
    {
        /// <summary>
        /// The course the star is contained in
        /// </summary>
        public CourseInfo Course { get; }
        /// <summary>
        /// The type of star
        /// </summary>
        public StarType StarType { get; }

        public StarLocation(long locationId, StarInfo star)
            : base(locationId, star.UnlockableId, star.ToString())
        {
            Course = star.Course;
            StarType = star.StarType;
        }
    }
}
