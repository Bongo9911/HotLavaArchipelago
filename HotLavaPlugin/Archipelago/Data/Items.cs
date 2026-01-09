using HotLavaPlugin.Archipelago.Models.Items;
using System.Collections.Generic;

namespace HotLavaPlugin.Archipelago.Data
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

        private static Dictionary<long, Item> LoadItems()
        {
            return new Dictionary<long, Item>()
            {
                { 1, new XpShardItem(1) }
            };
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
    }
}
