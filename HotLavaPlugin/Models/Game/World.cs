using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// Information about a world in Hot Lava
    /// </summary>
    internal class World
    {
        [JsonIgnore]
        public static World Default => new World(string.Empty, string.Empty, string.Empty, []);

        private ForceField[] _forceFields = [];

        /// <summary>
        /// The ID of the unlock for the world
        /// </summary>
        [JsonIgnore]
        public string UnlockableId { get; set; }
        /// <summary>
        /// The internal name for the world
        /// </summary>
        [JsonIgnore]
        public string InternalName { get; set; }
        /// <summary>
        /// The display name for the world
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The courses the world contains
        /// </summary>
        public Course[] Courses { get; }
        /// <summary>
        /// The force fields within the world
        /// </summary>
        public ForceField[] ForceFields
        {
            get { return _forceFields; }
            set
            {
                foreach (ForceField forceField in value)
                {
                    forceField.World = this;
                }
                _forceFields = value;
            }
        }

        public World(string unlockableId, string internalName, string name, Course[] courses)
        {
            UnlockableId = unlockableId;
            InternalName = internalName;
            Name = name;
            Courses = courses;

            foreach (Course course in Courses)
            {
                course.World = this;
            }
        }
    }
}
