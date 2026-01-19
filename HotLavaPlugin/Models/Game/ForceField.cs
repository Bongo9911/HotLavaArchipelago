using Newtonsoft.Json;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// A force field within a world preventing progression
    /// </summary>
    internal class ForceField
    {
        [JsonIgnore]
        public World World = World.Default;

        public string Name { get; }
        [JsonIgnore]
        public Vector3 Position { get; }

        public ForceField(string name, Vector3 position)
        {
            Name = name;
            Position = position;
        }
    }
}
