using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// Information about a world in Hot Lava
    /// </summary>
    internal class WorldInfo
    {
        [JsonIgnore]
        public static WorldInfo Default => new WorldInfo(string.Empty, string.Empty, string.Empty, []);

        private ForceFieldInfo[] _forceFields = [];
        private ForceFieldInfo[] _disabledForceFields = [];

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
        public CourseInfo[] Courses { get; }
        /// <summary>
        /// The force fields within the world
        /// </summary>
        public ForceFieldInfo[] ForceFields
        {
            get { return _forceFields; }
            set
            {
                foreach (ForceFieldInfo forceField in value)
                {
                    forceField.World = this;
                }
                _forceFields = value;
            }
        }
        /// <summary>
        /// The disabled force fields within the world
        /// </summary>
        public ForceFieldInfo[] DisabledForceFields
        {
            get { return _disabledForceFields; }
            set
            {
                foreach (ForceFieldInfo forceField in value)
                {
                    forceField.World = this;
                }
                _disabledForceFields = value;
            }
        }

        public WorldInfo(string unlockableId, string internalName, string name, CourseInfo[] courses)
        {
            UnlockableId = unlockableId;
            InternalName = internalName;
            Name = name;
            Courses = courses;

            foreach (CourseInfo course in Courses)
            {
                course.World = this;
            }
        }
    }
}
