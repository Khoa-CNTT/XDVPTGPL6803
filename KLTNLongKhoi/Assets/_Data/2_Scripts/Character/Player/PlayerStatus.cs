using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;
using System.Collections;

namespace KLTNLongKhoi
{
    public class PlayerStatus : MonoBehaviour, IDamageable
    {
        [Header("UI References")]
        [SerializeField] private MicroBar healthBar;

        [Header("Respawn Settings")]
        [SerializeField] private float respawnDelay = 3f;
        private Vector3 lastCheckpoint;

        private PlayerStatsManager statsManager;
        private ThirdPersonController playerController;
        private RagdollAnimator ragdollAnimator;
        private CCBePushedBack ccBePushedBack;
        private bool isDead = false;
        private GameManager gameManager;
        private PauseManager pauseManager;

        private float currentHealth;

        private void Awake()
        {
            statsManager = GetComponent<PlayerStatsManager>();
            playerController = GetComponent<ThirdPersonController>();
            ragdollAnimator = GetComponent<RagdollAnimator>();
            ccBePushedBack = GetComponent<CCBePushedBack>();
            gameManager = FindFirstObjectByType<GameManager>();
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        private void Start()
        {
            // Đăng ký sự kiện khi stats thay đổi
            if (statsManager != null)
            {
                statsManager.StatsUpdatedEvent += OnStatsManagerUpdated;
            }

            // Khởi tạo health bar và health
            currentHealth = statsManager.MaxHealth;
            if (healthBar != null)
            {
                healthBar.Initialize(statsManager.MaxHealth);
            }

            // Set initial checkpoint
            lastCheckpoint = transform.position;
        }

        private void OnStatsManagerUpdated()
        {
            // Cập nhật máu khi stats thay đổi
            if (healthBar != null)
            {
                healthBar.SetNewMaxHP(statsManager.MaxHealth);
            }
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            if (isDead) return;

            float finalDamage = statsManager.CalculateFinalDamage(damage);
            currentHealth -= finalDamage;

            if (healthBar != null)
            {
                healthBar.UpdateBar(currentHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die(hitDirection);
            }
            else
            {
                playerController?.TakeHit(hitDirection, finalDamage);
                ccBePushedBack?.PushBack(hitDirection, finalDamage);
            }
        }

        public void Heal(float amount)
        {
            if (isDead) return;

            float newHealth = Mathf.Min(currentHealth + amount, statsManager.MaxHealth);
            currentHealth = newHealth;

            if (healthBar != null)
            {
                healthBar.UpdateBar(currentHealth, UpdateAnim.Heal);
            }
        }

        private void Die(Vector3 hitDirection)
        {
            if (isDead) return;
            isDead = true;

            playerController?.Die(hitDirection);
            ragdollAnimator?.EnableRagdoll();
            gameManager.GameOver();
            StartCoroutine(RespawnSequence());
        }

        private IEnumerator RespawnSequence()
        {
            yield return new WaitForSeconds(respawnDelay);

            transform.position = lastCheckpoint;
            
            isDead = false;
            currentHealth = statsManager.MaxHealth;
            healthBar?.UpdateBar(currentHealth);
            
            if (playerController != null)
            {
                var controller = playerController.GetComponent<CharacterController>();
                if (controller != null) controller.enabled = true;
            }

            gameManager?.RestartGame();
            ragdollAnimator?.DisableRagdoll();
        }

        // Public getters
        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => statsManager.MaxHealth;
        public bool IsDead() => isDead;

        // Checkpoint system
        public void SetCheckpoint(Vector3 position)
        {
            lastCheckpoint = position;
        }

        private void OnDestroy()
        {
            if (statsManager != null)
            {
                statsManager.StatsUpdatedEvent -= OnStatsManagerUpdated;
            }
        }
    }
}
