using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Game
{
    /// <summary>
    /// Information about a world in Hot Lava
    /// </summary>
    internal class World
    {
        [JsonIgnore]
        public static World Default => new World(string.Empty, []);

        public string Name { get; }
        public Course[] Courses { get; }
        public ForceField[] ForceFields { get; set; } = [];

        public World(string name, Course[] courses)
        {
            Name = name;
            Courses = courses;

            foreach (Course course in Courses)
            {
                course.World = this;
            }
        }
    }
}
