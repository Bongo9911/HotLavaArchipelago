using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava.Gameplay;
using System.Linq;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class ForceFieldItem : Item
    {
        public string InternalWorldName { get; }
        public Vector3 Position { get; }

        public ForceFieldItem(long id, string name, string internalWorldName, Vector3 position) : base(id, name)
        {
            InternalWorldName = internalWorldName;
            Position = position;
        }

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
    }
}
