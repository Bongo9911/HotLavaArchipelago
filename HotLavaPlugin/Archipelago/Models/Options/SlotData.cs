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
        public int StartWorld { get; set; } = 0;
        [JsonProperty("last_world")]
        public int LastWorld { get; set; } = 0;
        [JsonProperty("death_link")]
        public int DeathLink { get; set; } = 0;
    }
}
