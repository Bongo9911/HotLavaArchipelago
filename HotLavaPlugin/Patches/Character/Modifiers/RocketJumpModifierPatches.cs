using HarmonyLib;
using Klei.HotLava.Character;
using Klei.HotLava.Character.Modifiers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

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
    /// This also drives real first-person animation (idle/move/fire/reload/jump), recovered as
    /// genuine AnimationClips from an older game build, via the Playables API - see
    /// <see cref="AnimationOverlay"/> for why (the states these clips would normally play from
    /// were deleted outright from the live character animator, so there's nothing to
    /// AnimatorOverrideController onto). A full-controller swap to the AR-mode animator was tried
    /// first instead, to restore this the "easy" way; it broke movement entirely (the player got
    /// stuck unable to move in first person) - the AR rig isn't a drop-in replacement for the
    /// normal gameplay animator - so it was reverted in favour of this approach.
    /// </summary>
    [HarmonyPatch(typeof(RocketJumpModifier))]
    internal class RocketJumpModifierPatches
    {
        private const string AssetBundleFileName = "waterbomberassets";
        private const string GunPrefabAssetName = "WaterGunProp";
        private const string FireClipAssetName = "1P_Rocket_fire_shoot";
        private const string ReloadClipAssetName = "1P_Rocket_reload_loop";
        private const string IdleClipAssetName = "1P_Rocket_idle_loop";
        private const string MoveClipAssetName = "1P_Rocket_move_forward";
        private const string JumpPreClipAssetName = "1P_Rocket_jump_pre";
        private const string JumpLoopClipAssetName = "1P_Rocket_jump_loop";
        private const string JumpPostClipAssetName = "1P_Rocket_jump_pst";
        private const string CrouchClipAssetName = "1P_Rocket_move_crouch_still";

        // Below this flat speed (units/sec), the player counts as "stationary" for the idle vs.
        // move overlay.
        private const float IdleSpeedThreshold = 0.05f;

        // The hand correctly grips the gun's trigger, but the model itself renders rolled 90
        // degrees (held sideways instead of upright) - the prefab's baked orientation doesn't
        // match this hand bone's own local axes. Corrected here rather than by re-baking the
        // prefab, so it's a one-line adjustment if the sign/axis needs flipping after testing.
        private static readonly Quaternion HeldPropRotationOffset = Quaternion.Euler(90f, 0f, 0f);

        // The grip point (local origin) lines up correctly with the hand, but the gun otherwise
        // renders too close to the camera compared to reference footage. A raw localPosition
        // offset of just 0.15 sent it flying off to a completely different part of the screen -
        // this hand bone's hierarchy apparently carries an unusual inherited scale, which
        // amplifies local-space offsets unpredictably. World-space offset along the hand's own
        // forward direction (a proper unit vector, scale-independent) is far more predictable to
        // tune instead.
        private const float HeldPropForwardOffset = 0f;

        private static GameObject s_HeldPropTemplate;
        private static readonly Dictionary<RocketJumpModifier, GameObject> s_HeldProps = new Dictionary<RocketJumpModifier, GameObject>();

        private static bool s_AnimationClipsLoaded;
        private static AnimationClip s_FireClip;
        private static AnimationClip s_ReloadClip;
        private static AnimationClip s_IdleClip;
        private static AnimationClip s_MoveClip;
        private static AnimationClip s_JumpPreClip;
        private static AnimationClip s_JumpLoopClip;
        private static AnimationClip s_JumpPostClip;
        private static AnimationClip s_CrouchClip;

        // Crouching is internal on PlayerController, unreachable directly from this assembly.
        private static readonly PropertyInfo CrouchingProperty = AccessTools.Property(typeof(PlayerController), "Crouching");

        /// <summary>
        /// The Rocket_Jump/Fire/Reload states were deleted outright from the live character
        /// animator's 293-state graph (confirmed: AnimatorOverrideController.GetOverrides()
        /// reports zero rocket-related slots, and no state in the graph is even named "Rocket"
        /// anything) - only the unused boolean parameters remain. There's no state left to
        /// re-target a clip onto via the override system, and no runtime API to add new states to
        /// an existing RuntimeAnimatorController. Instead, a small per-player PlayableGraph wraps
        /// the animator's own existing controller (so normal locomotion keeps working exactly as
        /// before) and mixes in the recovered clips as full-body overlays: momentary one-shots
        /// for fire/reload/jump-pre/jump-post, and a continuous pose for idle/move/jump-loop that
        /// gets re-evaluated every frame based on the player's grounded state and speed.
        /// </summary>
        private class AnimationOverlay
        {
            public PlayerController Player;
            public PlayableGraph Graph;
            public AnimationMixerPlayable Mixer;

            // A momentary one-shot (fire/reload/jump-pre/jump-post) always takes priority over
            // the continuous pose below, for as long as it's playing.
            public int ActiveOneShotInput = -1;
            public float OneShotEndTime;

            // The sustained pose (idle/move/jump-loop) shown whenever no one-shot is active.
            public int ContinuousInput = -1;
            public bool WasGrounded = true;
        }

        private const int BaseControllerInput = 0;
        private const int FireOverlayInput = 1;
        private const int ReloadOverlayInput = 2;
        private const int IdleOverlayInput = 3;
        private const int MoveOverlayInput = 4;
        private const int JumpPreOverlayInput = 5;
        private const int JumpLoopOverlayInput = 6;
        private const int JumpPostOverlayInput = 7;
        private const int CrouchOverlayInput = 8;
        private const int MixerInputCount = 9;

        private static readonly Dictionary<RocketJumpModifier, AnimationOverlay> s_AnimationOverlays = new Dictionary<RocketJumpModifier, AnimationOverlay>();

        [HarmonyPatch(nameof(RocketJumpModifier.AddModifier))]
        [HarmonyPostfix]
        public static void AddModifier_Postfix(RocketJumpModifier __instance, PlayerController player)
        {
            GameObject template = GetHeldPropTemplate();
            Transform hand = player.Rig != null ? player.Rig.m_RightHandPalm : null;

            Plugin.Logger.LogInfo($"[Archipelago] AddModifier_Postfix: template={(template != null ? template.name : "null")} hand={(hand != null ? hand.name : "null")}");

            if (template != null && hand != null)
            {
                GameObject prop = Object.Instantiate(template, hand, false);
                prop.transform.localRotation = HeldPropRotationOffset;
                prop.transform.position = hand.position + (hand.forward * HeldPropForwardOffset);

                // The source mesh came from the RocketProjectile prefab (layer 27, tuned for a
                // flying projectile) rather than the character rig, and this game explicitly
                // per-perspective layer-masks what's visible (see PlayerRig.SetTypemaskThirdPerson)
                // - a held item needs the hand bone's own layer to render for the local player at
                // all, not whatever layer its source prefab happened to be on.
                SetLayerRecursively(prop, hand.gameObject.layer);

                prop.SetActive(true);
                s_HeldProps[__instance] = prop;
            }

            SetupAnimationOverlay(__instance, player);
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

            if (s_AnimationOverlays.TryGetValue(__instance, out AnimationOverlay overlay))
            {
                if (overlay.Graph.IsValid())
                {
                    overlay.Graph.Destroy();
                }
                s_AnimationOverlays.Remove(__instance);
            }
        }

        [HarmonyPatch("Fire")]
        [HarmonyPostfix]
        public static void Fire_Postfix(RocketJumpModifier __instance)
        {
            if (s_AnimationOverlays.TryGetValue(__instance, out AnimationOverlay overlay))
            {
                TryStartOneShot(overlay, FireOverlayInput, s_FireClip);
            }
        }

        [HarmonyPatch("Reload")]
        [HarmonyPostfix]
        public static void Reload_Postfix(RocketJumpModifier __instance)
        {
            if (s_AnimationOverlays.TryGetValue(__instance, out AnimationOverlay overlay))
            {
                TryStartOneShot(overlay, ReloadOverlayInput, s_ReloadClip);
            }
        }

        [HarmonyPatch(nameof(RocketJumpModifier.Update))]
        [HarmonyPostfix]
        public static void Update_Postfix(RocketJumpModifier __instance)
        {
            if (!s_AnimationOverlays.TryGetValue(__instance, out AnimationOverlay overlay) || overlay.Player == null)
            {
                return;
            }

            // A one-shot (fire/reload/jump-pre/jump-post) in progress always keeps priority over
            // the continuous idle/move/jump-loop pose below, until it finishes.
            if (overlay.ActiveOneShotInput >= 0)
            {
                if (Time.time < overlay.OneShotEndTime)
                {
                    return;
                }

                int finishedInput = overlay.ActiveOneShotInput;
                overlay.Mixer.SetInputWeight(finishedInput, 0f);
                overlay.ActiveOneShotInput = -1;

                // Coming out of the takeoff anticipation clip, go straight into the airborne loop
                // rather than falling through to a single frame of normal/idle pose first.
                if (finishedInput == JumpPreOverlayInput && s_JumpLoopClip != null && !overlay.Player.Grounded)
                {
                    SetContinuous(overlay, JumpLoopOverlayInput);
                    return;
                }
            }

            UpdateGroundedTransitions(overlay);
        }

        /// <summary>
        /// Detects takeoff/landing edges and picks the right continuous pose the rest of the
        /// time. Re-evaluated every frame (no cached "was idle last frame" check on the
        /// continuous side) so whatever weights a just-finished one-shot left behind always get
        /// corrected on the very next frame rather than relying on a stale comparison.
        /// </summary>
        private static void UpdateGroundedTransitions(AnimationOverlay overlay)
        {
            bool grounded = overlay.Player.Grounded;

            if (overlay.WasGrounded && !grounded)
            {
                overlay.WasGrounded = false;
                if (TryStartOneShot(overlay, JumpPreOverlayInput, s_JumpPreClip))
                {
                    return;
                }
            }
            else if (!overlay.WasGrounded && grounded)
            {
                overlay.WasGrounded = true;
                if (TryStartOneShot(overlay, JumpPostOverlayInput, s_JumpPostClip))
                {
                    return;
                }
            }

            if (!grounded)
            {
                SetContinuous(overlay, s_JumpLoopClip != null ? JumpLoopOverlayInput : -1);
                return;
            }

            // Only one crouch pose is available (stationary), so it's used for crouching outright
            // regardless of speed rather than trying to distinguish crouch-walking from
            // crouch-standing.
            if (s_CrouchClip != null && IsCrouching(overlay.Player))
            {
                SetContinuous(overlay, CrouchOverlayInput);
                return;
            }

            bool moving = overlay.Player.FlatSpeed >= IdleSpeedThreshold;
            if (moving && s_MoveClip != null)
            {
                SetContinuous(overlay, MoveOverlayInput);
            }
            else if (!moving && s_IdleClip != null)
            {
                SetContinuous(overlay, IdleOverlayInput);
            }
            else
            {
                SetContinuous(overlay, -1);
            }
        }

        private static bool IsCrouching(PlayerController player)
        {
            return CrouchingProperty != null && (bool)CrouchingProperty.GetValue(player);
        }

        private static void SetupAnimationOverlay(RocketJumpModifier instance, PlayerController player)
        {
            EnsureAnimationClipsLoaded();
            if (s_FireClip == null && s_ReloadClip == null && s_IdleClip == null && s_MoveClip == null
                && s_JumpPreClip == null && s_JumpLoopClip == null && s_JumpPostClip == null && s_CrouchClip == null)
            {
                return;
            }

            Animator animator = GetAnimator(player);
            if (animator == null || animator.runtimeAnimatorController == null)
            {
                return;
            }

            PlayableGraph graph = PlayableGraph.Create("RocketJumpAnimationOverlay");
            AnimationMixerPlayable mixer = AnimationMixerPlayable.Create(graph, MixerInputCount);

            // Wrapping the animator's own existing controller (rather than replacing it) keeps
            // all normal locomotion/ability animation - and every Animator.SetBool/SetFloat call
            // the rest of the game's code already makes - working exactly as it did before this
            // graph existed.
            AnimatorControllerPlayable controllerPlayable = AnimatorControllerPlayable.Create(graph, animator.runtimeAnimatorController);
            mixer.ConnectInput(BaseControllerInput, controllerPlayable, 0);
            mixer.SetInputWeight(BaseControllerInput, 1f);

            ConnectClip(graph, mixer, FireOverlayInput, s_FireClip);
            ConnectClip(graph, mixer, ReloadOverlayInput, s_ReloadClip);
            ConnectClip(graph, mixer, IdleOverlayInput, s_IdleClip);
            ConnectClip(graph, mixer, MoveOverlayInput, s_MoveClip);
            ConnectClip(graph, mixer, JumpPreOverlayInput, s_JumpPreClip);
            ConnectClip(graph, mixer, JumpLoopOverlayInput, s_JumpLoopClip);
            ConnectClip(graph, mixer, JumpPostOverlayInput, s_JumpPostClip);
            ConnectClip(graph, mixer, CrouchOverlayInput, s_CrouchClip);

            AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "RocketJumpAnimationOverlayOutput", animator);
            output.SetSourcePlayable(mixer);
            graph.Play();

            s_AnimationOverlays[instance] = new AnimationOverlay { Player = player, Graph = graph, Mixer = mixer };
        }

        private static void ConnectClip(PlayableGraph graph, AnimationMixerPlayable mixer, int inputIndex, AnimationClip clip)
        {
            if (clip == null)
            {
                return;
            }

            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(graph, clip);
            mixer.ConnectInput(inputIndex, clipPlayable, 0);
            mixer.SetInputWeight(inputIndex, 0f);
        }

        /// <summary>
        /// Starts a momentary one-shot (fire/reload/jump-pre/jump-post), pre-empting whatever
        /// continuous pose was playing. Returns false (no-op) if the clip isn't available, so
        /// callers can fall through to their own next-best behaviour.
        /// </summary>
        private static bool TryStartOneShot(AnimationOverlay overlay, int inputIndex, AnimationClip clip)
        {
            if (clip == null)
            {
                return false;
            }

            if (overlay.ContinuousInput >= 0)
            {
                overlay.Mixer.SetInputWeight(overlay.ContinuousInput, 0f);
                overlay.ContinuousInput = -1;
            }

            if (overlay.ActiveOneShotInput >= 0 && overlay.ActiveOneShotInput != inputIndex)
            {
                overlay.Mixer.SetInputWeight(overlay.ActiveOneShotInput, 0f);
            }

            overlay.Mixer.GetInput(inputIndex).SetTime(0);
            overlay.Mixer.SetInputWeight(BaseControllerInput, 0f);
            overlay.Mixer.SetInputWeight(inputIndex, 1f);
            overlay.ActiveOneShotInput = inputIndex;
            overlay.OneShotEndTime = Time.time + (float)clip.length;
            return true;
        }

        /// <summary>
        /// Switches the sustained pose (idle/move/jump-loop), or -1 to fall back to the animator's
        /// own normal output. Always writes both weights explicitly rather than skipping when
        /// nothing "changed", since a just-finished one-shot can leave the mixer in a state that
        /// doesn't match overlay.ContinuousInput's last known value.
        /// </summary>
        private static void SetContinuous(AnimationOverlay overlay, int inputIndex)
        {
            if (overlay.ContinuousInput >= 0 && overlay.ContinuousInput != inputIndex)
            {
                overlay.Mixer.SetInputWeight(overlay.ContinuousInput, 0f);
            }

            overlay.ContinuousInput = inputIndex;

            if (inputIndex >= 0)
            {
                overlay.Mixer.SetInputWeight(inputIndex, 1f);
                overlay.Mixer.SetInputWeight(BaseControllerInput, 0f);
            }
            else
            {
                overlay.Mixer.SetInputWeight(BaseControllerInput, 1f);
            }
        }

        private static Animator GetAnimator(PlayerController player)
        {
            if (player.PlayerRigAnimator == null)
            {
                return null;
            }

            return (Animator)AccessTools.Field(typeof(PlayerRigAnimator), "m_Animator").GetValue(player.PlayerRigAnimator);
        }

        /// <summary>
        /// Both clips are shipped in the same "waterbomberassets" bundle as the held gun prop, so
        /// this only needs to open the bundle once, regardless of which of the two is requested
        /// first.
        /// </summary>
        private static void EnsureAnimationClipsLoaded()
        {
            if (s_AnimationClipsLoaded)
            {
                return;
            }

            s_AnimationClipsLoaded = true;

            try
            {
                string pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string bundlePath = Path.Combine(pluginDirectory ?? string.Empty, AssetBundleFileName);

                if (!File.Exists(bundlePath))
                {
                    return;
                }

                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
                if (bundle == null)
                {
                    Plugin.Logger.LogWarning($"[Archipelago] Found '{AssetBundleFileName}' but failed to load it as an AssetBundle (animation clips).");
                    return;
                }

                s_FireClip = LoadClip(bundle, FireClipAssetName);
                s_ReloadClip = LoadClip(bundle, ReloadClipAssetName);
                s_IdleClip = LoadClip(bundle, IdleClipAssetName);
                s_MoveClip = LoadClip(bundle, MoveClipAssetName);
                s_JumpPreClip = LoadClip(bundle, JumpPreClipAssetName);
                s_JumpLoopClip = LoadClip(bundle, JumpLoopClipAssetName);
                s_JumpPostClip = LoadClip(bundle, JumpPostClipAssetName);
                s_CrouchClip = LoadClip(bundle, CrouchClipAssetName);

                // The clips themselves stay valid (they're what LoadAsset returned); only the
                // bundle's own loaded-object registry is torn down here.
                bundle.Unload(false);
            }
            catch (System.Exception exception)
            {
                Plugin.Logger.LogError($"[Archipelago] Exception while loading rocket jump animation clips: {exception}");
            }
        }

        private static AnimationClip LoadClip(AssetBundle bundle, string assetName)
        {
            AnimationClip clip = bundle.LoadAsset<AnimationClip>(assetName);
            if (clip == null)
            {
                Plugin.Logger.LogWarning($"[Archipelago] AssetBundle '{AssetBundleFileName}' did not contain a '{assetName}' animation clip.");
            }

            return clip;
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
