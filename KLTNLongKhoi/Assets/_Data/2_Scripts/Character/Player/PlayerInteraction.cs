using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

namespace KLTNLongKhoi
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject interactionIcon;
        private StarterAssetsInputs starterAssetsInputs;
        private IInteractable currentInteractable;

        private void Awake()
        {
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        }

        private void OnEnable()
        {
            starterAssetsInputs.Interact += HandleInteraction;
        }

        private void OnDisable()
        {
            starterAssetsInputs.Interact -= HandleInteraction;
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
            if (currentInteractable != null && currentInteractable.CanInteract())
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