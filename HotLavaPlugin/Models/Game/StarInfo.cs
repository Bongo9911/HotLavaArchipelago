using HotLavaArchipelagoPlugin.Enums;
using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// A star unlockable by completing a challenge in a course
    /// </summary>
    internal class StarInfo
    {
        [JsonIgnore]
        public CourseInfo Course { get; set; } = CourseInfo.Default;

        /// <summary>
        /// The ID of the AP location
        /// </summary>
        public long LocationId { get; set; }
        [JsonIgnore]
        public string UnlockableId { get; }
        public string Name { get; }
        public StarType StarType { get; }

        public StarInfo(long locationId, string unlockableId, string name, StarType starType = StarType.Generic)
        {
            LocationId = locationId;
            UnlockableId = unlockableId;
            Name = name;
            StarType = starType;
        }

        public override string ToString()
        {
            return Course.World.Name + " - " + Course.Name + " - " + Name;
        }
    }
}
