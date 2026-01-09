using HotLavaArchipelagoPlugin.Archipelago.Models.Locations;
using HotLavaArchipelagoPlugin.Enums;
using HotLavaArchipelagoPlugin.Game;
using System.Collections.Generic;
using System.Linq;

namespace HotLavaArchipelagoPlugin.Archipelago.Data
{
    internal static class Locations
    {
        private static Dictionary<long, Location>? _allLocations = null;

        public static Dictionary<long, Location> AllLocations
        {
            get
            {
                if (_allLocations == null)
                {
                    _allLocations = SetUpLocations();
                }

                return _allLocations;
            }
        }

        private static Dictionary<long, Location> SetUpLocations()
        {
            Dictionary<long, Location> locations = new Dictionary<long, Location>();

            long worldIdOffset = 100;
            foreach (World world in Worlds.AllWorlds)
            {
                long courseIdOffset = 0;
                foreach (Course course in world.Courses)
                {
                    for (long i = 0; i < course.Stars.Count(); ++i)
                    {
                        Star star = course.Stars[i];
                        long locationId = worldIdOffset + courseIdOffset + i;
                        locations.Add(locationId, new UnlockableLocation(locationId, star.UnlockableId, star.ToString()));
                    }

                    if (course.CourseType == CourseType.Standard)
                    {
                        courseIdOffset += 10;
                    }
                    else
                    {
                        courseIdOffset += 1;
                    }
                }

                worldIdOffset += 100;
            }

            return locations;
        }

        /// <summary>
        /// Gets a location by ID
        /// </summary>
        /// <param name="id">The id of the location</param>
        /// <returns>The location</returns>
        public static Location? GetLocation(long id)
        {
            return AllLocations.GetValueOrDefault(id);
        }

        /// <summary>
        /// Gets an unlockable location by it's unlockable ID
        /// </summary>
        /// <param name="unlockableId">The id of the unlockable</param>
        /// <returns>The location</returns>
        public static UnlockableLocation? GetUnlockableLocation(string unlockableId)
        {
            return AllLocations.Values.Where(l => l is UnlockableLocation)
                .Select(l => (UnlockableLocation)l)
                .Where(l => l.UnlockableId == unlockableId)
                .FirstOrDefault();
        }
    }
}
