using HarmonyLib;
using HotLavaArchipelagoPlugin.Factories;
using HotLavaArchipelagoPlugin.Gameplay.Modifiers;
using Klei.HotLava.Character.Modifiers;
using Klei.HotLava.Settings;
using System;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    //TODO: look at CharacterModifierSelect
    [HarmonyPatch(typeof(CharacterModifierData))]
    internal class CharacterModifierDataPatches
    {
        [HarmonyPatch("Load")]
        [HarmonyPrefix]
        public static bool Load_Prefix()
        {
            FieldInfo instanceField = typeof(CharacterModifierData).GetField("s_Instance", BindingFlags.NonPublic | BindingFlags.Static);

            CharacterModifierData data = Resources.Load<CharacterModifierData>("Characters/character_modifier_data");

            FieldInfo modifiersProp = typeof(CharacterModifierData).GetField("m_Modifiers", BindingFlags.NonPublic | BindingFlags.Instance);
            Array modifiers = (Array)modifiersProp.GetValue(data);

            Type entryType = modifiers.GetValue(0).GetType();
            FieldInfo modifierProp = entryType.GetField("m_Modifier", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo iconProp = entryType.GetField("m_Icon", BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < modifiers.Length; ++i)
            {
                PlayerControllerModifier modifier = (PlayerControllerModifier)modifierProp.GetValue(modifiers.GetValue(i));

                if (modifier is LungeModifier lungeModifier)
                {
                    AbilityRandomizerModifier.LungeModifier = lungeModifier;
                }
                else if (modifier is DoubleJumpModifier doubleJumpModifier)
                {
                    AbilityRandomizerModifier.DoubleJumpModifier = doubleJumpModifier;
                }
                else if (modifier is SlideJumpModifier slideJumpModifier)
                {
                    AbilityRandomizerModifier.SlideJumpModifier = slideJumpModifier;
                }
            }

            AbilityRandomizerModifier archipelagoModifier = ScriptableObject.CreateInstance<AbilityRandomizerModifier>();
            modifiers = AppendModifierEntry(modifiers, entryType, modifierProp, iconProp, archipelagoModifier, SpriteFactory.GetArchipelagoSprite());

            // RocketJumpModifier is fully implemented but isn't referenced by any other asset the
            // game still loads (its animator hookup was independently gutted - see
            // PlayerRigAnimatorPatches), so there's no existing instance to grab like the
            // Lunge/DoubleJump/SlideJump modifiers above; it has to be constructed fresh. The
            // base m_UIOverlayResource field is still left unset (a serialized prefab reference
            // with no live pointer to grab), so the rocket-jump ammo HUD won't appear, but
            // firing/reloading/the jump boost itself work off the modifier's own logic.
            RocketJumpModifier rocketJumpModifier = ScriptableObject.CreateInstance<RocketJumpModifier>();

            // Recovered from the Hot Lava Mod Kit's exported "Player_RocketJumpModified.physicMaterial"
            // (the mod kit still ships this asset even though the main game no longer references
            // it), so the surface still gets the original low-friction, no-bounce behaviour.
            rocketJumpModifier.m_RocketJumpPhysicsMaterial = new PhysicMaterial("Player_RocketJumpModified")
            {
                dynamicFriction = 0.04f,
                staticFriction = 0.04f,
                bounciness = 0f,
                frictionCombine = PhysicMaterialCombine.Average,
                bounceCombine = PhysicMaterialCombine.Minimum
            };

            modifiers = AppendModifierEntry(modifiers, entryType, modifierProp, iconProp, rocketJumpModifier, SpriteFactory.GetArchipelagoSprite());

            modifiersProp.SetValue(data, modifiers);

            instanceField.SetValue(null, data);

            return false;
        }

        private static Array AppendModifierEntry(Array modifiers, Type entryType, FieldInfo modifierProp, FieldInfo iconProp, PlayerControllerModifier modifier, Sprite icon)
        {
            // tData is a value-type struct, so this is a boxed copy of the last entry - used only
            // as a template for whatever other fields it carries (e.g. m_WeeklyCheckpointTime);
            // the original entries in `modifiers` are left untouched.
            object newEntry = modifiers.GetValue(modifiers.Length - 1);
            modifierProp.SetValue(newEntry, modifier);
            iconProp.SetValue(newEntry, icon);

            Array newModifiers = Array.CreateInstance(entryType, modifiers.Length + 1);
            Array.Copy(modifiers, newModifiers, modifiers.Length);
            newModifiers.SetValue(newEntry, modifiers.Length);

            return newModifiers;
        }
    }
}
