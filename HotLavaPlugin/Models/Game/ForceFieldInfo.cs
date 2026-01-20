using Newtonsoft.Json;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    /// <summary>
    /// A force field within a world preventing progression
    /// </summary>
    internal class ForceFieldInfo
    {
        [JsonIgnore]
        public WorldInfo World = WorldInfo.Default;

        public string Name { get; }
        [JsonIgnore]
        public Vector3 Position { get; }

        public ForceFieldInfo(string name, Vector3 position)
        {
            Name = name;
            Position = position;
        }
    }
}
