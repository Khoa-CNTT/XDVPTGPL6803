using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;
using System.Collections;
using System;

namespace KLTNLongKhoi
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerStatus : MonoBehaviour, IDamageable
    {
        [Header("Sounds Effects")]
        [SerializeField] private AudioClip damageSound;
        [SerializeField] private AudioClip healSound;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip manaRestoreSound;
        [SerializeField] private AudioClip staminaRestoreSound;

        private PlayerStatsManager playerStatsManager;
        private ThirdPersonController playerController;
        private RagdollAnimator ragdollAnimator;
        private CCBePushedBack ccBePushedBack;
        private bool isDead = false;
        private PauseManager pauseManager;
        private ThirdPersonController thirdPersonController;
        private GameManager gameManager;
        private AudioSource audioSource;

        public bool IsDead() => isDead;

        private void Awake()
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            playerController = GetComponent<ThirdPersonController>();
            ragdollAnimator = GetComponent<RagdollAnimator>();
            ccBePushedBack = GetComponent<CCBePushedBack>();
            gameManager = FindFirstObjectByType<GameManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();

            
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; 
        }

        public void TakeDamage(float damage, Vector3 hitDirection, Transform attacker)
        {
            if (isDead) return;

            float finalDamage = damage;
            playerStatsManager.CurrentHP -= finalDamage;

            // Play damage sound
            if (damageSound != null)
            {
                audioSource.PlayOneShot(damageSound);
            }

            if (playerStatsManager.CurrentHP <= 0)
            {
                isDead = true;
                playerStatsManager.CurrentHP = 0;
                playerController.OnDead();
                ragdollAnimator.EnableRagdoll();

                if (deathSound != null)
                {
                    audioSource.PlayOneShot(deathSound);
                }
            }
            else
            {
                ccBePushedBack?.PushBack(hitDirection, finalDamage);
            }
        }

        public void RestoreHealth(float amount)
        {
            if (isDead) return;

            float newHealth = Mathf.Min(playerStatsManager.CurrentHP + amount, playerStatsManager.PlayerData.baseHP);
            playerStatsManager.CurrentHP = newHealth;

            if (healSound != null)
            {
                audioSource.PlayOneShot(healSound);
            }
        }

        public void RestoreMana(float amount)
        {
            if (isDead) return;

            float newMana = Mathf.Min(playerStatsManager.CurrentMP + amount, playerStatsManager.PlayerData.baseMP);
            playerStatsManager.CurrentMP = newMana;

            if (manaRestoreSound != null)
            {
                audioSource.PlayOneShot(manaRestoreSound);
            }
        }

        public void RestoreStamina(float amount)
        {
            if (isDead) return;

            float newStamina = Mathf.Min(playerStatsManager.CurrentSP + amount, playerStatsManager.PlayerData.baseSP);
            playerStatsManager.CurrentSP = newStamina;

            if (staminaRestoreSound != null)
            {
                audioSource.PlayOneShot(staminaRestoreSound);
            }
        }
    }
}
