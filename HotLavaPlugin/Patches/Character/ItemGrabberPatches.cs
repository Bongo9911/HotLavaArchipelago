using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using Klei.HotLava.Character;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    [HarmonyPatch(typeof(ItemGrabber))]
    internal class ItemGrabberPatches
    {
        /// <summary>
        /// Prevent grabbing items without grab unlocked
        /// </summary>
        /// <returns></returns>
        [HarmonyPatch("StartGrab")]
        [HarmonyPrefix]
        public static bool StartGrab_Prefix()
        {
            if (Multiworld.Connected && !Multiworld.HasReceivedItem(Items.Grab))
            {
                return false;
            }
            return true;
        }
    }
}
