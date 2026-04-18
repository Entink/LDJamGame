using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    private readonly List<IInteractable> interactablesInRange = new List<IInteractable>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IInteractable interactable = GetBestInteractable();

            if (interactable != null)
                interactable.Interact(this);
        }
    }

    private IInteractable GetBestInteractable()
    {
        interactablesInRange.RemoveAll(x => x == null);

        IInteractable best = null;
        float bestDistance = Mathf.Infinity;

        foreach (IInteractable interactable in interactablesInRange)
        {
            MonoBehaviour interactableBehaviour = interactable as MonoBehaviour;
            if (interactableBehaviour == null) continue;

            float distance = Vector2.Distance(transform.position, interactableBehaviour.transform.position);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = interactable;
            }
        }

        return best;
    }

    public bool HasItem(string itemId, int amount = 1)
    {
        return InventoryManager.Instance != null && InventoryManager.Instance.HasItem(itemId, amount);
    }

    public bool RemoveItem(string itemId, int amount = 1)
    {
        return InventoryManager.Instance != null && InventoryManager.Instance.RemoveItem(itemId, amount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && !interactablesInRange.Contains(interactable))
            interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
            interactablesInRange.Remove(interactable);
    }
}