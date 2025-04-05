using System.Collections.Generic;

[System.Serializable]
public class DialogLine
{
    public ActorStats actorStats;
    public string dialogText;
    public bool isEndDialog;    // có muốn ngắt cuộc hội thoại sau khi chọn option này không
    public List<DialogOption> dialogOptions; // Danh sách các option để chọn

    public bool IsDialogOption()
    {
        return dialogOptions != null && dialogOptions.Count > 0;
    }
}