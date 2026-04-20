using UnityEngine;
using System.Collections;

public class CheckpointInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform playerT;

    private void Awake()
    {
        playerT = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void Interact(PlayerInteract player)
    {
        GameOverMananger.Instance.SetCheckpointPosition(playerT);
    }

    public string GetInteractionText(PlayerInteract player)
    {
        return "Save position [E]";
    }
}