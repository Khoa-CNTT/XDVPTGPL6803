using System;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class NPC : MonoBehaviour, IInteractable
    { 
        [SerializeField] private Transform cameraView;
        [SerializeField] private bool isInteractable = true;
        [SerializeField] private string interactionText = "Press E to interact";
        [SerializeField] private DialogContent dialogContent; // Reference to the dialog content
        DialogCtrl dialogCtrl;

        public DialogContent DialogContent { get => dialogContent; set => dialogContent = value; }

        public string GetInteractionText()
        {
            return interactionText;
        }

        public void Interact()
        {
            dialogCtrl = FindFirstObjectByType<DialogCtrl>();
            if (dialogCtrl != null && DialogContent != null)
            {
                if (cameraView != null)
                {
                    cameraView.gameObject.SetActive(true);
                }
                dialogCtrl.OpenDialogBox(this);
            }
        }

        public bool CanInteract()
        {
            return isInteractable;
        }

        public void OnCloseDialog()
        {  
            if (cameraView != null)
            {
                cameraView.gameObject.SetActive(false);
            }
        }
    }
}
