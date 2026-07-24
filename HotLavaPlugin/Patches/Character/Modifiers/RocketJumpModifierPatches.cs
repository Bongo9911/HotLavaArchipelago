using HarmonyLib;
using Klei.HotLava.Character;
using Klei.HotLava.Character.Modifiers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches.Character.Modifiers
{
    /// <summary>
    /// RocketJumpModifier has no code of its own to show a held weapon model - the original
    /// "water_bomber" gun appears to have been baked directly into the first-person arm rig
    /// itself rather than spawned as a separate object, and that rig content was stripped along
    /// with the rest of the abandoned rocket jump feature (see PlayerRigAnimatorPatches). There's
    /// no rig asset left to restore that properly, so this parents a held prop to the player's
    /// hand while the ability is equipped instead of leaving the hand empty: the real gun model
    /// (recovered from an older game build and rebuilt into an AssetBundle - see
    /// "waterbomberassets" shipped alongside this plugin) if it's present, falling back to a
    /// clone of the projectile's own water_bomber_bottle mesh otherwise.
    ///
    /// A full-controller swap to the AR-mode animator (which still has real clips wired to the
    /// Rocket_Jump/Rocket_Fire/Rocket_Reload states) was also tried here, to restore third-person
    /// animation. It broke movement entirely (the player got stuck unable to move in first
    /// person) - the AR rig apparently isn't a drop-in replacement for the normal gameplay
    /// animator - so it was reverted. The default animations (a static pose while the ability is
    /// equipped) are used instead.
    /// </summary>
    [HarmonyPatch(typeof(RocketJumpModifier))]
    internal class RocketJumpModifierPatches
    {
        private const string AssetBundleFileName = "waterbomberassets";
        private const string GunPrefabAssetName = "WaterGunProp";

        private static GameObject s_HeldPropTemplate;
        private static readonly Dictionary<RocketJumpModifier, GameObject> s_HeldProps = new Dictionary<RocketJumpModifier, GameObject>();

        [HarmonyPatch(nameof(RocketJumpModifier.AddModifier))]
        [HarmonyPostfix]
        public static void AddModifier_Postfix(RocketJumpModifier __instance, PlayerController player)
        {
            GameObject template = GetHeldPropTemplate();
            Transform hand = player.Rig != null ? player.Rig.m_RightHandPalm : null;

            Plugin.Logger.LogInfo($"[Archipelago] AddModifier_Postfix: template={(template != null ? template.name : "null")} hand={(hand != null ? hand.name : "null")}");

            if (template == null || hand == null)
            {
                return;
            }

            GameObject prop = Object.Instantiate(template, hand, false);

            // The source mesh came from the RocketProjectile prefab (layer 27, tuned for a
            // flying projectile) rather than the character rig, and this game explicitly
            // per-perspective layer-masks what's visible (see PlayerRig.SetTypemaskThirdPerson)
            // - a held item needs the hand bone's own layer to render for the local player at
            // all, not whatever layer its source prefab happened to be on.
            SetLayerRecursively(prop, hand.gameObject.layer);

            prop.SetActive(true);
            s_HeldProps[__instance] = prop;
        }

        [HarmonyPatch(nameof(RocketJumpModifier.RemoveModifier))]
        [HarmonyPostfix]
        public static void RemoveModifier_Postfix(RocketJumpModifier __instance)
        {
            if (s_HeldProps.TryGetValue(__instance, out GameObject prop))
            {
                Object.Destroy(prop);
                s_HeldProps.Remove(__instance);
            }
        }

        private static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        private static GameObject GetHeldPropTemplate()
        {
            if (s_HeldPropTemplate != null)
            {
                return s_HeldPropTemplate;
            }

            GameObject fromBundle = LoadGunPropFromAssetBundle();
            GameObject template = fromBundle ?? LoadFallbackBottleProp();
            Plugin.Logger.LogInfo($"[Archipelago] GetHeldPropTemplate: fromBundle={(fromBundle != null)} finalTemplate={(template != null ? template.name : "null")}");

            if (template == null)
            {
                return null;
            }

            template.name = "WaterBomberHeldProp";
            Object.DontDestroyOnLoad(template);

            // Strip any MonoBehaviours the source carried (e.g. the bottle fallback's flight
            // sound trigger, meant for the flying projectile, not a static held prop) - harmless
            // no-op for the gun model, which has none.
            foreach (MonoBehaviour behaviour in template.GetComponentsInChildren<MonoBehaviour>(true))
            {
                Object.Destroy(behaviour);
            }

            template.SetActive(false);
            s_HeldPropTemplate = template;
            return template;
        }

        /// <summary>
        /// Loads the real water_bomber gun model, rebuilt from an older game build into a plain
        /// Unity AssetBundle (this plugin's assembly and this bundle are the only two things a
        /// Unity project needed to produce - no Hot Lava code was involved). The bundle is
        /// expected to sit alongside this plugin's own DLL.
        /// </summary>
        private static GameObject LoadGunPropFromAssetBundle()
        {
            try
            {
                string pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string bundlePath = Path.Combine(pluginDirectory ?? string.Empty, AssetBundleFileName);

                bool exists = File.Exists(bundlePath);
                Plugin.Logger.LogInfo($"[Archipelago] Looking for AssetBundle at '{bundlePath}'. Exists={exists}");

                if (!exists)
                {
                    return null;
                }

                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
                if (bundle == null)
                {
                    Plugin.Logger.LogWarning($"[Archipelago] Found '{AssetBundleFileName}' but failed to load it as an AssetBundle.");
                    return null;
                }

                GameObject prefab = bundle.LoadAsset<GameObject>(GunPrefabAssetName);
                if (prefab == null)
                {
                    Plugin.Logger.LogWarning($"[Archipelago] AssetBundle '{AssetBundleFileName}' did not contain a '{GunPrefabAssetName}' prefab.");
                    bundle.Unload(false);
                    return null;
                }

                GameObject instance = Object.Instantiate(prefab);

                // Only the loaded object registry is torn down here (false); the instance just
                // created above keeps its own copies of the mesh/material data and stays valid.
                bundle.Unload(false);
                return instance;
            }
            catch (System.Exception exception)
            {
                Plugin.Logger.LogError($"[Archipelago] Exception while loading gun prop AssetBundle: {exception}");
                return null;
            }
        }

        private static GameObject LoadFallbackBottleProp()
        {
            GameObject rocketProjectilePrefab = Resources.Load<GameObject>("gameplay/RocketProjectile");
            if (rocketProjectilePrefab == null || rocketProjectilePrefab.transform.childCount == 0)
            {
                return null;
            }

            // Child 0 ("scale_offset") carries the correct baked-in position/rotation/scale for
            // the water_bomber_bottle mesh underneath it, hence cloning that rather than just the
            // mesh's own GameObject.
            return Object.Instantiate(rocketProjectilePrefab.transform.GetChild(0).gameObject);
        }
    }
}
