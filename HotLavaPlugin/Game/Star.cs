using Newtonsoft.Json;

namespace HotLavaPlugin.Game
{
    /// <summary>
    /// A star unlockable by completing a challenge in a course
    /// </summary>
    internal class Star
    {
        [JsonIgnore]
        public Course Course { get; set; } = Course.Default;

        public string UnlockableId { get; }
        public string Name { get; }

        public Star(string unlockableId, string name)
        {
            UnlockableId = unlockableId;
            Name = name;
        }

        public override string ToString()
        {
            return Course.World.Name + " - " + Course.Name + " - " + Name;
        }
    }
}
