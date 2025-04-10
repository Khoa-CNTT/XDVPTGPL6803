using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    public class UIPlayerStats : MonoBehaviour
    {
        [Header("Text Components")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private TextMeshProUGUI criticalText; // Đổi từ charmText
        [SerializeField] private TextMeshProUGUI intelligenceText;

        [Header("References")]
        [SerializeField] private PlayerStatus playerStatus;

        private void Start()
        {
            if (GameManagerPlayerStats.Instance != null)
            {
                UpdateStatsUIGameManagerPlayerStats();
            }

            if (playerStatus != null)
            {
                UpdateHealthUI();
            }
        }

        public void UpdateStatsUIGameManagerPlayerStats()
        {
            if (GameManagerPlayerStats.Instance == null) return;

            if (moneyText != null)
                moneyText.text = $"Money: {GameManagerPlayerStats.Instance.Money}";
            
            if (strengthText != null)
                strengthText.text = $"Strength: {GameManagerPlayerStats.Instance.Strength}";
            
            if (criticalText != null)
                criticalText.text = $"Critical: {GameManagerPlayerStats.Instance.Critical}%";
            
            if (intelligenceText != null)
                intelligenceText.text = $"Intelligence: {GameManagerPlayerStats.Instance.Intelligence}";
        }

        public void UpdateHealthUI()
        {
            if (playerStatus == null || healthText == null) return;

            float currentHealth = playerStatus.GetCurrentHealth();
            float maxHealth = playerStatus.GetMaxHealth();
            healthText.text = $"HP: {currentHealth:F0}/{maxHealth:F0}";
        }

        // Call this method when player stats change
        public void UpdateAllStats()
        {
            UpdateStatsUIGameManagerPlayerStats();
            UpdateHealthUI();
        }
    }
}
