using Klei.HotLava.Character.Progression;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class XpShardItem : Item
    {
        public XpShardItem(long id) : base(id, "XP Shard") { }

        public override void GrantItem()
        {
            CharacterStatistics.HitShard();
        }
    }
}
