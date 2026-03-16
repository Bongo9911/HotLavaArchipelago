using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.GameData;
using HotLavaArchipelagoPlugin.Models.Game;
using HotLavaArchipelagoPlugin.Properties;
using Klei.HotLava.Audio;
using System.Collections.Generic;
using System.Linq;

namespace HotLavaArchipelagoPlugin.Archipelago.Data
{
    internal static class Items
    {
        private static Dictionary<long, Item>? _allItems = null;

        public static AbilityItem DoubleJump { get; } = new AbilityItem(10, "Double Jump", Resources.DoubleJump);
        public static AbilityItem BoostJump { get; } = new AbilityItem(11, "Boost Jump", Resources.BoostJump);
        public static AbilityItem SlideJump { get; } = new AbilityItem(12, "Slide Jump", Resources.SlideJump);
        public static AbilityItem VaultJump { get; } = new AbilityItem(13, "Vault Jump", Resources.VaultJump);

        public static AbilityItem Crouch { get; } = new AbilityItem(20, "Crouch", Resources.Crouch);
        public static AbilityItem Grab { get; } = new AbilityItem(21, "Grab", Resources.Grab);
        public static AbilityItem Surf { get; } = new AbilityItem(22, "Surf", Resources.Surf);
        public static AbilityItem WallJump { get; } = new AbilityItem(23, "Wall Jump", Resources.WallJump);
        public static AbilityItem Swing { get; } = new AbilityItem(24, "Swing", Resources.Swing);
        public static AbilityItem Climb { get; } = new AbilityItem(25, "Climb", Resources.Climb);

        public static AbilityItem Pogo { get; } = new AbilityItem(30, "Pogo", Resources.Pogo);
        public static AbilityItem TinyToy { get; } = new AbilityItem(31, "Tiny Toy", Resources.TinyToy);
        public static AbilityItem Jetpack { get; } = new AbilityItem(32, "Jetpack", Resources.Jetpack);

        //public static List<CharacterItem> CharacterItems { get; } = [
        //    //new CharacterItem(40, "Hazard", eVoiceCharacter.HAZARD),
        //    new CharacterItem(41, "Jen Forcer", eVoiceCharacter.JENFORCER),
        //    new CharacterItem(42, "Lex Splorer", eVoiceCharacter.LEXSPLORER),
        //    new CharacterItem(43, "Sue Nami", eVoiceCharacter.SUENAMI),
        //    new CharacterItem(44, "Lord Sludge", eVoiceCharacter.LORDSLUDGE),
        //    new CharacterItem(45, "Poizone", eVoiceCharacter.POIZONE),
        //    new CharacterItem(46, "Infantry", eVoiceCharacter.INFANTRY),
        //    new CharacterItem(47, "Megamortabeast", eVoiceCharacter.MEGAMORTABEAST),
        //    new CharacterItem(48, "Rambull", eVoiceCharacter.RAMBULL),
        //    new CharacterItem(49, "Stink Bomb", eVoiceCharacter.STINKBOMB),
        //    new CharacterItem(50, "Venomess", eVoiceCharacter.VENOMESS),
        //    new CharacterItem(51, "Tyler Rex", eVoiceCharacter.TYRONEREX),
        //    new CharacterItem(52, "Hera Scarlet", eVoiceCharacter.HERASCARLET),
        //    new CharacterItem(53, "Leo", eVoiceCharacter.LEO),
        //];

        public static List<CharacterItem> CharacterItems { get; } = [
            //new CharacterItem(40, "Hazard", eVoiceCharacter.HAZARD),
            new CharacterItem(41, "Jen Forcer", "skin_jen_forcer_default", eVoiceCharacter.JENFORCER),
            new CharacterItem(42, "Lex Splorer", "skin_lex_splorer_default", eVoiceCharacter.LEXSPLORER),
            new CharacterItem(43, "Sue Nami", "skin_sue_nami_default", eVoiceCharacter.SUENAMI),
            new CharacterItem(44, "Lord Sludge", "skin_lord_sludge_default", eVoiceCharacter.LORDSLUDGE),
            new CharacterItem(45, "Poizone", "skin_poizone_default", eVoiceCharacter.POIZONE),
            new CharacterItem(46, "Infantry", "skin_infantry_default", eVoiceCharacter.INFANTRY),
            new CharacterItem(47, "Megamortabeast", "skin_megamortabeast_default", eVoiceCharacter.MEGAMORTABEAST),
            new CharacterItem(48, "Rambull", "skin_rambull_default", eVoiceCharacter.RAMBULL),
            new CharacterItem(49, "Stink Bomb", "skin_stink_bomb_default", eVoiceCharacter.STINKBOMB),
            new CharacterItem(50, "Venomess", "skin_venomess_default", eVoiceCharacter.VENOMESS),
            new CharacterItem(51, "Tyler Rex", "skin_tyrone_rex_default", eVoiceCharacter.TYRONEREX),
            new CharacterItem(52, "Hera Scarlet", "skin_hera_scarlet_default", eVoiceCharacter.HERASCARLET),
            new CharacterItem(53, "Leo Satomi", "skin_leo_default", eVoiceCharacter.LEO),
        ];

        public static Dictionary<long, Item> AllItems
        {
            get
            {
                if (_allItems == null)
                {
                    _allItems = LoadItems();
                }

                return _allItems;
            }
        }

        /// <summary>
        /// Builds the item dictionary
        /// </summary>
        /// <returns>The item dictionary</returns>
        private static Dictionary<long, Item> LoadItems()
        {
            Dictionary<long, Item> itemDictionary = new Dictionary<long, Item>()
            {
                { 1, new XpShardItem(1) },
                { DoubleJump.Id, DoubleJump },
                { BoostJump.Id, BoostJump },
                { SlideJump.Id, SlideJump },
                { VaultJump.Id, VaultJump },
                { Crouch.Id, Crouch },
                { Grab.Id, Grab },
                { Surf.Id, Surf },
                { WallJump.Id, WallJump },
                { Swing.Id, Swing },
                { Climb.Id, Climb },
                { Pogo.Id, Pogo },
                { TinyToy.Id, TinyToy },
                { Jetpack.Id, Jetpack },
            };

            LoadWorldItems(itemDictionary);

            foreach (CharacterItem characterItem in CharacterItems)
            {
                itemDictionary[characterItem.Id] = characterItem;
            }

            return itemDictionary;
        }

        /// <summary>
        /// Builds all the items specific to each world and adds them to the dictionary
        /// </summary>
        /// <param name="itemDictionary">The item dictionary</param>
        private static void LoadWorldItems(Dictionary<long, Item> itemDictionary)
        {
            foreach (WorldInfo world in Worlds.AllWorlds)
            {
                itemDictionary.Add(world.ItemId, new WorldUnlockItem(world.ItemId, world));

                foreach (ForceFieldInfo forceField in world.ForceFields)
                {
                    itemDictionary.Add(forceField.ItemId, new ForceFieldItem(forceField.ItemId, forceField));
                }
            }
        }

        /// <summary>
        /// Gets all items of a provided type
        /// </summary>
        /// <returns>The list of items</returns>
        public static IEnumerable<T> GetItems<T>() where T : Item
        {
            return AllItems.Values
                .Where(m => m is T)
                .Select(m => (T)m);
        }

        /// <summary>
        /// Gets an item by ID
        /// </summary>
        /// <param name="id">The ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static Item? GetItem(long id)
        {
            return AllItems.GetValueOrDefault(id);
        }

        /// <summary>
        /// Gets an item by ID
        /// </summary>
        /// <param name="id">The ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static T? GetItem<T>(long id) where T : Item
        {
            return GetItems<T>()
                .FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Gets an item by the unlockable ID
        /// </summary>
        /// <param name="unlockableId">The Unlockable ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static UnlockableItem? GetUnlockableItem(string unlockableId)
        {
            return GetUnlockableItem<UnlockableItem>(unlockableId);
        }

        /// <summary>
        /// Gets an item by the unlockable ID
        /// </summary>
        /// <param name="unlockableId">The Unlockable ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static T? GetUnlockableItem<T>(string unlockableId) where T : UnlockableItem
        {
            return GetItems<T>()
                .FirstOrDefault(m => m.UnlockableId == unlockableId);
        }
    }
}
