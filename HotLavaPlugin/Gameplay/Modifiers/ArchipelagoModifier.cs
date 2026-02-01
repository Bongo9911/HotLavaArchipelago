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

        #region DoubleJump Settings
        [Header("DoubleJumpModifier")]
        [Tooltip("Don't double jump when on these materials. Probably want Trampoline and Bouncy.")]
        //Set in CharacterModifierDataPatches
        public List<PhysicMaterial> m_NoDoubleJumpMaterials;

        [Tooltip("Amount of current speed to add after a double jump.")]
        [Range(0f, 2f)]
        public float m_DoubleJumpSpeedMultiplier = 0.7f;

        [Tooltip("Amount of height you gain on a double jump.")]
        [Range(0f, 2f)]
        public float m_DoubleJumpHeightMultiplier = 0.8f;

        [Tooltip("Maximum current speed to carry into the jump. Most useful when m_DoubleJumpSpeedMultiplier is above 1.")]
        [Min(0f)]
        public float m_DoubleJumpMaxSpeedBoost = 1000f;

        [Tooltip("Gravity applied during double jump. Otherwise uses m_GravityMultiplier.")]
        [Range(0f, 5f)]
        public float m_DoubleJumpGravityMultiplier = 1f;

        private bool m_DidDoubleJump;

        public bool DidDoubleJump => m_DidDoubleJump;

        public override float GravityMultiplier => m_DidDoubleJump ? m_DoubleJumpGravityMultiplier : base.GravityMultiplier;
        #endregion

        #region Vault/Lunge Settings
        [Header("Lunge Settings")]
        //Animation Curves set in CharacterModifierDataPatches
        public AnimationCurve m_LungeVelociyCurve;
        public AnimationCurve m_ClamberVelocityCurve;

        [Tooltip("Always allow clamber regardless of lunge state")]
        public bool m_AlwaysAllowClamber;

        [Tooltip("Height range to detect climbable ledges when falling")]
        [Range(0.1f, 1f)]
        public float m_ClimbDetectHeightFalling = 0.3f;

        [Tooltip("Height range to detect climbable ledges when rising")]
        [Range(0.1f, 1f)]
        public float m_ClimbDetectHeightRising = 0.6f;

        [Tooltip("Minimum vertical velocity required to trigger climb detection")]
        [Min(0f)]
        public float m_ClimbDetectMinVelocity = 1f;

        private float m_LungeTimer;
        private float m_ClamberTimer;
        private float m_ClamberDuration;
        private float m_OriginalVelocityFlat;

        private Vector3 m_OriginalVelocity;
        private Vector3 m_LungeDirection;

        private bool m_IsLunging;
        private bool m_CanLunge = true;
        private bool m_IsClambering;
        private bool m_ActionPressed;
        private bool m_ReadyState;

        private EventInstance LungeJump;
        #endregion

        #region Slide Jump State
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
        private MethodInfo _isJumpPressedThisFrameMethod;
        private FieldInfo _inFanTunnelField;
        private FieldInfo _canBounceField;
        private FieldInfo _itemGrabberField;
        private MethodInfo _dropAllItemMethod;
        #endregion

        #region Cached Constants
        // Pre-calculated constants to avoid repeated calculations
        private const float BOOST_JUMP_MIN_DISTANCE = 0.75f;
        private const float DOUBLE_JUMP_GROUND_CHECK_DISTANCE = 1.8f;
        private const float DOUBLE_JUMP_FALLING_VELOCITY = -1.5f;
        private const float DOUBLE_JUMP_MIN_DISTANCE = 0.3f;
        private const float NORMAL_DOT_THRESHOLD = 0.6f;
        private const float FORWARD_INPUT_THRESHOLD = 0.7f;
        private const float CAMERA_DOT_THRESHOLD = 0.6f;
        private const float CAMERA_DOT_HIGH = 0.9f;
        private const float FALLING_VELOCITY_THRESHOLD = -0.8f;
        private const float CLIMB_VELOCITY_SCALE = 0.3f;
        private const float SLIDE_JUMP_FORWARD_SPEED = 5.99f;
        private const float SLIDE_JUMP_CHARGE = 0.3f;
        private const float SLIDE_JUMP_MIN_TIME = 0.15f;
        private const float NORMAL_JUMP_SPEED_MULT = 0.85f;
        private const float CLAMBER_DIRECTION_LERP = 0.7f;
        private const float DISTANCE_WEIGHT_MULT = 0.4f;
        private const float DISTANCE_OFFSET = 0.2f;
        private const float CLIMB_TIME_MODIFIER = 0.24f;
        private const float CEILING_CHECK_HEIGHT = 1.5f;

        // Raycast offset multipliers
        private const float FAR_OFFSET_MULT = 0.16f;
        private const float MID_OFFSET_MULT = 0.08f;
        private const float NEAR_OFFSET_MULT = 0.02f;
        #endregion

        #region Cached Allocations
        // Reusable arrays to avoid GC allocations
        private readonly RaycastHit[] _raycastHits = new RaycastHit[2];
        private readonly RaycastHit[] _ceilingHits = new RaycastHit[1];
        private readonly Vector3[] _raycastOrigins = new Vector3[3];
        private readonly object[] _emptyArgs = new object[0];
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            CacheReflectionMembers();
        }

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

        #region Initialization
        private void CacheReflectionMembers()
        {
            if (_isJumpPressedThisFrameMethod != null) return;

            var playerControllerType = typeof(PlayerController);
            var itemGrabberType = typeof(ItemGrabber);
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            _isJumpPressedThisFrameMethod = playerControllerType.GetMethod("IsJumpPressedThisFrame", bindingFlags);
            _inFanTunnelField = playerControllerType.GetField("m_InFanTunnel", bindingFlags);
            _canBounceField = playerControllerType.GetField("CanBounce", bindingFlags);
            _itemGrabberField = playerControllerType.GetField("m_ItemGrabber", bindingFlags);
            _dropAllItemMethod = itemGrabberType.GetMethod("DropAllItem", bindingFlags);
        }
        #endregion

        #region Double Jump Logic
        private void DoubleJumpUpdate()
        {
            bool canDoubleJump = PlayerCanPerformAirborneAbility();

            if (_hasBoostJump)
            {
                // Reduce the feeling of double jumping when you think you should boost
                canDoubleJump = canDoubleJump && m_PlayerController.DistanceToGround > BOOST_JUMP_MIN_DISTANCE;
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
            var grounder = m_PlayerController.Grounder;
            float distanceToGround = grounder.DistanceToGround;
            float verticalVelocity = m_PlayerController.Velocity.y;

            // Player is too close to ground and falling fast
            if (distanceToGround < DOUBLE_JUMP_GROUND_CHECK_DISTANCE && verticalVelocity < DOUBLE_JUMP_FALLING_VELOCITY)
            {
                // Only allow if not very close to ground or on a valid material
                return distanceToGround >= DOUBLE_JUMP_MIN_DISTANCE ||
                       !m_NoDoubleJumpMaterials.Contains(grounder.NearGroundMaterial);
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

            var rigidBody = m_PlayerController.RigidBody;
            m_PlayerController.Jumper.Jump(
                m_PlayerController.GetCachedInput().normalized,
                rigidBody.velocity.normalized,
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
            var animator = m_PlayerController.PlayerRigAnimator;
            animator.SetIsEdgeClimbing(m_IsClambering);
            animator.SetIsLunging(m_IsLunging);
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
            if (m_IsClambering) return false;

            bool canClamber = PerformClimbCheck(cameraDot, out float detectDistance, out float climbHeight);
            bool movingForward = m_PlayerController.GetCachedInput().y > FORWARD_INPUT_THRESHOLD;
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
            var rigidBody = m_PlayerController.RigidBody;
            m_OriginalVelocity = rigidBody.velocity;

            DropAllHeldItems();

            Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.HANDGRAB_ON_VAULT, m_PlayerController.gameObject)
                .StartAndRelease();

            // If player is farther from the ledge, make climb animation shorter
            float distanceWeight = (detectDistance - DISTANCE_OFFSET) * DISTANCE_WEIGHT_MULT;
            float climbTimeModifier = Mathf.Clamp01(cameraDot) * CLIMB_TIME_MODIFIER - distanceWeight;
            m_ClamberDuration = m_ClamberVelocityCurve.keys[m_ClamberVelocityCurve.length - 1].time + climbTimeModifier;

            m_PlayerController.PlayerRigAnimator.SetEdgeClimbingHeight(climbHeight);
        }

        private void UpdateClambering()
        {
            if (!m_IsClambering) return;

            m_ClamberTimer += Time.fixedDeltaTime;

            if (m_ClamberTimer < m_ClamberDuration)
            {
                Vector3 climbDirection = Vector3.Slerp(
                    m_PlayerController.m_CameraBase.forward,
                    Vector3.up,
                    CLAMBER_DIRECTION_LERP
                );
                m_PlayerController.RigidBody.velocity = climbDirection * m_ClamberVelocityCurve.Evaluate(m_ClamberTimer);
            }
            else
            {
                FinishClamber();
            }
        }

        private void FinishClamber()
        {
            float forwardSpeed = m_PlayerController.ForwardSpeed;

            m_OriginalVelocity.y *= CLIMB_VELOCITY_SCALE;
            m_OriginalVelocity.x = Mathf.Min(m_OriginalVelocity.x, forwardSpeed);
            m_OriginalVelocity.z = Mathf.Min(m_OriginalVelocity.z, forwardSpeed);
            m_PlayerController.RigidBody.velocity = m_OriginalVelocity;

            Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.HANDGRAB_OFF_VALUT, m_PlayerController.gameObject)
                .StartAndRelease();

            m_ClamberTimer = 0f;
            m_IsClambering = false;
        }

        private void StartLunge()
        {
            var rigidBody = m_PlayerController.RigidBody;
            m_OriginalVelocity = rigidBody.velocity;
            m_OriginalVelocityFlat = m_PlayerController.FlatSpeed;
            m_LungeDirection = m_PlayerController.CameraForward;

            bool isCameraAttached = m_PlayerController.IsCameraAttached;
            LungeJump = Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.JUMP_LUNGE_2D3D, m_PlayerController.gameObject)
                .SetAudioParameter("3d", isCameraAttached ? 0 : 1)
                .SetAudioParameter("velocity_horizontal", m_OriginalVelocityFlat)
                .StartAndRelease();

            m_IsLunging = true;
            m_CanLunge = false;
        }

        private void UpdateLunging()
        {
            if (!m_IsLunging) return;

            m_LungeTimer += Time.fixedDeltaTime;

            if (m_LungeTimer < m_LungeVelociyCurve.keys[m_LungeVelociyCurve.length - 1].time)
            {
                m_PlayerController.RigidBody.velocity = m_OriginalVelocity +
                    m_LungeDirection * m_LungeVelociyCurve.Evaluate(m_LungeTimer);
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
                var rigidBody = m_PlayerController.RigidBody;
                rigidBody.velocity = Vector3.zero;

                if (m_PlayerController.Climbing)
                {
                    m_PlayerController.AttachVelocity = m_PlayerController.AttachVelocity.normalized *
                        m_PlayerController.ForwardSpeed;
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
            detectDistance = 0f;
            climbHeight = 0f;

            PopulateClimbCheckOrigins(cameraDot);
            float scanRange = GetClimbScanRange(cameraDot);

            foreach (Vector3 origin in _raycastOrigins)
            {
                if (CheckForClimbableSpot(origin, scanRange, out detectDistance, out climbHeight))
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

        private void PopulateClimbCheckOrigins(float cameraDot)
        {
            float scale = m_PlayerController.Scale;
            float colliderRadius = m_PlayerController.ColliderRadius;

            float farOffset = colliderRadius + FAR_OFFSET_MULT * scale;
            float midOffset = colliderRadius + MID_OFFSET_MULT * scale;
            float nearOffset = colliderRadius + NEAR_OFFSET_MULT * scale;

            Vector3 checkDirection = (cameraDot > 0f) ?
                m_PlayerController.CameraForward :
                m_PlayerController.m_CameraBase.forward;

            Vector3 basePosition = m_PlayerController.m_CameraBase.position;
            _raycastOrigins[0] = basePosition + checkDirection * farOffset;
            _raycastOrigins[1] = basePosition + checkDirection * midOffset;
            _raycastOrigins[2] = basePosition + checkDirection * nearOffset;
        }

        private float GetClimbScanRange(float cameraDot)
        {
            float scanRange = (m_PlayerController.Velocity.y > FALLING_VELOCITY_THRESHOLD)
                ? m_ClimbDetectHeightRising
                : m_ClimbDetectHeightFalling;

            if (cameraDot > CAMERA_DOT_THRESHOLD)
            {
                scanRange *= CAMERA_DOT_HIGH;
            }

            return scanRange;
        }

        private bool CheckForClimbableSpot(Vector3 origin, float scanRange, out float detectDistance, out float climbHeight)
        {
            detectDistance = 0f;
            climbHeight = 0f;

            int hitCount = Physics.RaycastNonAlloc(
                origin,
                Vector3.down,
                _raycastHits,
                scanRange,
                LevelSingleton.m_LayerWorld,
                QueryTriggerInteraction.Ignore
            );

            if (hitCount <= 0) return false;

            float basePositionY = m_PlayerController.BasePosition.y;

            for (int i = 0; i < hitCount; i++)
            {
                // Make sure surface is flat enough to be climbed on
                if (Vector3.Dot(_raycastHits[i].normal, Vector3.up) > NORMAL_DOT_THRESHOLD)
                {
                    climbHeight = _raycastHits[i].point.y - basePositionY;
                    detectDistance = _raycastHits[i].distance;
                    return true;
                }
            }

            return false;
        }

        private bool HasCeilingObstruction(Vector3 origin)
        {
            int ceilingHitCount = Physics.RaycastNonAlloc(
                origin,
                Vector3.up,
                _ceilingHits,
                CEILING_CHECK_HEIGHT,
                LevelSingleton.m_LayerWorld,
                QueryTriggerInteraction.Ignore
            );

            if (ceilingHitCount <= 0)
            {
                ceilingHitCount = Physics.RaycastNonAlloc(
                    m_PlayerController.m_CameraBase.position,
                    Vector3.up,
                    _ceilingHits,
                    CEILING_CHECK_HEIGHT,
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
            bool isClimbing = m_PlayerController.Climbing;

            if (!previouslyClimbing && isClimbing)
            {
                previouslyClimbing = true;
            }
            else if (previouslyClimbing && m_PlayerController.Grounded)
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
            if (Time.time - m_PlayerController.CrouchStateChangeTimeStamp <= SLIDE_JUMP_MIN_TIME)
            {
                return;
            }

            var rigidBody = m_PlayerController.RigidBody;
            Vector3 velocity = m_PlayerController.m_CameraBase.forward * SLIDE_JUMP_FORWARD_SPEED;
            velocity.y = rigidBody.velocity.y;
            rigidBody.velocity = velocity;

            m_PlayerController.PlayerRigAnimator.SetJumpCharge(SLIDE_JUMP_CHARGE);

            if (m_PlayerController.IsCameraAttached)
            {
                Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.JUMP_SLIDEJUMP, m_PlayerController.gameObject)
                    .SetAudioParameter("velocity_horizontal", m_PlayerController.FlatSpeed)
                    .SetAudioParameter("3d", 0)
                    .StartAndRelease();
            }
        }

        private void PerformNormalJump()
        {
            var rigidBody = m_PlayerController.RigidBody;
            Vector3 velocity = rigidBody.velocity;
            velocity.y = 0f;

            float flatSpeedMultiplier = GetJumpSpeedMultiplier();
            float targetSpeed = Mathf.Max(
                m_PlayerController.FlatSpeed * flatSpeedMultiplier,
                m_PlayerController.CurrentTargetSpeed
            );

            velocity = velocity.normalized * targetSpeed;
            velocity.y = rigidBody.velocity.y;
            rigidBody.velocity = velocity;
        }

        private float GetJumpSpeedMultiplier()
        {
            // Don't lose speed while jumping if player has advanced abilities
            return (_hasBoostJump || _hasVaultJump) ? 1f : NORMAL_JUMP_SPEED_MULT;
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
            var rigidBody = m_PlayerController.RigidBody;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(
                rigidBody.velocity,
                m_PlayerController.Grounder.GroundContactNormal
            );

            rigidBody.velocity = projectedVelocity.normalized * m_PlayerController.CurrentTargetSpeed;
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
            return (bool)_isJumpPressedThisFrameMethod.Invoke(m_PlayerController, _emptyArgs);
        }

        private void DropAllHeldItems()
        {
            ItemGrabber itemGrabber = (ItemGrabber)_itemGrabberField.GetValue(m_PlayerController);
            _dropAllItemMethod.Invoke(itemGrabber, _emptyArgs);
        }
        #endregion
    }
}