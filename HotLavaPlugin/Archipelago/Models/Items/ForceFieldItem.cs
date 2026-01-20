using HotLavaArchipelagoPlugin.Factories;
using HotLavaArchipelagoPlugin.Helpers;
using HotLavaArchipelagoPlugin.Models.Game;
using Klei.HotLava.Gameplay;
using Klei.HotLava.Rewards;
using System.Linq;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class ForceFieldItem : Item
    {
        public string InternalWorldName { get; }
        public Vector3 Position { get; }

        public ForceFieldItem(long id, ForceFieldInfo forceField)
            : base(id, forceField.World.Name + " - Deactivate Force Field - " + forceField.Name)
        {
            InternalWorldName = forceField.World.InternalName;
            Position = forceField.Position;
        }

        /// <inheritdoc/>
        public override void GrantItem()
        {
            string? currentWorld = LevelHelper.GetCurrentWorldName();

            //Don't try to deactivate force field if player is not currently in the world
            if (currentWorld != InternalWorldName) return;

            GameModeCompletedGate[] forceFields = Object.FindObjectsByType<GameModeCompletedGate>(FindObjectsSortMode.InstanceID);

            GameModeCompletedGate? forceField = forceFields.FirstOrDefault(f => f.transform.position == Position);

            if (forceField != null)
            {
                //TODO: only play this if not already played
                forceField.PlayDeactivationAnimation();
            }
            else
            {
                Plugin.Logger.LogInfo("Could not find force field: " + Position.x + ", " + Position.y + ", " + Position.z);
                Plugin.Logger.LogInfo("Found force fields: ");

                foreach (GameModeCompletedGate gameModeCompletedGate in forceFields)
                {
                    Plugin.Logger.LogInfo(gameModeCompletedGate.name);
                }
            }
        }

        /// <inheritdoc/>
        public override RewardVisualization GetRewardVisualization(GiftDropVisualization giftDropVisualization)
        {
            return RewardVisualizationFactory.FromImage(giftDropVisualization, Properties.Resources.ForceField);
        }
    }
}
