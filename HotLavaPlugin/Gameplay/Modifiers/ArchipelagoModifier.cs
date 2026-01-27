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

        /// <summary>
        /// TODO: Unlock via Archipelago Item
        /// </summary>
        public override bool CanBhop => true;
        /// <summary>
        /// TODO: should this be only if Double Jump is not unlocked?
        /// </summary>
        public override bool CanPerfectJump => true;

        public override bool CanGrab => true;
        public override bool CanCrouch => true;
        public override bool CanSurf => true;
        public override bool CanWallRun => true;
        public override bool CanReach => true;

        public override void Update()
        {
            if (!m_PlayerController.IsMine) return;

            bool m_InFanTunnel = (bool)typeof(PlayerController).GetField("m_InFanTunnel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(m_PlayerController);
            CanBounce canBounce = (CanBounce)typeof(PlayerController).GetField("CanBounce", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(m_PlayerController);

            bool num = !m_PlayerController.Grounded && !m_PlayerController.WallRun && !m_InFanTunnel && !m_PlayerController.Climbing && !canBounce.JustBounced && !m_PlayerController.InJumpGracePeriod;

            //TODO: maybe this will help?
            num = num && m_PlayerController.DistanceToGround > .75f;

            m_PlayerController.PlayerRigAnimator.SetDoubleJump(to: false);

            if (num)
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
    }
}
