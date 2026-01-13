namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class WorldUnlockItem : UnlockableItem
    {
        /// <summary>
        /// The internal name for the world
        /// </summary>
        public string InternalName { get; set; }

        public WorldUnlockItem(long id, string unlockabledId, string internalName, string worldName)
            : base(id, unlockabledId, "World Unlock - " + worldName)
        {
            InternalName = internalName;
        }
    }
}
