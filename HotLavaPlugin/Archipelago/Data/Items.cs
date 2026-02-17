using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Models.Game;
using System.Collections.Generic;
using System.Linq;

namespace HotLavaArchipelagoPlugin.Archipelago.Data
{
    internal static class Items
    {
        private static Dictionary<long, Item>? _allItems = null;

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
            };

            LoadWorldItems(itemDictionary);

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
