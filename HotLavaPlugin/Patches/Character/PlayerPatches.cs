using HarmonyLib;
using Klei.HotLava.Character.Modifiers;
using Klei.HotLava.Online;

namespace HotLavaArchipelagoPlugin.Patches.Character
{
    [HarmonyPatch(typeof(Player))]
    internal class PlayerPatches
    {
        private static PlayerControllerModifier _modifier = new LavaBounceModifier();

        //[HarmonyPatch("Modifier", MethodType.Getter)]
        //[HarmonyPrefix]
        //public static bool Modifier_Prefix(Player __instance, ref PlayerControllerModifier __result)
        //{
        //    __result = _modifier;
        //    return false;
        //}

        //[HarmonyPatch("Modifier", MethodType.Setter)]
        //[HarmonyPrefix]
        //public static bool Modifier_Prefix(Player __instance, ref PlayerControllerModifier value)
        //{
        //    value = _modifier;
        //    return false;
        //}

        //[HarmonyPatch(MethodType.Constructor)]
        //[HarmonyPostfix]
        //public static void Construct_Postfix(Player __instance)
        //{
        //    PropertyInfo prop = __instance.GetType().GetProperty("Modifier");
        //    // Set the value without regard for the 'internal' access modifier
        //    prop.SetValue(__instance, _modifier);
        //}
    }
}
