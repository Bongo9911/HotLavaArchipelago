using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.Game;
using System.Collections.Generic;

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

        private static Dictionary<long, Item> LoadItems()
        {
            Dictionary<long, Item> itemDictionary = new Dictionary<long, Item>()
            {
                { 1, new XpShardItem(1) },
            };

            long worldOffset = 100;
            foreach (World world in Worlds.AllWorlds)
            {
                itemDictionary.Add(worldOffset, new WorldUnlockItem(worldOffset, world.UnlockableId, world.InternalName, world.Name));

                long idOffset = 1;

                foreach (ForceField forceField in world.ForceFields)
                {
                    long id = worldOffset + idOffset;
                    itemDictionary.Add(id, new ForceFieldItem(id, forceField.World.Name + " - Deactivate Force Field - " + forceField.Name, forceField.World.InternalName, forceField.ObjectName));
                    idOffset++;
                }

                worldOffset += 100;
            }

            return itemDictionary;
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
