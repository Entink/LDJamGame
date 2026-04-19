using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameStartManager : MonoBehaviour
{
    public static GameStartManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject startScreenRoot;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI pressSpaceText;

    [Header("Auto found scene refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Tilemap wallsTilemap;
    [SerializeField] private string wallsTilemapName = "Walls";

    private bool hasStarted = false;
    private bool firstGameLaunch = true;

    public bool HasStarted => hasStarted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SetupCurrentScene();
    }

    private void Update()
    {
        if (hasStarted)
            return;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (firstGameLaunch)
                StartFirstGame();
            else
                return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupCurrentScene();
    }

    private void SetupCurrentScene()
    {
        FindSceneReferences();

        if(firstGameLaunch)
        {
            ShowStartScreen();

        }
        else
        {
            StartLevelImmediately();
        }
    }

    private void FindSceneReferences()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        wallsTilemap = FindWallsTilemap();
    }

    private Tilemap FindWallsTilemap()
    {
        Tilemap[] allTilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);

        for (int i = 0; i < allTilemaps.Length; i++)
        {
            if (allTilemaps[i].name == wallsTilemapName)
                return allTilemaps[i];
        }

        return null;
    }

    private void ShowStartScreen()
    {
        hasStarted = false;


        startScreenRoot.SetActive(true);


        playerController.SetCanMove(false);
        playerController.SetCanUsePing(false);


        SetWallsAlpha(1f);
    }

    private void StartFirstGame()
    {
        firstGameLaunch = false;
        hasStarted = true;


        startScreenRoot.SetActive(false);

        SetWallsAlpha(0f);


        playerController.SetCanMove(true);
        playerController.SetCanUsePing(true);
        playerController.ForceFirePing();
        
    }

    private void StartLevelImmediately()
    {
        hasStarted = true;

        startScreenRoot.SetActive(false);

        SetWallsAlpha(0f);

        playerController.SetCanMove(true);
        playerController.SetCanUsePing(true);
    }

    private void SetWallsAlpha(float alpha)
    {

        Color color = wallsTilemap.color;
        color.a = alpha;
        wallsTilemap.color = color;
    }

    public void ResetToFirstLaunch()
    {
        firstGameLaunch = true;
        hasStarted = false;
    }
}