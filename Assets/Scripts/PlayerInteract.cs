using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private readonly List<IInteractable> interactablesInRange = new List<IInteractable>();

    private void Update()
    {
        IInteractable currentInteractable = GetBestInteractable();
        UpdatePrompt(currentInteractable);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
                currentInteractable.Interact(this);
        }
    }

    private void UpdatePrompt(IInteractable interactable)
    {
        if (InteractionMessageUI.Instance == null)
            return;

        if (interactable == null)
        {
            InteractionMessageUI.Instance.ClearPrompt();
            return;
        }

        string text = interactable.GetInteractionText(this);

        if (string.IsNullOrEmpty(text))
            InteractionMessageUI.Instance.ClearPrompt();
        else
            InteractionMessageUI.Instance.SetPrompt(text);
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

        if (interactable == null)
            interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null && !interactablesInRange.Contains(interactable))
            interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable == null)
            interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
            interactablesInRange.Remove(interactable);
    }
}