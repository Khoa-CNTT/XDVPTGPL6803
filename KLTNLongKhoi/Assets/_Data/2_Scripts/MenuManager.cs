using StarterAssets;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] btnOpenPopupPanel btnOpenPopupPanelSetting;
    PauseManager pauseManager;

    private void Start()
    {
        pauseManager = FindFirstObjectByType<PauseManager>();
        pauseManager.onGamePaused.AddListener(OpenPopupPanelSetting);
        pauseManager.onGameResumed.AddListener(ClosePopupPanelSetting);
    }

    public void OpenPopupPanelSetting()
    {
        btnOpenPopupPanelSetting.OpenPopupPanel(true);
    }

    public void ClosePopupPanelSetting()
    {
        btnOpenPopupPanelSetting.OpenPopupPanel(false);
    }
}
