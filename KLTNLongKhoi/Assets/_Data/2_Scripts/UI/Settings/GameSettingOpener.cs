using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class GameSettingOpener : MonoBehaviour
    {
        public OnTriggerThis onOpenSettingPanel;
        PauseManager pauseManager;
        GameManager gameManager;
        StarterAssetsInputs inputs;
        bool isShowSettingPanel;

        private void Start()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            gameManager = FindFirstObjectByType<GameManager>();
            inputs = FindFirstObjectByType<StarterAssetsInputs>();

            inputs.Escape += OpenPopupPanelSetting;
        }

        public void OpenPopupPanelSetting()
        {
            if (pauseManager.IsPaused && isShowSettingPanel == false)
            {
                return;
            }
            isShowSettingPanel = !isShowSettingPanel;

            if (isShowSettingPanel)
            {
                onOpenSettingPanel.ActiveObjects();
            }
            else
            {
                onOpenSettingPanel.UnActiveObjects();
            }

            if (gameManager.IsGameOver == false)
            {
                pauseManager.SetPause(isShowSettingPanel);
            }
        }

        public void OpenPopupPanelSetting(bool isOpen)
        {
            isShowSettingPanel = isOpen;
            if (isShowSettingPanel)
            {
                onOpenSettingPanel.ActiveObjects();
            }
            else
            {
                onOpenSettingPanel.UnActiveObjects();
            }

            if (gameManager.IsGameOver == false)
            {
                pauseManager.SetPause(isShowSettingPanel);
            }
        }
    }
}