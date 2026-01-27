using HarmonyLib;
using HotLavaArchipelagoPlugin.Factories;
using HotLavaArchipelagoPlugin.Gameplay.Modifiers;
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

            object modifier = modifiers.GetValue(modifiers.Length - 1);
            FieldInfo modifierProp = modifier.GetType().GetField("m_Modifier", BindingFlags.NonPublic | BindingFlags.Instance);
            modifierProp.SetValue(modifier, ScriptableObject.CreateInstance<ArchipelagoModifier>());

            FieldInfo iconProp = modifier.GetType().GetField("m_Icon", BindingFlags.NonPublic | BindingFlags.Instance);
            iconProp.SetValue(modifier, SpriteFactory.GetArchipelagoSprite());

            Array newModifiers = Array.CreateInstance(modifier.GetType(), modifiers.Length + 1);

            for (int i = 0; i < modifiers.Length; i++)
            {
                newModifiers.SetValue(modifiers.GetValue(i), i);
            }

            newModifiers.SetValue(modifier, modifiers.Length);

            modifiersProp.SetValue(data, newModifiers);

            instanceField.SetValue(null, data);

            return false;
        }
    }
}
