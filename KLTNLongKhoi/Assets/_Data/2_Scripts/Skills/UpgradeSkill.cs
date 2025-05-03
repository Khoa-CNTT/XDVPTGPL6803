using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    public class UpgradeSkill : MonoBehaviour
    {
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text notifyUpgrade;
        [SerializeField] private KeyCode openUpgradePanelKey = KeyCode.K; // phím tắt mở bẳng nâng cấp
        private PauseManager pauseManager;
        private bool isOpen = false;

        private void Start()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        }

        private void Update()
        {
            if (pauseManager.IsPaused && isOpen == false)
            {
                return;
            }
            if (Input.GetKeyDown(openUpgradePanelKey))
            {
                OnClick();
            }
        }

        private void OnClickUpgradeButton()
        {
            // Kiểm tra điều kiện nâng cấp và thực hiện nâng cấp
            bool upgradeSuccess = true; // Thay thế bằng logic kiểm tra điều kiện nâng cấp thực tế

            if (upgradeSuccess)
            {
                UpgradeSuccess("Nâng cấp thành công!");
            }
            else
            {
                UpgradeFail("Không đủ điều kiện nâng cấp!");
            }
        }

        private void HideNotify()
        {
            notifyUpgrade.gameObject.SetActive(false);
        }

        private void UpgradeSuccess(string message)
        {
            notifyUpgrade.gameObject.SetActive(true);
            notifyUpgrade.text = message;
            notifyUpgrade.color = Color.green;
            Invoke("HideNotify", 2f);
        }

        private void UpgradeFail(string message)
        {
            notifyUpgrade.gameObject.SetActive(true);
            notifyUpgrade.text = message;
            notifyUpgrade.color = Color.red;
            Invoke("HideNotify", 2f);
        }

        public void OnClick()
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                OpenStorage();
            }
            else
            {
                CloseStorage();
            }
        }

        public void OpenStorage()
        {
            upgradePanel.SetActive(true);
            pauseManager.PauseGame();
            isOpen = true;
        }

        public void CloseStorage()
        {
            upgradePanel.SetActive(false);
            pauseManager.ResumeGame();
            isOpen = false;
        }
    }
}