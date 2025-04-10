using UnityEngine;

public class btnOpenPopupPanel : MonoBehaviour
{
    public bool isOpenPanel;
    public PopupScale popupScale;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenPopupPanel);
    }

    public void OpenPopupPanel()
    {
        if (isOpenPanel)
        {
            popupScale.ScaleUp();
        }
        else
        {
            popupScale.ScaleDown();
        }
    }
}
