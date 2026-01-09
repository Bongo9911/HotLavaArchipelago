using HarmonyLib;
using Klei.HotLava.UI;
using System;
using System.Reflection;

namespace HotLavaArchipelagoPlugin.Patches
{
    [HarmonyPatch(typeof(OptionsMenu))]
    internal class OptionsMenuPatches
    {
        [HarmonyPatch("BuildMenu")]
        [HarmonyPrefix]
        public static void BuildMenu_Postfix(OptionsMenu __instance)
        {
            OptionsCategory optionsCategory = CreateOptionsCategory(__instance, "Archipelago");
            //TODO: No way to do free form text D:
            CreateEntry(__instance, optionsCategory, "Port", "The Port for the Archipelago server", HLOptions.eOption.INVALID, OptionsEntry.eOptionsStyle.SLIDER);
        }

        [HarmonyPatch("CreateEntry")]
        [HarmonyPrefix]
        public static void CreateEntry_Postfix(OptionsMenu __instance,
            OptionsCategory layout_group,
            string title,
            string description,
            HLOptions.eOption option_type,
            OptionsEntry.eOptionsStyle style,
            bool sub_option = false,
            bool has_sub_options = false)
        {
            if (option_type == HLOptions.eOption.AllowDataCollection)
            {
                CreateEntry(__instance, layout_group, "Port", "The Port for the Archipelago server", HLOptions.eOption.INVALID, OptionsEntry.eOptionsStyle.SLIDER);
            }
        }

        private static OptionsCategory CreateOptionsCategory(OptionsMenu menu, string name)
        {
            Type type = menu.GetType();
            MethodInfo methodInfo = type.GetMethod("CreateCategory", BindingFlags.NonPublic | BindingFlags.Instance);

            if (methodInfo != null)
            {
                object[] parameters = { name };
                return (OptionsCategory)methodInfo.Invoke(menu, parameters);
            }
            else
            {
                //TODO:
                return null;
            }
        }

        private static OptionsEntry CreateEntry(OptionsMenu menu,
            OptionsCategory layout_group,
            string title,
            string description,
            HLOptions.eOption option_type,
            OptionsEntry.eOptionsStyle style,
            bool sub_option = false,
            bool has_sub_options = false)
        {
            Type type = menu.GetType();
            MethodInfo methodInfo = type.GetMethod("CreateEntry", BindingFlags.NonPublic | BindingFlags.Instance);

            if (methodInfo != null)
            {
                object[] parameters = { layout_group, title, description, option_type, style, sub_option, has_sub_options };
                return (OptionsEntry)methodInfo.Invoke(menu, parameters);
            }
            else
            {
                //TODO:
                return null;
            }
        }
    }
}
