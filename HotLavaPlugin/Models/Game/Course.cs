using HotLavaArchipelagoPlugin.Enums;
using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// A course contained within a world
    /// </summary>
    internal class Course
    {
        [JsonIgnore]
        public static Course Default => new Course(string.Empty, []);

        [JsonIgnore]
        public World World { get; set; } = World.Default;
        public string Name { get; }
        public CourseType CourseType { get; }
        public Star[] Stars { get; set; }

        public Course(string name, Star[] stars) : this(name, CourseType.Standard, stars) { }

        public Course(string name, CourseType courseType, Star[] stars)
        {
            Name = name;
            CourseType = courseType;
            Stars = stars;

            foreach (Star star in stars)
            {
                star.Course = this;
            }
        }
    }
}
