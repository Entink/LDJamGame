using UnityEngine;

public class EnemyKillPlayerOnTouch : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag))
            return;

        if (GameOverMananger.Instance == null)
            return;

        GameOverMananger.Instance.TriggerGameOver(gameObject);
    }
}
