using Klei.HotLava.Unlockables;
using System.Linq;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class UnlockableItem : Item
    {
        /// <summary>
        /// The identifier for the unlockable
        /// </summary>
        public string UnlockableId { get; }

        public UnlockableItem(long id, string unlockabledId, string name) : base(id, name)
        {
            UnlockableId = unlockabledId;
        }

        public override void GrantItem()
        {
            Unlockable unlockable = Statistics.AllUnlockables.FirstOrDefault(u => u.m_Key.m_Value == UnlockableId);

            Statistics.UnlockUnlockable(unlockable, true);
        }
    }
}
