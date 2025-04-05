using UnityEngine;
using UnityEngine.UI;

public class btnOpenDialog : MonoBehaviour
{
    Button thisButton;
    public DialogCtrl dialogCtrl;

    void Start()
    {
        dialogCtrl = FindFirstObjectByType<DialogCtrl>();
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OpenDialog);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenDialog();
        }
    }

    void OpenDialog()
    {
        PlayerInteraction playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            NPC npcInteract = playerInteraction.GetNPCInteract();
            if (npcInteract != null)
            {
                dialogCtrl.OpenDialogBoxNPC(npcInteract.dialogContent);
            }
        }
    }
}