using StarterAssets;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] PopupScale panelSetting;
        PauseManager pauseManager;
        GameManager gameManager;
        StarterAssetsInputs inputs;
        bool isShowSettingPanel;

        private void Start()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            gameManager = FindFirstObjectByType<GameManager>();
            inputs = FindFirstObjectByType<StarterAssetsInputs>();

            inputs.Escape.AddListener(OpenPopupPanelSetting);
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
                panelSetting.ScaleUp();
            }
            else
            {
                panelSetting.ScaleDown();
            }

            if (gameManager.IsGameOver == false)
            {
                pauseManager.TogglePause(isShowSettingPanel);
            }
        }
    }
}