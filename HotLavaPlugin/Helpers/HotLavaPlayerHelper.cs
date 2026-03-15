using Klei.HotLava.Character;
using Klei.HotLava.Online;

namespace HotLavaArchipelagoPlugin.Helpers
{
    internal static class HotLavaPlayerHelper
    {
        public static PlayerController? GetLocalPlayer()
        {
            return State.LocalPlayer;
        }

        public static void KillLocalPlayer()
        {
            PlayerController? player = GetLocalPlayer();
            if (player != null)
            {
                player.BroadcastKilled(Klei.HotLava.Enums.eDeathReason.Ragdoll);
            }
        }
    }
}
