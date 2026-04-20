using UnityEngine;
using System.Collections;

public class LevelExitTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip levelCompleteClip;
    [SerializeField] private float loadDelay = 0.5f;

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered)
            return;

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null)
            return;

        audioSource.PlayOneShot(levelCompleteClip);

        StartCoroutine(LoadNextLevelRoutine());

        
    }

    private IEnumerator LoadNextLevelRoutine()
    {
        yield return new WaitForSeconds(loadDelay);

        FloorProgressManager.Instance.CompleteFloorAndLoadNext();
    }
}
