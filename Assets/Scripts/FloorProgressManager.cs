using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorProgressManager : MonoBehaviour
{
    public static FloorProgressManager Instance;

    private const string CurrentFloorKey = "CurrentFloor";

    [SerializeField] private int currentFloor = 0;
    [SerializeField] private float restartHoldTime = 2f;

    private float restartHoldTimer = 0f;

    public int CurrentFloor => currentFloor;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentFloor = PlayerPrefs.GetInt(CurrentFloorKey, 0);
    }

    private void Update()
    {
        HandleRestartInput();
    }

    private void HandleRestartInput()
    {
        if (Input.GetKey(KeyCode.R))
        {
            restartHoldTimer += Time.unscaledDeltaTime;

            if (restartHoldTimer >= restartHoldTime)
                ResetProgress();
        }
        else
        {
            restartHoldTimer = 0f;
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(CurrentFloorKey);
        PlayerPrefs.Save();

        currentFloor = 0;
        SceneManager.LoadScene("SampleScene");
    }

    public void CompleteFloor()
    {
        currentFloor++;
        PlayerPrefs.SetInt(CurrentFloorKey, currentFloor);
        PlayerPrefs.Save();
    }

    public void CompleteFloorAndLoadNext()
    {
        CompleteFloor();
        LoadCurrentFloorScene();
    }

    public void LoadCurrentFloorScene()
    {
        SceneManager.LoadScene(GetSceneNameForFloor(currentFloor));
    }

    public string GetSceneNameForFloor(int floor)
    {
        switch (floor)
        {
            case 0: return "SampleScene";
            case 1: return "Floor1";
            case 2: return "Floor2";
            case 3: return "Floor3";
            default: return "SampleScene";
        }
    }
}
