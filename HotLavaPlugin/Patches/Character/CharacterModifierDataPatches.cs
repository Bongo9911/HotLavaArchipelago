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

            object modifierCopy = modifiers.GetValue(modifiers.Length - 1);
            FieldInfo modifierProp = modifierCopy.GetType().GetField("m_Modifier", BindingFlags.NonPublic | BindingFlags.Instance);

            AbilityRandomizerModifier archipelagoModifier = ScriptableObject.CreateInstance<AbilityRandomizerModifier>();

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

            modifierProp.SetValue(modifierCopy, archipelagoModifier);

            FieldInfo iconProp = modifierCopy.GetType().GetField("m_Icon", BindingFlags.NonPublic | BindingFlags.Instance);
            iconProp.SetValue(modifierCopy, SpriteFactory.GetArchipelagoSprite());

            Array newModifiers = Array.CreateInstance(modifierCopy.GetType(), modifiers.Length + 1);

            for (int i = 0; i < modifiers.Length; i++)
            {
                newModifiers.SetValue(modifiers.GetValue(i), i);
            }

            newModifiers.SetValue(modifierCopy, modifiers.Length);

            modifiersProp.SetValue(data, newModifiers);

            instanceField.SetValue(null, data);

            return false;
        }
    }
}
