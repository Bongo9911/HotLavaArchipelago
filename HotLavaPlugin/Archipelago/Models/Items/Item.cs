namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    /// <summary>
    /// An item that can be granted by Archipelago
    /// </summary>
    internal abstract class Item
    {
        /// <summary>
        /// The unique ID for the item
        /// </summary>
        public long Id { get; }
        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name { get; }

        public Item(long id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Grants the item to the player
        /// </summary>
        public abstract void GrantItem();
    }
}
