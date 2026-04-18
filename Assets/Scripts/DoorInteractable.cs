using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Collider2D blockingCollider;
    [SerializeField] private GameObject closedVisual;
    [SerializeField] private GameObject openVisual;

    public void Interact(PlayerInteract player)
    {
        ToggleDoor();
    }

    public string GetInteractionText(PlayerInteract player)
    {
        return isOpen ? "Close door [E]" : "Open door [E]";
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;

        if (blockingCollider != null)
            blockingCollider.enabled = !isOpen;

        if (closedVisual != null)
            closedVisual.SetActive(!isOpen);

        if (openVisual != null)
            openVisual.SetActive(isOpen);
    }
}
