public interface IInteractable
{
    void Interact(PlayerInteract player);
    string GetInteractionText(PlayerInteract player);
}