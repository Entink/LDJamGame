using UnityEngine;
using System.Collections;

public class LockedDoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool consumeItem;
    [SerializeField] private string missingItemMessage = "You need a key.";

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoorClip;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isOpened = false;

    public void Interact(PlayerInteract player)
    {
        if (isOpened)
            return;

        if (player.HasItem(requiredItemId))
        {
            if (consumeItem)
                player.RemoveItem(requiredItemId, 1);

            isOpened = true;
            StartCoroutine(OpenDoorRoutine());
        }
        else
        {
            InteractionMessageUI.Instance?.ShowMessage(missingItemMessage);
        }
    }

    private IEnumerator OpenDoorRoutine()
    {
        doorCollider.enabled = false;
        spriteRenderer.enabled = false;
        audioSource.PlayOneShot(openDoorClip);

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public string GetInteractionText(PlayerInteract player)
    {
        if (player.HasItem(requiredItemId))
            return "Unlock door [E]";

        return "Locked";
    }
}