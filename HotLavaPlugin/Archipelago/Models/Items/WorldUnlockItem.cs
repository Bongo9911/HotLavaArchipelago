using HotLavaArchipelagoPlugin.Models.Game;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class WorldUnlockItem : UnlockableItem
    {
        /// <summary>
        /// The internal name for the world
        /// </summary>
        public string InternalName { get; set; }

        public WorldUnlockItem(long id, WorldInfo world)
            : base(id, world.UnlockableId, "World Unlock - " + world.Name)
        {
            InternalName = world.InternalName;
        }
    }
}
