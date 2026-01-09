using Klei.HotLava.Gameplay;
using System.Linq;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Archipelago.Models.Items
{
    internal class ForceFieldItem : Item
    {
        public string ObjectName { get; }

        public ForceFieldItem(long id, string name, string objectName) : base(id, name)
        {
            ObjectName = objectName;
        }

        public override void GrantItem()
        {
            //TODO: verify current force field is for the current world

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
