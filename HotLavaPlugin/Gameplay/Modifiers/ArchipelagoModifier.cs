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

        //public void Awake()
        //{
        //    LungeModifier lunge = Object.Instantiate<LungeModifier>();
        //}

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

        private void DoubleJumpUpdate()
        {
            bool canDoubleJump = PlayerCanPerformAirborneAbility();

            if (_hasBoostJump)
            {
                //Reduce the feeling of double jumping when you think you should boost
                canDoubleJump = canDoubleJump && m_PlayerController.DistanceToGround > .75f;
            }

            m_PlayerController.PlayerRigAnimator.SetDoubleJump(to: false);

            if (canDoubleJump)
            {
                MethodInfo isJumpPressedThisFrame = typeof(PlayerController).GetMethod("IsJumpPressedThisFrame", BindingFlags.NonPublic | BindingFlags.Instance);

                if (!m_DidDoubleJump && (bool)isJumpPressedThisFrame.Invoke(m_PlayerController, [])
                    && (!(m_PlayerController.Grounder.DistanceToGround < 1.8f) || !(m_PlayerController.Velocity.y < -1.5f)
                        || (!(m_PlayerController.Grounder.DistanceToGround < 0.3f) && !m_NoDoubleJumpMaterials.Contains(m_PlayerController.Grounder.NearGroundMaterial))))
                {
                    m_PlayerController.PlayerRigAnimator.SetDoubleJump(to: true);
                    m_PlayerController.PlayerAudioComponent.PlayFootstep(eFootstepAction.JUMP);
                    float maxJumpModifier = m_MaxJumpModifier;
                    m_MaxJumpModifier = m_DoubleJumpHeightMultiplier;
                    float value = m_PlayerController.JumpFlatSpeed * m_DoubleJumpSpeedMultiplier;
                    value = Mathf.Clamp(value, 0f, m_DoubleJumpMaxSpeedBoost);
                    m_PlayerController.Jumper.Jump(m_PlayerController.GetCachedInput().normalized, m_PlayerController.RigidBody.velocity.normalized, value);
                    m_MaxJumpModifier = maxJumpModifier;
                    m_DidDoubleJump = true;
                }
            }
            else
            {
                m_DidDoubleJump = false;
            }
        }

        public void VaultJumpUpdate()
        {
            bool m_InFanTunnel = (bool)typeof(PlayerController).GetField("m_InFanTunnel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(m_PlayerController);
            CanBounce canBounce = (CanBounce)typeof(PlayerController).GetField("CanBounce", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(m_PlayerController);

            m_ReadyState = PlayerCanPerformAirborneAbility() && !m_PlayerController.Victory && !m_PlayerController.HasPendingDeath;
            m_ActionPressed = m_ActionPressed || m_PlayerController.GetActionJustPressed();
        }

        /// <summary>
        /// Checks if the player is currently airborne, not on a slime/wall run, not in a fan tunnel, not climbing, not hitting a trampoline/balloon, and didn't jump too recently
        /// </summary>
        /// <returns></returns>
        private bool PlayerCanPerformAirborneAbility()
        {
            bool m_InFanTunnel = (bool)typeof(PlayerController).GetField("m_InFanTunnel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(m_PlayerController);
            CanBounce canBounce = (CanBounce)typeof(PlayerController).GetField("CanBounce", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(m_PlayerController);

            return !m_PlayerController.Grounded && !m_PlayerController.WallRun && !m_InFanTunnel
                && !m_PlayerController.Climbing && !canBounce.JustBounced && !m_PlayerController.InJumpGracePeriod;
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

        private void VaultJumpFixedUpdate()
        {
            if (m_ReadyState)
            {
                //Detect if camera is looking up or down
                float cameraDot = Vector3.Dot(m_PlayerController.CameraForward, Vector3.up);
                //Gap between player and ledge
                float detect_distance = 0f;
                float climb_height = 0f;
                bool canClamber = PerformClimbCheck(cameraDot, out detect_distance, out climb_height);

                bool movingForward = m_PlayerController.GetCachedInput().y > 0.7f;
                bool forwardVelocityBelowMin = m_PlayerController.Velocity.y < m_ClimbDetectMinVelocity;
                bool allowClamber = m_AlwaysAllowClamber ? (forwardVelocityBelowMin && movingForward) : !m_CanLunge;

                if (!m_IsClambering && allowClamber && canClamber)
                {
                    m_IsClambering = true;
                    m_OriginalVelocity = m_PlayerController.RigidBody.velocity;

                    ItemGrabber m_ItemGrabber = (ItemGrabber)typeof(PlayerController).GetField("m_ItemGrabber", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(m_PlayerController);
                    typeof(ItemGrabber).GetMethod("DropAllItem", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(m_ItemGrabber, []);

                    Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.HANDGRAB_ON_VAULT, m_PlayerController.gameObject).StartAndRelease();
                    //If player is farther from the ledge, make climb animation shorter
                    float distanceWeight = (detect_distance - 0.2f) * 0.4f;
                    float climbTimeModifier = Mathf.Clamp01(cameraDot) * 0.24f - distanceWeight;
                    m_ClamberDuration = m_ClamberVelocityCurve.keys[m_ClamberVelocityCurve.length - 1].time + climbTimeModifier;
                    m_PlayerController.PlayerRigAnimator.SetEdgeClimbingHeight(climb_height);
                }

                if (m_IsClambering)
                {
                    m_ClamberTimer += Time.fixedDeltaTime;
                    if (m_ClamberTimer < m_ClamberDuration)
                    {
                        Vector3 vector = Vector3.Slerp(m_PlayerController.m_CameraBase.forward, Vector3.up, 0.7f);
                        m_PlayerController.RigidBody.velocity = vector * m_ClamberVelocityCurve.Evaluate(m_ClamberTimer);
                    }
                    else
                    {
                        m_OriginalVelocity.y *= 0.3f;
                        m_OriginalVelocity.x = Mathf.Min(m_OriginalVelocity.x, m_PlayerController.ForwardSpeed);
                        m_OriginalVelocity.z = Mathf.Min(m_OriginalVelocity.z, m_PlayerController.ForwardSpeed);
                        m_PlayerController.RigidBody.velocity = m_OriginalVelocity;
                        Runtime.PlayOneShotAttached(Klei.HotLava.Audio.Event.HANDGRAB_OFF_VALUT, m_PlayerController.gameObject).StartAndRelease();
                        m_ClamberTimer = 0f;
                        m_IsClambering = false;
                    }
                }

                if (m_ActionPressed && !m_IsLunging && m_CanLunge)
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

                if (m_IsLunging)
                {
                    m_LungeTimer += Time.fixedDeltaTime;
                    if (m_LungeTimer < m_LungeVelociyCurve.keys[m_LungeVelociyCurve.length - 1].time)
                    {
                        m_PlayerController.RigidBody.velocity = m_OriginalVelocity + m_LungeDirection * m_LungeVelociyCurve.Evaluate(m_LungeTimer);
                    }
                    else
                    {
                        m_PlayerController.RigidBody.velocity = (m_AlwaysAllowClamber ? m_OriginalVelocity : Vector3.zero);
                        m_IsLunging = false;
                        m_LungeTimer = 0f;
                    }
                }
            }
            else
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

            m_ActionPressed = false;
            m_PlayerController.PlayerRigAnimator.SetIsEdgeClimbing(m_IsClambering);
            m_PlayerController.PlayerRigAnimator.SetIsLunging(m_IsLunging);
        }

        public override void OnVictory()
        {
            m_PlayerController.PlayerRigAnimator.SetIsEdgeClimbing(to: false);
            m_PlayerController.PlayerRigAnimator.SetIsLunging(to: false);
        }

        /// <summary>
        /// Checks for any ledges within reach of the player for them to climb
        /// </summary>
        /// <param name="camera_dot"></param>
        /// <param name="detect_distance"></param>
        /// <param name="climb_height"></param>
        /// <returns></returns>
        private bool PerformClimbCheck(float camera_dot, out float detect_distance, out float climb_height)
        {
            RaycastHit[] array = new RaycastHit[2];
            int hitCount = 0;
            detect_distance = 0f;
            climb_height = 0f;

            //Distances to check from the player for ledges
            float farOffset = m_PlayerController.ColliderRadius + 0.16f * m_PlayerController.Scale;
            float midOffset = m_PlayerController.ColliderRadius + 0.08f * m_PlayerController.Scale;
            float nearOffset = m_PlayerController.ColliderRadius + 0.02f * m_PlayerController.Scale;

            //Get horizontal direction of the camera
            Vector3 checkDirection = (camera_dot > 0f) ? m_PlayerController.CameraForward : m_PlayerController.m_CameraBase.forward;

            //Points to start from to look for climb spots
            Vector3[] raycastOrigins =
            [
                m_PlayerController.m_CameraBase.position + checkDirection * farOffset,
                m_PlayerController.m_CameraBase.position + checkDirection * midOffset,
                m_PlayerController.m_CameraBase.position + checkDirection * nearOffset
            ];

            //Larger range if falling, shorter range if looking straight up
            float scanRange = ((m_PlayerController.Velocity.y > -0.8f) ? m_ClimbDetectHeightRising : m_ClimbDetectHeightFalling);
            if (camera_dot > 0.6f)
            {
                scanRange *= 0.9f;
            }

            for (int i = 0; i < raycastOrigins.Length; i++)
            {
                //Search down from the origin point to find any climbale spots
                hitCount = UnityEngine.Physics.RaycastNonAlloc(raycastOrigins[i], Vector3.down, array, scanRange, LevelSingleton.m_LayerWorld, QueryTriggerInteraction.Ignore);
                if (hitCount <= 0)
                {
                    continue;
                }

                //Check for any ceiling above the points to make sure the player's head doesn't hit the ceiling after climbing
                RaycastHit[] ceilingHits = new RaycastHit[1];
                int ceilingHitCount = UnityEngine.Physics.RaycastNonAlloc(raycastOrigins[i], Vector3.up, ceilingHits, 1.5f, LevelSingleton.m_LayerWorld, QueryTriggerInteraction.Ignore);
                if (ceilingHitCount <= 0)
                {
                    ceilingHitCount = UnityEngine.Physics.RaycastNonAlloc(m_PlayerController.m_CameraBase.position, Vector3.up, ceilingHits, 1.5f, LevelSingleton.m_LayerWorld, QueryTriggerInteraction.Ignore);
                }

                if (ceilingHitCount > 0)
                {
                    //Player will hit the ceiling if they attempt to climb here, return false indicating climbing is not possible
                    return false;
                }

                for (int j = 0; j < hitCount; j++)
                {
                    //Make sure surface is flat enough to be climbed on
                    if (Vector3.Dot(array[j].normal, Vector3.up) > 0.6f)
                    {
                        climb_height = array[j].point.y - m_PlayerController.BasePosition.y;
                        detect_distance = array[j].distance;
                        return true;
                    }
                }
            }

            return false;
        }

        private void SlideJumpFixedUpdate()
        {
            if (m_PlayerController.ExternalJump || m_PlayerController.Surfing)
            {
                return;
            }

            if (!previouslyClimbing && m_PlayerController.Climbing)
            {
                previouslyClimbing = true;
            }

            if (previouslyClimbing && m_PlayerController.Grounded)
            {
                previouslyClimbing = false;
            }

            if (m_PlayerController.JustJumped && !m_PlayerController.PreviouslySurfing && !previouslyClimbing)
            {
                if (lastFrameSlide)
                {
                    if (Time.time - m_PlayerController.CrouchStateChangeTimeStamp > 0.15f)
                    {
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
                }
                else
                {
                    Vector3 velocity2 = m_PlayerController.RigidBody.velocity;
                    velocity2.y = 0f;
                    float flatSpeedMultiplier;
                    if (_hasDoubleJump || _hasBoostJump || _hasVaultJump)
                    {
                        //Don't lose speed while jumping
                        flatSpeedMultiplier = 1f;
                    }
                    else
                    {
                        flatSpeedMultiplier = 0.85f;
                    }
                    velocity2 = velocity2.normalized * Mathf.Max(m_PlayerController.FlatSpeed * flatSpeedMultiplier, m_PlayerController.CurrentTargetSpeed);

                    velocity2.y = m_PlayerController.RigidBody.velocity.y;
                    m_PlayerController.RigidBody.velocity = velocity2;
                }
            }
            else
            {
                lastFrameSlide = m_PlayerController.IsSliding;
                if (!m_PlayerController.CrouchJumped)
                {
                    m_PlayerController.PlayerRigAnimator.SetJumpCharge(0f);
                }

                if (m_PlayerController.Grounded && m_PlayerController.IsSliding)
                {
                    Vector3 vector = Vector3.ProjectOnPlane(m_PlayerController.RigidBody.velocity, m_PlayerController.Grounder.GroundContactNormal);
                    m_PlayerController.RigidBody.velocity = vector.normalized * m_PlayerController.CurrentTargetSpeed;
                }
            }
        }
    }
}
