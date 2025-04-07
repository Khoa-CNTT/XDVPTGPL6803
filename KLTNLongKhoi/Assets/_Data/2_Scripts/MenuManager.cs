using StarterAssets;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] btnOpenPopupPanel btnOpenPopupPanelSetting;
    PauseManager pauseManager;
    StarterAssetsInputs inputs;
    bool isShowSettingPanel;

    private void Start()
    {
        pauseManager = FindFirstObjectByType<PauseManager>(); 
        inputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    public void OpenPopupPanelSetting(bool value)
    {
        btnOpenPopupPanelSetting.OpenPopupPanel(value);
    }

    void Update()
    {
        if (inputs.escape != isShowSettingPanel)
        {
            isShowSettingPanel = inputs.escape;
            pauseManager.TogglePause(isShowSettingPanel);
            OpenPopupPanelSetting(isShowSettingPanel);
        }
    }
}
