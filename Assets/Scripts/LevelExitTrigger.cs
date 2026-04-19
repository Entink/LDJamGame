using UnityEngine;

public class LevelExitTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null)
            return;

        if (FloorProgressManager.Instance != null)
            FloorProgressManager.Instance.CompleteFloorAndLoadNext();
    }
}
