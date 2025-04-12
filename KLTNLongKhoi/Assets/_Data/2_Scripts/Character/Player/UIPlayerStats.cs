using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Microlight.MicroBar;

namespace KLTNLongKhoi
{
    public class UIPlayerStats : MonoBehaviour
    {
        [Header("Text Components")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI staminaText;
        [SerializeField] private TextMeshProUGUI manaText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private TextMeshProUGUI criticalText;
        [SerializeField] private TextMeshProUGUI intelligenceText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI experienceText;

        [SerializeField] private MicroBar healthBar;
        [SerializeField] private MicroBar manaBar;
        [SerializeField] private MicroBar staminaBar;

        PlayerStatsManager playerStatsManager;

        void Awake()
        {
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            playerStatsManager.StatsUpdatedEvent += UpdateAllStats;
        }

        private void Start()
        {
            healthBar.Initialize(playerStatsManager.MaxHealth);
            staminaBar.Initialize(playerStatsManager.MaxStamina);
            manaBar.Initialize(playerStatsManager.MaxMana);
        }

        public void UpdateAllStats()
        {  
            if (staminaText != null)
                staminaText.text = $"Stamina: {playerStatsManager.CurrentStamina:F0}/{playerStatsManager.MaxStamina:F0}";

            if (manaText != null)
                manaText.text = $"Mana: {playerStatsManager.CurrentMana:F0}/{playerStatsManager.MaxMana:F0}";

            if (moneyText != null)
                moneyText.text = $"Money: {playerStatsManager.BaseMoney}";

            if (strengthText != null)
                strengthText.text = $"Strength: {playerStatsManager.BaseStrength}";

            if (criticalText != null)
                criticalText.text = $"Critical: {playerStatsManager.BaseCritical}%";

            if (intelligenceText != null)
                intelligenceText.text = $"Intelligence: {playerStatsManager.BaseIntelligence}";

            if (healthText != null)
                healthText.text = $"HP: {playerStatsManager.CurrentHealth:F0}/{playerStatsManager.MaxHealth:F0}";

            if (levelText != null)
                levelText.text = $"Level: {playerStatsManager.Level}";

            if (experienceText != null)
                experienceText.text = $"Exp: {playerStatsManager.Experience}";

            healthBar.UpdateBar(playerStatsManager.CurrentHealth);
            staminaBar.UpdateBar(playerStatsManager.CurrentStamina);
            manaBar.UpdateBar(playerStatsManager.CurrentMana);
        }
    }
}
