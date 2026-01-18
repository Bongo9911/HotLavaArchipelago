using Klei.HotLava.Character;
using Klei.HotLava.Online;

namespace HotLavaArchipelagoPlugin.Helpers
{
    internal static class PlayerHelper
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
                player.KillWithRagdoll();
            }
        }
    }
}
