using UnityEngine;
using System.Collections;
using TMPro;

public class GameOverMananger : MonoBehaviour
{
    public static GameOverMananger Instance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gameOverClips;

    [Header("Reveal")]
    [SerializeField] private float revealedEnemyAlpha = 0.5f;
    [SerializeField] private float revealDuration = 2f;

    [Header("Respawn")]
    [SerializeField] private Vector2 checkpointPosition;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float respawnProtection = 0.5f;

    [SerializeField] private Vector2 backupCheckpoint = Vector2.zero;
    private bool isGameOver;
    private bool canRestart;
    [SerializeField] private float respawnProtectionTimer;
    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        SetCheckpointPosition(playerPosition);
        gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        SetCheckpointPosition(playerPosition);
    }

    private void Update()
    {
        playerPosition = GameObject.Find("Player").GetComponent<Transform>();

        respawnProtectionTimer -= Time.deltaTime;
        if (respawnProtectionTimer <= -1f)
            respawnProtectionTimer = -1f;

        if (!isGameOver || !canRestart)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    public void TriggerGameOver(GameObject enemyObject)
    {
        if (isGameOver)
            return;

        isGameOver = true;
        StartCoroutine(GameOverSequence(enemyObject));
    }

    private IEnumerator GameOverSequence(GameObject enemyObject)
    {
        Time.timeScale = 0f;
        RevealEnemy(enemyObject);

        PlayRandomGameOverClip();

        yield return new WaitForSecondsRealtime(revealDuration);
        HideEnemy(enemyObject);

        gameOverPanel.SetActive(true);

        canRestart = true;
        
    }


    private void PlayRandomGameOverClip()
    {
        if (audioSource == null)
            return;

        if (gameOverClips == null || gameOverClips.Length == 0)
            return;

        AudioClip clip = gameOverClips[Random.Range(0, gameOverClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    private void RevealEnemy(GameObject enemyObject)
    {
        if (enemyObject == null)
            return;

        SpriteRenderer sr = enemyObject.GetComponent<SpriteRenderer>();

        Color color = sr.color;
        color.a = revealedEnemyAlpha;
        sr.color = color;
        

    }

    private void HideEnemy(GameObject enemyObject)
    {
        if (enemyObject == null)
            return;

        SpriteRenderer sr = enemyObject.GetComponent<SpriteRenderer>();

        Color color = sr.color;
        color.a = 0f;
        sr.color = color;
    }

    public void SetCheckpointPosition(Transform pos)
    {
        checkpointPosition = pos.position;
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        canRestart = false;
        isGameOver = false;
        gameOverPanel.SetActive(false);

        if(respawnProtectionTimer >= 0f)
        {
            playerPosition.position = backupCheckpoint;
            respawnProtectionTimer = respawnProtection;

            return;
        }

        playerPosition.position = checkpointPosition;
        respawnProtectionTimer = respawnProtection;
    }

}
