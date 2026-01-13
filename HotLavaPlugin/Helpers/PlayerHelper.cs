using Klei.HotLava.Character;
using Klei.HotLava.Online;

namespace HotLavaArchipelagoPlugin.Helpers
{
    internal class PlayerHelper
    {
        public PlayerController? GetLocalPlayer()
        {
            return State.LocalPlayer;
        }

        public void KillLocalPlayer()
        {
            PlayerController? player = GetLocalPlayer();
            if (player != null)
            {
                player.KillWithRagdoll();
            }
        }
    }
}
