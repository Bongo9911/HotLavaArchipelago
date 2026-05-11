using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava.Character;
using Klei.HotLava.Rewards;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items.Traps
{
    internal class SlowTrapItem : Item
    {
        public SlowTrapItem(long id) : base(id, "Slow Trap") { }

        public override RewardVisualization? GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            return null;
        }

        public override void GrantItem()
        {
            PlayerController? player = HotLavaPlayerHelper.GetLocalPlayer();

            if (player != null)
            {
                //Vector3 vector3 = player.RigidBody.velocity;
                //vector3.y = 0.0f;
                //vector3 = vector3.normalized * 0;
                //vector3.y = player.RigidBody.velocity.y;
                player.RigidBody.velocity = player.RigidBody.velocity.normalized * 0;
            }
        }
    }
}
