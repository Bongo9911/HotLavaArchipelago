using HotLavaArchipelagoPlugin.Enums;
using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// A star unlockable by completing a challenge in a course
    /// </summary>
    internal class Star
    {
        [JsonIgnore]
        public Course Course { get; set; } = Course.Default;

        [JsonIgnore]
        public string UnlockableId { get; }
        public string Name { get; }
        public StarType StarType { get; }

        public Star(string unlockableId, string name, StarType starType = StarType.Generic)
        {
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
