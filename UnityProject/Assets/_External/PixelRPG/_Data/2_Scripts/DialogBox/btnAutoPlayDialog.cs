using UnityEngine;
using UnityEngine.UI;

public class btnAutoPlayDialog : MonoBehaviour
{
    Button thisButton;
    DialogBox dialogBox;
    public Color autoPlayColor;
    public Color normalColor;

    void Start()
    { 
        dialogBox = FindFirstObjectByType<DialogBox>();
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(AutoPlayDialog);
    }

    void AutoPlayDialog()
    {
        dialogBox.autoPlayMode = !dialogBox.autoPlayMode;
        if (dialogBox.autoPlayMode)
        {
            thisButton.GetComponent<Image>().color = autoPlayColor;
        }
        else
        {
            thisButton.GetComponent<Image>().color = normalColor;
        }
    }
}
