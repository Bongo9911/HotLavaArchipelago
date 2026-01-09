using Klei.HotLava.Unlockables;
using System.Linq;

namespace HotLavaPlugin.Helpers
{
    internal static class UnlockableHelper
    {
        /// <summary>
        /// Returns the unlockable object with the matching ID
        /// </summary>
        /// <param name="id">The ID of the unlockable</param>
        /// <returns>The unlockable, if found, else null</returns>
        internal static Unlockable? GetUnlockableById(string id)
        {
            return Statistics.AllUnlockables.FirstOrDefault(u => u.m_Key.m_Value.Equals(id));
        }
    }
}
