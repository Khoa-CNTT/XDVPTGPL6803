
namespace KLTNLongKhoi
{
    public interface IInteractable
    {
        string GetInteractionText();
        void Interact();
        bool CanInteract();
    }
}