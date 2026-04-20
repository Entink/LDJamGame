using UnityEngine;
using System.Collections;

public class DoorInteractable : MonoBehaviour, IInteractable
{

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

        isOpened = true;

        StartCoroutine(OpenDoorRoutine());
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
        return "Open door [E]";
    }
}