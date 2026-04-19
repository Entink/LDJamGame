using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool isGameOver;
    private bool canRestart;

    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);

    }

    private void Update()
    {
        if (!isGameOver || !canRestart)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            RestartCurrentScene();
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

    private void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        canRestart = false;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ClearInventory();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

}
