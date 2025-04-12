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

            inputs.Escape.AddListener(() => OpenPopupPanelSetting(true));
        }

        public void OpenPopupPanelSetting(bool value)
        {
            btnOpenPopupPanelSetting.OpenPopupPanel(value);

            if (isShowSettingPanel && !gameManager.IsGameOver)
            {
                pauseManager.TogglePause(isShowSettingPanel);
            }
        }

    
    }
}