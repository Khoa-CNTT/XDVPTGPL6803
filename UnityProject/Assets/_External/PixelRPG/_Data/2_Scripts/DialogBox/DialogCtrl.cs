using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogCtrl : MonoBehaviour
{
    [SerializeField] List<DialogCollection> dialogCollections;
    [SerializeField] DialogBox dialogBox;

    [Header("DialogOption")]
    [SerializeField] btnDialogOption btnDialogOptionPrefabs;
    [SerializeField] Transform dialogOptionParent;

    /// <summary> khi dialogBox chạy xong DialogLine đang lưu sẽ gọi </summary>
    public void OnDialogBoxClose()
    {
        if (dialogCollections.Count > 0)
        {
            dialogCollections.RemoveAt(dialogCollections.Count - 1);
            DialogBoxShowLines();
        }
    }

    public void OnShowNextLine(DialogLine dialogLine, int currentLineIndex)
    {
        SetCurrentLineIndex(currentLineIndex);
        CreateDialogButton(dialogLine);
    }

    private void RemoveDialogButton()
    {
        foreach (Transform child in dialogOptionParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateDialogButton(DialogLine dialogLine)
    {
        // tạo các button option
        if (dialogLine.IsDialogOption())
        {
            print("IsDialogOption");
            foreach (DialogOption btnOption in dialogLine.dialogOptions)
            {
                btnDialogOption btnDialogOption = Instantiate(btnDialogOptionPrefabs, dialogOptionParent);
                btnDialogOption.SetBtnOption(btnOption);
            }
        }
    }

    /// <summary> Set currentLineIndex cho dialogCollections cuối </summary>
    public void SetCurrentLineIndex(int index)
    {
        dialogCollections[dialogCollections.Count - 1].currentLineIndex = index;
    }

    /// <summary> Lấy currentLineIndex của dialogCollections cuối </summary>
    public int GetCurrentLineIndex()
    {
        return dialogCollections[dialogCollections.Count - 1].currentLineIndex;
    }

    /// <summary> thêm hội thoại vào dialogCollections </summary>
    public void AddDialogCollection(List<DialogLine> dialogLines)
    {
        DialogCollection dialogCollection = new DialogCollection();
        dialogCollection.dialogLines = dialogLines;
        this.dialogCollections.Add(dialogCollection);
    }

    /// <summary> chạy cuộc hội thoại cuối trong dialogCollections </summary>
    public void DialogBoxShowLines()
    {
        if (dialogCollections.Count == 0) return;

        List<DialogLine> dialogLines = dialogCollections[dialogCollections.Count - 1].dialogLines;
        dialogBox.SetDialogLines(dialogLines, GetCurrentLineIndex());
    }

    /// <summary> Phương thức để mở hộp thoại với dialogLines </summary>
    public void OpenDialogBoxNPC(DialogContent dialogContent)
    {
        if (dialogBox.gameObject.activeSelf) return;
        LoadDialogBoxContent(dialogContent);
    }

    public void LoadDialogBoxContent(DialogContent dialogContent)
    {
        RemoveDialogButton();
        AddDialogCollection(dialogContent.dialogLines);
        DialogBoxShowLines();
    }

    public void OnButtonLoadDialogBox(DialogContent dialogContent)
    {
        dialogCollections[dialogCollections.Count - 1].currentLineIndex++;
        LoadDialogBoxContent(dialogContent);
    }

    [Serializable]
    public class DialogCollection
    {
        public int currentLineIndex;
        public List<DialogLine> dialogLines;
    }
}
