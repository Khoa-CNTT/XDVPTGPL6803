using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class btnDialogOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpDialogOption;
    DialogCtrl dialogCtrl;
    DialogContent dialogContent;

    private void Start()
    {
        dialogCtrl = FindFirstObjectByType<DialogCtrl>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void SetBtnOption(DialogOption dialogOption)
    {
        tmpDialogOption.text = dialogOption.optionText;
        dialogContent = dialogOption.dialogContent;
    } 

    public void OnClick()
    {
        print("DialogOption Clicked");
        dialogCtrl.OnButtonLoadDialogBox(dialogContent);
    }
}
