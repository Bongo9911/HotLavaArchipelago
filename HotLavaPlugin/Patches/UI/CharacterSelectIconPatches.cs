using HarmonyLib;
using HotLavaArchipelagoPlugin.Archipelago;
using HotLavaArchipelagoPlugin.Archipelago.Data;
using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using Klei.HotLava.UI;
using System.Linq;
using UnityEngine.UI;

namespace HotLavaArchipelagoPlugin.Patches.UI
{
    [HarmonyPatch(typeof(CharacterSelectIcon))]
    internal class CharacterSelectIconPatches
    {
        [HarmonyPatch(nameof(CharacterSelectIcon.Set))]
        [HarmonyPostfix]
        public static void Set_Postfix(CharacterSelectIcon __instance)
        {
            if (Multiworld.Connected)
            {
                CharacterItem? characterItem = Items.CharacterItems.FirstOrDefault(c => c.CharacterId == __instance.m_Character.CharacterEnum);

                if (characterItem != null)
                {
                    bool unlocked = Multiworld.HasReceivedItem(characterItem);

                    if (!unlocked)
                    {
                        __instance.m_LockedText.text = "LOCKED";
                        __instance.m_LockedText.fontSize = 12;
                    }

                    __instance.m_Locked.gameObject.SetActive(!unlocked);
                    __instance.GetComponent<Button>().interactable = unlocked;
                }
            }
        }
    }
}
