using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava.Unlockables;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Locations
{
    /// <summary>
    /// A location associated to an unlockable
    /// </summary>
    internal class UnlockableLocation : Location
    {
        /// <summary>
        /// The identifier for the unlockable
        /// </summary>
        public string UnlockableId { get; }
        /// <summary>
        /// A descritpion of the unlockable
        /// </summary>
        public string UnlockableDescription { get; }

        public UnlockableLocation(long locationId, string unlockableId, string unlockableDescription) : base(locationId)
        {
            UnlockableId = unlockableId;
            UnlockableDescription = unlockableDescription;
        }

        /// <summary>
        /// Gets the unlockable associated with this location
        /// </summary>
        /// <returns>The unlockable object</returns>
        public Unlockable? GetUnlockable()
        {
            return UnlockableHelper.GetUnlockableById(UnlockableId);
        }

        /// <inheritdoc/>
        public override void CheckLocation()
        {
            Statistics.UnlockUnlockable(GetUnlockable(), false);
        }
    }
}
