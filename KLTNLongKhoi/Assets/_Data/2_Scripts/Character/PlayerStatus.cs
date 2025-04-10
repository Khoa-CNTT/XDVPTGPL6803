using UnityEngine;
using Microlight.MicroBar;
using StarterAssets;

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

    private ThirdPersonController playerController;
    private RagdollAnimator ragdollAnimator;
    private CCBePushedBack ccBePushedBack;
    private bool isDead = false;

    private void Awake()
    {
        playerController = GetComponent<ThirdPersonController>();
        ragdollAnimator = GetComponent<RagdollAnimator>();
        ccBePushedBack = GetComponent<CCBePushedBack>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        healthBar.Initialize(maxHealth);

        // Load stats from GameManager
        LoadStatsFromGameManager();
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

        // Trigger death animation and ragdoll through ThirdPersonController
        playerController?.Die(hitDirection);
        ragdollAnimator?.User_EnableFreeRagdoll();

        // You might want to trigger game over screen or respawn logic here
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
}
