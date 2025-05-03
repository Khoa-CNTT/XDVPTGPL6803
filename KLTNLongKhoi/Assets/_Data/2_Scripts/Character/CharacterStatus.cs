using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;

namespace KLTNLongKhoi
{
    public class CharacterStatus : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        [Header("UI References")]
        [SerializeField] private MicroBar healthBar;
        [SerializeField] private BarHpCtrl barHpCtrl;

        [Header("Stats from GameManager")]
        private int baseHP;
        private int baseStrength;

        private RagdollAnimator ragdollAnimator;
        private CCBePushedBack ccBePushedBack;
        protected bool isDead = false;

        protected virtual void Awake()
        {
            ragdollAnimator = GetComponent<RagdollAnimator>();
            ccBePushedBack = GetComponent<CCBePushedBack>();
            healthBar = GetComponentInChildren<MicroBar>();
            barHpCtrl = GetComponentInChildren<BarHpCtrl>();
            currentHealth = maxHealth;
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            healthBar.Initialize(maxHealth);
        }

        public virtual void TakeDamage(float damage, Vector3 hitDirection, Transform attacker)
        {
            if (isDead) return;

            // Apply damage reduction based on strength stat
            float damageReduction = baseStrength * 0.05f; // Example: Each strength point reduces damage by 5%
            float finalDamage = Mathf.Max(0, damage * (1 - damageReduction));

            currentHealth -= finalDamage;

            // Update health bar
            if (healthBar != null)
            {
                barHpCtrl?.ShowHealthBar();
                healthBar.UpdateBar(currentHealth);
            }

            ccBePushedBack?.PushBack(hitDirection, finalDamage);

            // Check for death
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die(hitDirection);
            }
        }

        public void RestoreHealth(float amount)
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

        protected virtual void Die(Vector3 hitDirection)
        {
            if (isDead) return;
            HandleReward();
            isDead = true;

            ccBePushedBack.IsDead = true; // Không cho push back nữa khi chết
            ragdollAnimator?.EnableRagdoll();
        }

        protected virtual void HandleReward() { } // Override this method to handle reward logic for specific characters (e.g., enemies)

        // Public getters for stats
        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
        public bool IsDead() => isDead;
        public int GetStrength() => baseStrength;


    }
}