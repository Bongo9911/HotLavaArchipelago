using Klei.HotLava.Audio;
using Klei.HotLava.Rewards;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    /// <summary>
    /// An item that unlocks a character
    /// </summary>
    internal class CharacterItem : Item
    {
        public eVoiceCharacter CharacterId { get; set; }

        public CharacterItem(long id, string name, eVoiceCharacter characterId) : base(id, name)
        {
            CharacterId = characterId;
        }

        public override RewardVisualization? GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            //TODO:
            return null;
        }

        public override void GrantItem() { }
    }
}
