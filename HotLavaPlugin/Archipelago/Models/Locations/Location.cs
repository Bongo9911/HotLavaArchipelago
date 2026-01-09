using System.Threading.Tasks;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Locations
{
    internal abstract class Location
    {
        /// <summary>
        /// The ID for the location in archipelago
        /// </summary>
        public long LocationID { get; }

        public Location(long locationID)
        {
            LocationID = locationID;
        }

        /// <summary>
        /// Performs logic needed for if the location is checked manually
        /// </summary>
        public abstract void CheckLocation();

        public async Task CompleteLocationChecks()
        {
            if (Plugin.ArchipelagoSession != null)
            {
                Plugin.ArchipelagoSession.Locations.CompleteLocationChecks(LocationID);
            }
        }
    }
}
