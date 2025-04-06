using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f; // Phạm vi tương tác
    public LayerMask interactableLayer;
    public GameObject interactionIcon; // Tham chiếu đến icon

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void FixedUpdate()
    {
        CheckForInteractable();
    }

    private bool PerformRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayer);
    }

    private void TryInteract()
    {
        RaycastHit hit;
        if (PerformRaycast(out hit))
        { 
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void CheckForInteractable()
    {
        RaycastHit hit;
        if (PerformRaycast(out hit))
        {
            interactionIcon.SetActive(true);
        }
        else
        {
            interactionIcon.SetActive(false);
        }
    }
}