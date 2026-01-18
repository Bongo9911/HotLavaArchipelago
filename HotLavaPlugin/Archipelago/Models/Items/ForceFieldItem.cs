using HotLavaArchipelagoPlugin.Helpers;
using Klei.HotLava.Gameplay;
using System.Linq;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class ForceFieldItem : Item
    {
        public string InternalWorldName { get; }
        public string ObjectName { get; }

        public ForceFieldItem(long id, string name, string internalWorldName, string objectName) : base(id, name)
        {
            InternalWorldName = internalWorldName;
            ObjectName = objectName;
        }

        public override void GrantItem()
        {
            string? currentWorld = LevelHelper.GetCurrentWorldName();

            //Don't try to deactivate force field if player is not currently in the world
            if (currentWorld != InternalWorldName) return;

            GameModeCompletedGate[] forceFields = Object.FindObjectsByType<GameModeCompletedGate>(FindObjectsSortMode.InstanceID);

            GameModeCompletedGate? forceField = forceFields.FirstOrDefault(f => f.name == ObjectName);

            if (forceField != null)
            {
                Plugin.Logger.LogInfo("Deactivating force field: " + ObjectName);
                //TODO: only play this if not already played
                forceField.PlayDeactivationAnimation();
            }
            else
            {
                Plugin.Logger.LogInfo("Could not find force field: " + ObjectName);
                Plugin.Logger.LogInfo("Found force fields: ");

                foreach (GameModeCompletedGate gameModeCompletedGate in forceFields)
                {
                    Plugin.Logger.LogInfo(gameModeCompletedGate.name);
                }
            }
        }
    }
}
