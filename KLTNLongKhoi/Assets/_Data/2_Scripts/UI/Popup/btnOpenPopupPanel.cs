using UnityEngine;

public class btnOpenPopupPanel : MonoBehaviour
{
    public bool isOpenPanel;
    [SerializeField] Transform objectActive;
    [SerializeField] PopupScale popupScale;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>()?.onClick.AddListener(OpenPopupPanel);
    }

    public void OpenPopupPanel()
    {
        if (isOpenPanel)
        {
            if (objectActive != null)
            {
                objectActive.gameObject.SetActive(true);
            }
            popupScale.ScaleUp();
        }
        else
        {
            if (objectActive != null)
            {
                objectActive.gameObject.SetActive(false);
            }
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
