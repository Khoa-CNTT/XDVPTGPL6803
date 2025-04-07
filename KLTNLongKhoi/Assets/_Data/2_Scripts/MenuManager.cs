using StarterAssets;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] btnOpenPopupPanel btnOpenPopupPanelSetting;
        PauseManager pauseManager;
        GameManager gameManager;
        StarterAssetsInputs inputs;
        bool isShowSettingPanel;

        private void Start()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            gameManager = FindFirstObjectByType<GameManager>();
            inputs = FindFirstObjectByType<StarterAssetsInputs>();
        }

        public void OpenPopupPanelSetting(bool value)
        {
            btnOpenPopupPanelSetting.OpenPopupPanel(value);
        }

        void Update()
        {
            if (inputs.escape != isShowSettingPanel && !gameManager.IsGameOver)
            {
                isShowSettingPanel = inputs.escape;
                pauseManager.TogglePause(isShowSettingPanel);
                OpenPopupPanelSetting(isShowSettingPanel);
            }
        }
    }
}