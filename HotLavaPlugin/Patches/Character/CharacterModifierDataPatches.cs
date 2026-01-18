using HarmonyLib;
using Klei.HotLava.Settings;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    //TODO: look at CharacterModifierSelect
    [HarmonyPatch(typeof(CharacterModifierData))]
    internal class CharacterModifierDataPatches
    {
        //[HarmonyPatch("Load")]
        //[HarmonyPrefix]
        //public static bool Load_Prefix()
        //{
        //    FieldInfo instanceField = typeof(CharacterModifierData).GetField("s_Instance", BindingFlags.NonPublic | BindingFlags.Static);

        //    CharacterModifierData data = Resources.Load<CharacterModifierData>("Characters/character_modifier_data");

        //    Plugin.Logger.LogInfo("1");

        //    FieldInfo modifiersProp = typeof(CharacterModifierData).GetField("m_Modifiers", BindingFlags.NonPublic | BindingFlags.Instance);
        //    Plugin.Logger.LogInfo("2");

        //    //This doesn't work for whatever reason ??? Invalid cast???
        //    object[] modifiers = (object[])modifiersProp.GetValue(data);

        //    Plugin.Logger.LogInfo("3");

        //    FieldInfo modifierProp = modifiers[3].GetType().GetField("m_Modifier", BindingFlags.NonPublic | BindingFlags.Instance);
        //    Plugin.Logger.LogInfo("4");

        //    modifierProp.SetValue(modifiers[3], new DashModifier());
        //    Plugin.Logger.LogInfo("5");


        //    modifiersProp.SetValue(data, modifiers);
        //    Plugin.Logger.LogInfo("6");


        //    instanceField.SetValue(null, data);
        //    Plugin.Logger.LogInfo("7");


        //    return false;
        //}
    }
}
