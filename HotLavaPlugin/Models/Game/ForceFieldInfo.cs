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

        /// <summary>
        /// The ID of the AP item
        /// </summary>
        public long ItemId { get; set; }
        public string Name { get; }
        [JsonIgnore]
        public Vector3 Position { get; }

        public ForceFieldInfo(long itemId, string name, Vector3 position)
        {
            ItemId = itemId;
            Name = name;
            Position = position;
        }
    }
}
