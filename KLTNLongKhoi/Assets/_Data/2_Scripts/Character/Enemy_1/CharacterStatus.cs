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
        private bool isDead = false;

        private void Awake()
        {
            ragdollAnimator = GetComponent<RagdollAnimator>();
            ccBePushedBack = GetComponent<CCBePushedBack>();
            healthBar = GetComponentInChildren<MicroBar>();
            barHpCtrl = GetComponentInChildren<BarHpCtrl>();
            currentHealth = maxHealth;
        }

        private void Start()
        {
            currentHealth = maxHealth;
            healthBar.Initialize(maxHealth);
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

            ragdollAnimator?.EnableRagdoll();
        }

        // Public getters for stats
        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
        public bool IsDead() => isDead;
        public int GetStrength() => baseStrength;


    }
}