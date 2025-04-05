using UnityEngine;
using UnityEngine.UI;

public class btnSkipDialog : MonoBehaviour
{
    Button thisButton;
    DialogBox dialogBox;

    void Start()
    { 
        dialogBox = FindFirstObjectByType<DialogBox>();
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(SkipDialog);
    }

    void SkipDialog()
    {
        dialogBox.CloseDialog();
    }
}
