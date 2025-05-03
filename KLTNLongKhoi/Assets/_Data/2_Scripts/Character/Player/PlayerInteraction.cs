using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject interactionIcon;

        private IInteractable currentInteractable;

        private void Update()
        {
            HandleInteraction();
        }

        private void OnTriggerEnter(Collider other)
        { 
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                currentInteractable = interactable;
                ShowInteractionUI(interactable.GetInteractionText());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();

            if (interactable != null && interactable == currentInteractable)
            {
                currentInteractable = null;
                HideInteractionUI();
            }
        }

        private void HandleInteraction()
        {
            if (currentInteractable != null && Input.GetKeyDown(KeyCode.E) && currentInteractable.CanInteract())
            {
                currentInteractable.Interact();
            }
        }

        private void ShowInteractionUI(string text)
        {
            Debug.Log("ShowInteractionUI " + text); 
        }

        private void HideInteractionUI()
        {
            Debug.Log("HideInteractionUI");
        }
    }
}