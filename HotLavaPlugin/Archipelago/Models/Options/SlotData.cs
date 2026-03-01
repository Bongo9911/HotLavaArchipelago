using Newtonsoft.Json;
using System.Collections.Generic;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Options
{
    internal class SlotData
    {
        [JsonProperty("world_unlock_logic")]
        public int WorldUnlockLogic { get; set; }
        [JsonProperty("force_field_logic")]
        public ForceFieldLogicOption ForceFieldLogicOption { get; set; } = ForceFieldLogicOption.Disabled;
        [JsonProperty("enabled_worlds")]
        public IEnumerable<string> EnabledWorlds { get; set; } = [];
        [JsonProperty("start_world")]
        public WorldSelect StartWorld { get; set; } = WorldSelect.GymClass;
        [JsonProperty("last_world")]
        public WorldSelect LastWorld { get; set; } = WorldSelect.MasterClass;
        [JsonProperty("death_link")]
        public int DeathLink { get; set; } = 0;
    }
}
