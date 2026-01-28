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

            ArchipelagoModifier archipelagoModifier = ScriptableObject.CreateInstance<ArchipelagoModifier>();

            for (int i = 0; i < modifiers.Length; ++i)
            {
                PlayerControllerModifier modifier = (PlayerControllerModifier)modifierProp.GetValue(modifiers.GetValue(i));

                if (modifier is LungeModifier lungeModifier)
                {
                    archipelagoModifier.m_LungeVelociyCurve = lungeModifier.m_LungeVelociyCurve;
                    archipelagoModifier.m_ClamberVelocityCurve = lungeModifier.m_ClamberVelocityCurve;
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
