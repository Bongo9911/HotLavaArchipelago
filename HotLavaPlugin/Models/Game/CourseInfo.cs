using HotLavaArchipelagoPlugin.Enums;
using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// A course contained within a world
    /// </summary>
    internal class CourseInfo
    {
        [JsonIgnore]
        public static CourseInfo Default => new CourseInfo(string.Empty, []);

        [JsonIgnore]
        public WorldInfo World { get; set; } = WorldInfo.Default;
        public string Name { get; }
        public CourseType CourseType { get; }
        public StarInfo[] Stars { get; set; }

        public CourseInfo(string name, StarInfo[] stars) : this(name, CourseType.Standard, stars) { }

        public CourseInfo(string name, CourseType courseType, StarInfo[] stars)
        {
            Name = name;
            CourseType = courseType;
            Stars = stars;

            foreach (StarInfo star in stars)
            {
                star.Course = this;
            }
        }
    }
}
