using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using Klei.HotLava;
using System.Threading.Tasks;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(LevelSingleton))]
    internal class LevelSingletonPatches
    {
        [HarmonyPatch("SendChatMessage")]
        [HarmonyPrefix]
        public static bool SendChatMessage_Prefix(LevelSingleton __instance, string message, object target)
        {
            if (message.StartsWith("/apconnect"))
            {
                Task.Run(() => Multiworld.Connect(message)).GetAwaiter().GetResult();
                return false;
            }

            if (Multiworld.Connected)
            {
                Multiworld.Instance.ArchipelagoSession.Say(message);
            }

            return true;
        }
    }
}
