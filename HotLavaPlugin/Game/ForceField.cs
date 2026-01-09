using Newtonsoft.Json;

namespace HotLavaArchipelagoPlugin.Game
{
    /// <summary>
    /// A force field within a world preventing progression
    /// </summary>
    internal class ForceField
    {
        [JsonIgnore]
        public World World = World.Default;

        [JsonIgnore]
        public string ObjectName { get; }
        public string Name { get; }

        public ForceField(string objectName, string name)
        {
            ObjectName = objectName;
            Name = name;
        }
    }
}
