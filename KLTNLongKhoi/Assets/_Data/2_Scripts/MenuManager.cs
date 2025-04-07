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
    }

    public void OpenPopupPanelSetting(bool value)
    {
        btnOpenPopupPanelSetting.OpenPopupPanel(value);
    }
}
