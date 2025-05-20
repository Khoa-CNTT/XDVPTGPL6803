using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    public class UISkill : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button UpgradeButton;
        [SerializeField] private Button btnClosePanel;
        [SerializeField] private Image skillIcon;
        [SerializeField] private TMP_Text skillNameText;
        [SerializeField] private TMP_Text skillDescriptionText;
        [SerializeField] private TMP_Text notifyUpgrade;
        SkillButton skillButton;
        UpgradeSkill upgradeSkill;

        private void Awake()
        { 
            upgradeSkill = FindFirstObjectByType<UpgradeSkill>();
        }

        private void Start()
        {
            btnClosePanel.onClick.AddListener(CloseSkillPanel);
            UpgradeButton.onClick.AddListener(OnClickUpgradeButton);
        }

        private void HideNotify()
        {
            notifyUpgrade.gameObject.SetActive(false);
        }

        public void UpgradeSuccess(string message)
        {
            notifyUpgrade.gameObject.SetActive(true);
            notifyUpgrade.text = message;
            notifyUpgrade.color = Color.green;
            Invoke("HideNotify", 2f);
        }

        public void UpgradeFail(string message)
        {
            notifyUpgrade.gameObject.SetActive(true);
            notifyUpgrade.text = message;
            notifyUpgrade.color = Color.red;
            Invoke("HideNotify", 2f);
        }

        public void CloseSkillPanel()
        {
            upgradeSkill.CloseStorage();
        }

        public void ShowSkillInfo(SkillButton skillButton)
        {   
            this.skillButton = skillButton; 
            skillNameText.text = skillButton.SkillName;
            skillDescriptionText.text = skillButton.SkillDescription;
            skillIcon.sprite = skillButton.SkillIcon;
        }

        public void OnClickUpgradeButton()
        {
            skillButton.OnClickUpgradeButton();
        }
    }
}