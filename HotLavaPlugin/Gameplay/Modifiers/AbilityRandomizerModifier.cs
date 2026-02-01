using Klei.HotLava.Character;
using Klei.HotLava.Character.Modifiers;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Gameplay.Modifiers
{
    /// <summary>
    /// Ability Randomizer modifier for Archipelago ability randomization. Combines all 4 special abilities in 1 class
    /// </summary>
    public class AbilityRandomizerModifier : PlayerControllerModifier
    {
        #region Static Abilities
        public static DoubleJumpModifier DoubleJumpModifier { get; set; }
        public static SlideJumpModifier SlideJumpModifier { get; set; }
        public static LungeModifier LungeModifier { get; set; }
        #endregion

        #region Ability Toggles
        [Header("Ability Toggles")]
        [Tooltip("Enable/disable double jump ability")]
        public bool EnableDoubleJump = true;

        [Tooltip("Enable/disable boost jump ability")]
        public bool EnableBoostJump = true;

        [Tooltip("Enable/disable slide jump ability")]
        public bool EnableSlideJump = true;

        [Tooltip("Enable/disable lunge/vault ability")]
        public bool EnableVaultJump = true;
        #endregion

        #region Modifier Instances
        private DoubleJumpModifier m_DoubleJumpModifier;
        private SlideJumpModifier m_SlideJumpModifier;
        private LungeModifier m_LungeModifier;
        #endregion

        #region Ability Properties
        public override bool CanBhop => EnableBoostJump;

        public override bool CanPerfectJump
        {
            get
            {
                bool result = base.CanPerfectJump;

                if (EnableDoubleJump && m_DoubleJumpModifier != null)
                    result |= m_DoubleJumpModifier.CanPerfectJump;

                if (EnableSlideJump && m_SlideJumpModifier != null)
                    result |= m_SlideJumpModifier.CanPerfectJump;

                if (EnableVaultJump && m_LungeModifier != null)
                    result |= m_LungeModifier.CanPerfectJump;

                return result;
            }
        }

        public override bool CanGrab
        {
            get
            {
                bool canGrab = true;

                if (EnableVaultJump && m_LungeModifier != null)
                    canGrab &= m_LungeModifier.CanGrab;

                return canGrab;
            }
        }

        public override bool CanCrouch => true;
        public override bool CanSurf => true;
        public override bool CanWallRun => true;

        public override bool CanReach
        {
            get
            {
                if (EnableVaultJump && m_LungeModifier != null)
                    return m_LungeModifier.CanReach;

                return base.CanReach;
            }
        }

        public override float GravityMultiplier
        {
            get
            {
                if (EnableDoubleJump && m_DoubleJumpModifier != null)
                    return m_DoubleJumpModifier.GravityMultiplier;

                return base.GravityMultiplier;
            }
        }
        #endregion

        #region Initialization
        public void Awake()
        {
            m_DoubleJumpModifier = (DoubleJumpModifier)PlayerControllerModifier.Clone(DoubleJumpModifier);
            m_SlideJumpModifier = (SlideJumpModifier)PlayerControllerModifier.Clone(SlideJumpModifier);
            m_LungeModifier = (LungeModifier)PlayerControllerModifier.Clone(LungeModifier);

            //Don't lose speed from double jumping
            m_DoubleJumpModifier.m_DoubleJumpSpeedMultiplier = 1f;
        }

        public override void AddModifier(PlayerController player)
        {
            m_DoubleJumpModifier.AddModifier(player);
            m_SlideJumpModifier.AddModifier(player);
            m_LungeModifier.AddModifier(player);

            base.AddModifier(player);
        }

        public override void RemoveModifier()
        {
            base.RemoveModifier();

            m_DoubleJumpModifier.RemoveModifier();
            m_SlideJumpModifier.RemoveModifier();
            m_LungeModifier.RemoveModifier();
        }
        #endregion

        #region Unity Lifecycle
        public override void Update()
        {
            if (!m_PlayerController.IsMine) return;

            if (EnableDoubleJump && m_DoubleJumpModifier != null)
            {
                m_DoubleJumpModifier.Update();
            }

            if (EnableVaultJump && m_LungeModifier != null)
            {
                m_LungeModifier.Update();
            }
        }

        public override void FixedUpdate()
        {
            if (!m_PlayerController.IsMine) return;

            if (EnableSlideJump && m_SlideJumpModifier != null)
            {
                m_SlideJumpModifier.FixedUpdate();
            }

            if (EnableVaultJump && m_LungeModifier != null)
            {
                m_LungeModifier.FixedUpdate();
            }
        }

        public override void OnVictory()
        {
            if (EnableVaultJump && m_LungeModifier != null)
            {
                m_LungeModifier.OnVictory();
            }
        }
        #endregion
    }
}