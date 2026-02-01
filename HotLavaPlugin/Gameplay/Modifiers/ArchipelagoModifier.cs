using FMOD.Studio;
using Klei.HotLava;
using Klei.HotLava.Audio;
using Klei.HotLava.Character;
using Klei.HotLava.Character.Modifiers;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Gameplay.Modifiers
{
    /// <summary>
    /// Archipelago modifier for locking/unlocking abilities
    /// </summary>
    public class ArchipelagoModifier : PlayerControllerModifier
    {
        #region AP Fields
        private bool _hasDoubleJump = true;
        private bool _hasBoostJump = true;
        private bool _hasSlideJump = true;
        private bool _hasVaultJump = true;
        #endregion

        #region DoubleJumpModifier
        [Header("DoubleJumpModifier")]
        [Tooltip("Don't double jump when on these materials. Probably want Trampoline and Bouncy.")]
        public List<PhysicMaterial> m_NoDoubleJumpMaterials;

        private bool m_DidDoubleJump;

        [Tooltip("Amount of current speed to add after a double jump.")]
        public float m_DoubleJumpSpeedMultiplier = 0.7f;

        [Tooltip("Amount of height you gain on a double jump.")]
        public float m_DoubleJumpHeightMultiplier = 0.8f;

        [Tooltip("Maximum current speed to carry into the jump. Most useful when m_DoubleJumpSpeedMultiplier is above 1.")]
        public float m_DoubleJumpMaxSpeedBoost = 1000f;

        [Tooltip("Gravity applied during double jump. Otherwise uses m_GravityMultiplier.")]
        public float m_DoubleJumpGravityMultiplier = 1f;

        public bool DidDoubleJump => m_DidDoubleJump;

        public override float GravityMultiplier
        {
            get
            {
                if (m_DidDoubleJump)
                {
                    return m_DoubleJumpGravityMultiplier;
                }

                return base.GravityMultiplier;
            }
        }
        #endregion

        #region Lunge Settings
        [Header("Lunge Settings")]
        //Animation Curves are initialized in CharacterModifierDataPatches
        public AnimationCurve m_LungeVelociyCurve;

        public AnimationCurve m_ClamberVelocityCurve;

        public bool m_AlwaysAllowClamber;

        public float m_ClimbDetectHeightFalling = 0.3f;

        public float m_ClimbDetectHeightRising = 0.6f;

        public float m_ClimbDetectMinVelocity = 1f;

        private float m_LungeTimer;

        private float m_ClamberTimer;

        private bool m_IsLunging;

        private bool m_CanLunge = true;

        private bool m_IsClambering;

        private bool m_ActionPressed;

        private bool m_ReadyState;

        private float m_ClamberDuration;

        private float m_OriginalVelocityFlat;

        private Vector3 m_OriginalVelocity;

        private Vector3 m_LungeDirection;

        private EventInstance LungeJump;
        #endregion

        #region Slide Jump
        private bool lastFrameSlide;

        private bool previouslyClimbing;
        #endregion

        #region Abilities
        public override bool CanBhop => _hasBoostJump;
        public override bool CanPerfectJump => _hasBoostJump || _hasVaultJump;

        public override bool CanGrab => !m_IsClambering;
        public override bool CanCrouch => true;
        public override bool CanSurf => true;
        public override bool CanWallRun => true;
        public override bool CanReach => !m_IsLunging;
        #endregion

        #region Cached Reflection
        private MethodInfo _isJumpPressedThisFrameMethod = typeof(PlayerController).GetMethod("IsJumpPressedThisFrame", BindingFlags.NonPublic | BindingFlags.Instance);
        private FieldInfo _inFanTunnelField = typeof(PlayerController).GetField("m_InFanTunnel", BindingFlags.NonPublic | BindingFlags.Instance);
        private FieldInfo _canBounceField = typeof(PlayerController).GetField("CanBounce", BindingFlags.NonPublic | BindingFlags.Instance);
        private FieldInfo _itemGrabberField = typeof(PlayerController).GetField("m_ItemGrabber", BindingFlags.Instance | BindingFlags.NonPublic);
        private MethodInfo _dropAllItemMethod = typeof(ItemGrabber).GetMethod("DropAllItem", BindingFlags.Instance | BindingFlags.NonPublic);
        #endregion

        #region Unity Lifecycle
        public override void Update()
        {
            if (!m_PlayerController.IsMine) return;

            if (_hasVaultJump)
            {
                VaultJumpUpdate();
            }
            if (_hasDoubleJump)
            {
                DoubleJumpUpdate();
            }
        }

        public override void FixedUpdate()
        {
            if (_hasSlideJump)
            {
                SlideJumpFixedUpdate();
            }

            if (!m_PlayerController.IsMine) return;

            if (_hasVaultJump)
            {
                VaultJumpFixedUpdate();
            }
        }

        public override void OnVictory()
        {
            m_PlayerController.PlayerRigAnimator.SetIsEdgeClimbing(to: false);
            m_PlayerController.PlayerRigAnimator.SetIsLunging(to: false);
        }
        #endregion

        #region Double Jump Logic
        private void DoubleJumpUpdate()
        {
            bool canDoubleJump = PlayerCanPerformAirborneAbility();

            if (_hasBoostJump)
            {
                // Reduce the feeling of double jumping when you think you should boost
                canDoubleJump = canDoubleJump && m_PlayerController.DistanceToGround > 0.75f;
            }

            m_PlayerController.PlayerRigAnimator.SetDoubleJump(to: false);

            if (canDoubleJump && !m_DidDoubleJump && IsJumpPressedThisFrame() && CanPerformDoubleJumpAtCurrentPosition())
            {
                PerformDoubleJump();
            }
            else if (!canDoubleJump)
            {
                m_DidDoubleJump = false;
            }
        }

        private bool CanPerformDoubleJumpAtCurrentPosition()
        {
            float distanceToGround = m_PlayerController.Grounder.DistanceToGround;
            float verticalVelocity = m_PlayerController.Velocity.y;

            // Player is too close to ground and falling fast
            if (distanceToGround < 1.8f && verticalVelocity < -1.5f)
            {
                // Only allow if not very close to ground or on a valid material
                return distanceToGround >= 0.3f || !m_NoDoubleJumpMaterials.Contains(m_PlayerController.Grounder.NearGroundMaterial);
            }

            return true;
        }

        private void PerformDoubleJump()
        {
            m_PlayerController.PlayerRigAnimator.SetDoubleJump(to: true);
            m_PlayerController.PlayerAudioComponent.PlayFootstep(eFootstepAction.JUMP);

            float originalMaxJumpModifier = m_MaxJumpModifier;
            m_MaxJumpModifier = m_DoubleJumpHeightMultiplier;

            float speedBoost = Mathf.Clamp(
                m_PlayerController.JumpFlatSpeed * m_DoubleJumpSpeedMultiplier,
                0f,
                m_DoubleJumpMaxSpeedBoost
            );

            m_PlayerController.Jumper.Jump(
                m_PlayerController.GetCachedInput().normalized,
                m_PlayerController.RigidBody.velocity.normalized,
                speedBoost
            );

            m_MaxJumpModifier = originalMaxJumpModifier;
            m_DidDoubleJump = true;
        }
        #endregion

        #region Vault Jump Logic
        private void VaultJumpUpdate()
        {
            m_ReadyState = PlayerCanPerformAirborneAbility()
                && !m_PlayerController.Victory
                && !m_PlayerController.HasPendingDeath;

            m_ActionPressed = m_ActionPressed || m_PlayerController.GetActionJustPressed();
        }

        private void VaultJumpFixedUpdate()
        {
            if (m_ReadyState)
            {
                HandleVaultJumpInReadyState();
            }
            else
            {
                ResetVaultJumpState();
            }

            m_ActionPressed = false;
            m_PlayerController.PlayerRigAnimator.SetIsEdgeClimbing(m_IsClambering);
            m_PlayerController.PlayerRigAnimator.SetIsLunging(m_IsLunging);
        }

        private void HandleVaultJumpInReadyState()
        {
            float cameraDot = Vector3.Dot(m_PlayerController.CameraForward, Vector3.up);

            if (TryStartClamber(cameraDot))
            {
                return;
            }

            UpdateClambering();

            if (m_ActionPressed && !m_IsLunging && m_CanLunge)
            {
                StartLunge();
            }

            UpdateLunging();
        }

        private bool TryStartClamber(float cameraDot)
        {
            if (m_IsClambering)
            {
                return false;
            }

            bool canClamber = PerformClimbCheck(cameraDot, out float detectDistance, out float climbHeight);
            bool movingForward = m_PlayerController.GetCachedInput().y > 0.7f;
            bool forwardVelocityBelowMin = m_PlayerController.Velocity.y < m_ClimbDetectMinVelocity;
            bool allowClamber = m_AlwaysAllowClamber ? (forwardVelocityBelowMin && movingForward) : !m_CanLunge;

            if (allowClamber && canClamber)
            {
                StartClamber(cameraDot, detectDistance, climbHeight);
                return true;
            }

            return false;
        }

        private void StartClamber(float cameraDot, float detectDistance, float climbHeight)
        {
            m_IsClambering = true;
            m_OriginalVelocity = m_PlayerController.RigidBody.velocity;

            DropAllHeldItems();

            Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.HANDGRAB_ON_VAULT, m_PlayerController.gameObject).StartAndRelease();

            // If player is farther from the ledge, make climb animation shorter
            float distanceWeight = (detectDistance - 0.2f) * 0.4f;
            float climbTimeModifier = Mathf.Clamp01(cameraDot) * 0.24f - distanceWeight;
            m_ClamberDuration = m_ClamberVelocityCurve.keys[m_ClamberVelocityCurve.length - 1].time + climbTimeModifier;

            m_PlayerController.PlayerRigAnimator.SetEdgeClimbingHeight(climbHeight);
        }

        private void UpdateClambering()
        {
            if (!m_IsClambering)
            {
                return;
            }

            m_ClamberTimer += Time.fixedDeltaTime;

            if (m_ClamberTimer < m_ClamberDuration)
            {
                Vector3 climbDirection = Vector3.Slerp(m_PlayerController.m_CameraBase.forward, Vector3.up, 0.7f);
                m_PlayerController.RigidBody.velocity = climbDirection * m_ClamberVelocityCurve.Evaluate(m_ClamberTimer);
            }
            else
            {
                FinishClamber();
            }
        }

        private void FinishClamber()
        {
            m_OriginalVelocity.y *= 0.3f;
            m_OriginalVelocity.x = Mathf.Min(m_OriginalVelocity.x, m_PlayerController.ForwardSpeed);
            m_OriginalVelocity.z = Mathf.Min(m_OriginalVelocity.z, m_PlayerController.ForwardSpeed);
            m_PlayerController.RigidBody.velocity = m_OriginalVelocity;

            Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.HANDGRAB_OFF_VALUT, m_PlayerController.gameObject).StartAndRelease();

            m_ClamberTimer = 0f;
            m_IsClambering = false;
        }

        private void StartLunge()
        {
            m_OriginalVelocity = m_PlayerController.RigidBody.velocity;
            m_OriginalVelocityFlat = m_PlayerController.FlatSpeed;
            m_LungeDirection = m_PlayerController.CameraForward;

            LungeJump = Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.JUMP_LUNGE_2D3D, m_PlayerController.gameObject)
                .SetAudioParameter("3d", (!m_PlayerController.IsCameraAttached) ? 1 : 0)
                .SetAudioParameter("velocity_horizontal", m_OriginalVelocityFlat)
                .StartAndRelease();

            m_IsLunging = true;
            m_CanLunge = false;
        }

        private void UpdateLunging()
        {
            if (!m_IsLunging)
            {
                return;
            }

            m_LungeTimer += Time.fixedDeltaTime;

            if (m_LungeTimer < m_LungeVelociyCurve.keys[m_LungeVelociyCurve.length - 1].time)
            {
                m_PlayerController.RigidBody.velocity = m_OriginalVelocity + m_LungeDirection * m_LungeVelociyCurve.Evaluate(m_LungeTimer);
            }
            else
            {
                FinishLunge();
            }
        }

        private void FinishLunge()
        {
            m_PlayerController.RigidBody.velocity = m_AlwaysAllowClamber ? m_OriginalVelocity : Vector3.zero;
            m_IsLunging = false;
            m_LungeTimer = 0f;
        }

        private void ResetVaultJumpState()
        {
            if (m_IsLunging)
            {
                m_PlayerController.RigidBody.velocity = Vector3.zero;
                if (m_PlayerController.Climbing)
                {
                    m_PlayerController.AttachVelocity = m_PlayerController.AttachVelocity.normalized * m_PlayerController.ForwardSpeed;
                }
            }

            m_LungeTimer = 0f;
            m_ClamberTimer = 0f;
            m_CanLunge = true;
            m_IsLunging = false;
            m_IsClambering = false;

            if (LungeJump.isValid())
            {
                LungeJump.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }

        private bool PerformClimbCheck(float cameraDot, out float detectDistance, out float climbHeight)
        {
            RaycastHit[] hits = new RaycastHit[2];
            detectDistance = 0f;
            climbHeight = 0f;

            Vector3[] raycastOrigins = GetClimbCheckOrigins(cameraDot);
            float scanRange = GetClimbScanRange(cameraDot);

            foreach (Vector3 origin in raycastOrigins)
            {
                if (CheckForClimbableSpot(origin, scanRange, hits, out detectDistance, out climbHeight))
                {
                    if (HasCeilingObstruction(origin))
                    {
                        return false;
                    }
                    return true;
                }
            }

            return false;
        }

        private Vector3[] GetClimbCheckOrigins(float cameraDot)
        {
            float farOffset = m_PlayerController.ColliderRadius + 0.16f * m_PlayerController.Scale;
            float midOffset = m_PlayerController.ColliderRadius + 0.08f * m_PlayerController.Scale;
            float nearOffset = m_PlayerController.ColliderRadius + 0.02f * m_PlayerController.Scale;

            Vector3 checkDirection = (cameraDot > 0f) ? m_PlayerController.CameraForward : m_PlayerController.m_CameraBase.forward;

            return
            [
                m_PlayerController.m_CameraBase.position + checkDirection * farOffset,
                m_PlayerController.m_CameraBase.position + checkDirection * midOffset,
                m_PlayerController.m_CameraBase.position + checkDirection * nearOffset
            ];
        }

        private float GetClimbScanRange(float cameraDot)
        {
            float scanRange = (m_PlayerController.Velocity.y > -0.8f)
                ? m_ClimbDetectHeightRising
                : m_ClimbDetectHeightFalling;

            if (cameraDot > 0.6f)
            {
                scanRange *= 0.9f;
            }

            return scanRange;
        }

        private bool CheckForClimbableSpot(Vector3 origin, float scanRange, RaycastHit[] hits, out float detectDistance, out float climbHeight)
        {
            detectDistance = 0f;
            climbHeight = 0f;

            int hitCount = Physics.RaycastNonAlloc(
                origin,
                Vector3.down,
                hits,
                scanRange,
                LevelSingleton.m_LayerWorld,
                QueryTriggerInteraction.Ignore
            );

            if (hitCount <= 0)
            {
                return false;
            }

            for (int i = 0; i < hitCount; i++)
            {
                // Make sure surface is flat enough to be climbed on
                if (Vector3.Dot(hits[i].normal, Vector3.up) > 0.6f)
                {
                    climbHeight = hits[i].point.y - m_PlayerController.BasePosition.y;
                    detectDistance = hits[i].distance;
                    return true;
                }
            }

            return false;
        }

        private bool HasCeilingObstruction(Vector3 origin)
        {
            RaycastHit[] ceilingHits = new RaycastHit[1];

            int ceilingHitCount = Physics.RaycastNonAlloc(
                origin,
                Vector3.up,
                ceilingHits,
                1.5f,
                LevelSingleton.m_LayerWorld,
                QueryTriggerInteraction.Ignore
            );

            if (ceilingHitCount <= 0)
            {
                ceilingHitCount = Physics.RaycastNonAlloc(
                    m_PlayerController.m_CameraBase.position,
                    Vector3.up,
                    ceilingHits,
                    1.5f,
                    LevelSingleton.m_LayerWorld,
                    QueryTriggerInteraction.Ignore
                );
            }

            return ceilingHitCount > 0;
        }
        #endregion

        #region Slide Jump Logic
        private void SlideJumpFixedUpdate()
        {
            if (m_PlayerController.ExternalJump || m_PlayerController.Surfing)
            {
                return;
            }

            UpdateClimbingState();

            if (m_PlayerController.JustJumped && !m_PlayerController.PreviouslySurfing && !previouslyClimbing)
            {
                HandleJumpFromSlide();
            }
            else
            {
                UpdateSlideState();
            }
        }

        private void UpdateClimbingState()
        {
            if (!previouslyClimbing && m_PlayerController.Climbing)
            {
                previouslyClimbing = true;
            }

            if (previouslyClimbing && m_PlayerController.Grounded)
            {
                previouslyClimbing = false;
            }
        }

        private void HandleJumpFromSlide()
        {
            if (lastFrameSlide)
            {
                PerformSlideJump();
            }
            else
            {
                PerformNormalJump();
            }
        }

        private void PerformSlideJump()
        {
            if (Time.time - m_PlayerController.CrouchStateChangeTimeStamp <= 0.15f)
            {
                return;
            }

            Vector3 velocity = m_PlayerController.m_CameraBase.forward * 5.99f;
            velocity.y = m_PlayerController.RigidBody.velocity.y;
            m_PlayerController.RigidBody.velocity = velocity;
            m_PlayerController.PlayerRigAnimator.SetJumpCharge(0.3f);

            if (m_PlayerController.IsCameraAttached)
            {
                Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.JUMP_SLIDEJUMP, m_PlayerController.gameObject)
                    .SetAudioParameter("velocity_horizontal", m_PlayerController.FlatSpeed)
                    .SetAudioParameter("3d", (!m_PlayerController.IsCameraAttached) ? 1 : 0)
                    .StartAndRelease();
            }
        }

        private void PerformNormalJump()
        {
            Vector3 velocity = m_PlayerController.RigidBody.velocity;
            velocity.y = 0f;

            float flatSpeedMultiplier = GetJumpSpeedMultiplier();
            velocity = velocity.normalized * Mathf.Max(
                m_PlayerController.FlatSpeed * flatSpeedMultiplier,
                m_PlayerController.CurrentTargetSpeed
            );

            velocity.y = m_PlayerController.RigidBody.velocity.y;
            m_PlayerController.RigidBody.velocity = velocity;
        }

        private float GetJumpSpeedMultiplier()
        {
            // Don't lose speed while jumping if player has boost or vault jump
            if (_hasBoostJump || _hasVaultJump)
            {
                return 1f;
            }

            return 0.85f;
        }

        private void UpdateSlideState()
        {
            lastFrameSlide = m_PlayerController.IsSliding;

            if (!m_PlayerController.CrouchJumped)
            {
                m_PlayerController.PlayerRigAnimator.SetJumpCharge(0f);
            }

            if (m_PlayerController.Grounded && m_PlayerController.IsSliding)
            {
                AdjustVelocityForSlope();
            }
        }

        private void AdjustVelocityForSlope()
        {
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(
                m_PlayerController.RigidBody.velocity,
                m_PlayerController.Grounder.GroundContactNormal
            );

            m_PlayerController.RigidBody.velocity = projectedVelocity.normalized * m_PlayerController.CurrentTargetSpeed;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Checks if the player is currently airborne, not on a slime/wall run, not in a fan tunnel, 
        /// not climbing, not hitting a trampoline/balloon, and didn't jump too recently
        /// </summary>
        private bool PlayerCanPerformAirborneAbility()
        {
            bool inFanTunnel = (bool)_inFanTunnelField.GetValue(m_PlayerController);
            CanBounce canBounce = (CanBounce)_canBounceField.GetValue(m_PlayerController);

            return !m_PlayerController.Grounded
                && !m_PlayerController.WallRun
                && !inFanTunnel
                && !m_PlayerController.Climbing
                && !canBounce.JustBounced
                && !m_PlayerController.InJumpGracePeriod;
        }

        private bool IsJumpPressedThisFrame()
        {
            return (bool)_isJumpPressedThisFrameMethod.Invoke(m_PlayerController, []);
        }

        private void DropAllHeldItems()
        {
            ItemGrabber itemGrabber = (ItemGrabber)_itemGrabberField.GetValue(m_PlayerController);
            _dropAllItemMethod.Invoke(itemGrabber, []);
        }
        #endregion
    }
}