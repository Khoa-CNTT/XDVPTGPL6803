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
            playerStatsManager.StatsUpdatedEvent.AddListener(UpdateAllStats);
        }

        private void Start()
        {
            healthBar.Initialize(playerStatsManager.BaseHP);
            staminaBar.Initialize(playerStatsManager.BaseSP);
            manaBar.Initialize(playerStatsManager.BaseMP);
        }

        public void UpdateAllStats()
        {  
            

            healthBar.UpdateBar(playerStatsManager.CurrentHP);
            staminaBar.UpdateBar(playerStatsManager.CurrentSP);
            manaBar.UpdateBar(playerStatsManager.CurrentMP);
        }
    }
}
