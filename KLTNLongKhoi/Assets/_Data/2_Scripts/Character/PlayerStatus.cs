using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;
using System.Collections;
using System.Linq;

namespace KLTNLongKhoi
{
    public class PlayerStatus : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        [Header("UI References")]
        [SerializeField] private MicroBar healthBar;

        [Header("Stats from GameManager")]
        private int baseHP;
        private int baseMoney;
        private int baseStrength;
        private int baseCharm;
        private int baseIntelligence;

        [Header("Respawn Settings")]
        [SerializeField] private float respawnDelay = 3f;
        private Vector3 lastCheckpoint;

        private ThirdPersonController playerController;
        private RagdollAnimator ragdollAnimator;
        private CCBePushedBack ccBePushedBack;
        private bool isDead = false;
        private GameManager gameManager;
        private PauseManager pauseManager;

        private void Awake()
        {
            playerController = GetComponent<ThirdPersonController>();
            ragdollAnimator = GetComponent<RagdollAnimator>();
            ccBePushedBack = GetComponent<CCBePushedBack>();
            currentHealth = maxHealth;
            gameManager = FindFirstObjectByType<GameManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        private void Start()
        {
            healthBar.Initialize(maxHealth);
            LoadStatsFromGameManager();
            // Set initial checkpoint as spawn position
            lastCheckpoint = transform.position;
        }

        private void LoadStatsFromGameManager()
        {
            if (GameManagerPlayerStats.Instance != null)
            {
                baseHP = GameManagerPlayerStats.Instance.HP;
                baseMoney = GameManagerPlayerStats.Instance.Money;
                baseStrength = GameManagerPlayerStats.Instance.Strength;
                baseCharm = GameManagerPlayerStats.Instance.Charm;
                baseIntelligence = GameManagerPlayerStats.Instance.Intelligence;

                // Update max health based on base HP stat
                maxHealth = baseHP * 10f; // Example: Each HP point gives 10 health
                currentHealth = maxHealth;

                if (healthBar != null)
                {
                    healthBar.SetNewMaxHP(maxHealth);
                }
            }
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            if (isDead) return;

            // Apply damage reduction based on strength stat
            float damageReduction = baseStrength * 0.05f; // Example: Each strength point reduces damage by 5%
            float finalDamage = Mathf.Max(0, damage * (1 - damageReduction));

            currentHealth -= finalDamage;

            // Update health bar
            if (healthBar != null)
            {
                healthBar.UpdateBar(currentHealth);
            }

            // Check for death
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die(hitDirection);
            }
            else
            {
                // Trigger hurt animation through ThirdPersonController
                playerController?.TakeHit(hitDirection, finalDamage);
                ccBePushedBack?.PushBack(hitDirection, finalDamage);
            }
        }

        public void Heal(float amount)
        {
            if (isDead) return;

            float newHealth = Mathf.Min(currentHealth + amount, maxHealth);
            currentHealth = newHealth;

            // Update health bar
            if (healthBar != null)
            {
                healthBar.UpdateBar(currentHealth, UpdateAnim.Heal);
            }
        }

        private void Die(Vector3 hitDirection)
        {
            if (isDead) return;
            isDead = true;

            // Trigger death animation and ragdoll
            playerController?.Die(hitDirection);
            ragdollAnimator?.EnableRagdoll();

            // Notify GameManager
            gameManager.GameOver();

            // Start respawn sequence
            StartCoroutine(RespawnSequence());
        }

        private IEnumerator RespawnSequence()
        {
            // Wait for delay
            yield return new WaitForSeconds(respawnDelay);

            // Reset position to last checkpoint
            transform.position = lastCheckpoint;
            
            // Reset health and state
            isDead = false;
            currentHealth = maxHealth;
            healthBar?.UpdateBar(currentHealth);
            
            // Re-enable components
            if (playerController != null)
            {
                var controller = playerController.GetComponent<CharacterController>();
                if (controller != null) controller.enabled = true;
            }

            // Resume game if paused
            gameManager?.RestartGame();
            
            // Disable ragdoll
            ragdollAnimator?.DisableRagdoll();
        }

        // Public getters for stats
        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
        public bool IsDead() => isDead;
        public int GetStrength() => baseStrength;
        public int GetCharm() => baseCharm;
        public int GetIntelligence() => baseIntelligence;
        public int GetMoney() => baseMoney;

        // Method to modify stats
        public void AddMoney(int amount)
        {
            baseMoney += amount;
            if (GameManagerPlayerStats.Instance != null)
            {
                GameManagerPlayerStats.Instance.Money = baseMoney;
            }
        }

        // Call this when player reaches a checkpoint
        public void SetCheckpoint(Vector3 position)
        {
            lastCheckpoint = position;
        }
    }
}
