using UnityEngine;
using System.Collections;

public class CheckpointInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform playerT;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip saveClip;

    private void Awake()
    {
        playerT = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void Interact(PlayerInteract player)
    {
        GameOverMananger.Instance.SetCheckpointPosition(playerT);
        audioSource.PlayOneShot(saveClip);
    }

    public string GetInteractionText(PlayerInteract player)
    {
        return "Save position [E]";
    }
}