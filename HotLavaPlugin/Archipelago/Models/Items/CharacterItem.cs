using Klei.HotLava.Audio;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    /// <summary>
    /// An item that unlocks a character
    /// </summary>
    internal class CharacterItem : CosmeticItem
    {
        public eVoiceCharacter CharacterId { get; set; }

        public CharacterItem(long id, string name, string internalName, eVoiceCharacter characterId) : base(id, name, internalName)
        {
            CharacterId = characterId;
        }
    }
}
