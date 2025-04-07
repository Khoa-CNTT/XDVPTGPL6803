using UnityEngine;

public class btnOpenPopupPanel : MonoBehaviour
{
    public bool isOpenPanel;
    [SerializeField] PopupScale popupScale;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>()?.onClick.AddListener(OpenPopupPanel);
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

    public void OpenPopupPanel(bool isOpen)
    {
        if (isOpen)
        {
            popupScale.ScaleUp();
        }
        else
        {
            popupScale.ScaleDown();
        }
    }
}
