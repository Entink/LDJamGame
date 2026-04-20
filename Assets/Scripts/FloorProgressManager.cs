using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorProgressManager : MonoBehaviour
{
    public static FloorProgressManager Instance;

    private const string CurrentFloorKey = "CurrentFloor";
    private const string TotalRunTimeKey = "TotalRunTime";
    private const string RunCompletedKey = "RunCompleted";

    [SerializeField] private int currentFloor = 0;
    [SerializeField] private float restartHoldTime = 2f;

    private float restartHoldTimer = 0f;
    private float totalRunTime = 0f;
    private float saveTimer = 0f;
    private bool runCompleted = false;

    public int CurrentFloor => currentFloor;
    public float TotalRunTime => totalRunTime;
    public bool RunCompleted => runCompleted;

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
        totalRunTime = PlayerPrefs.GetFloat(TotalRunTimeKey, 0f);
        runCompleted = PlayerPrefs.GetInt(RunCompletedKey, 0) == 1;
    }

    private void Update()
    {
        HandleRestartInput();
        UpdateRunTimer();
    }

    private void UpdateRunTimer()
    {
        if (runCompleted)
            return;

        if (Time.timeScale <= 0f)
            return;

        totalRunTime += Time.deltaTime;
        saveTimer += Time.deltaTime;

        if(saveTimer >= 1f)
        {
            SaveTimerData();
            saveTimer = 0f;
        }
    }

    private void SaveTimerData()
    {
        PlayerPrefs.SetFloat(TotalRunTimeKey, totalRunTime);
        PlayerPrefs.SetInt(RunCompletedKey, runCompleted ? 1 : 0);
        PlayerPrefs.Save();
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
        PlayerPrefs.DeleteKey(TotalRunTimeKey);
        PlayerPrefs.DeleteKey(RunCompletedKey);
        PlayerPrefs.Save();

        currentFloor = 0;
        totalRunTime = 0f;
        runCompleted = false;
        restartHoldTime = 0f;
        saveTimer = 0f;

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ClearInventory();

        if (GameStartManager.Instance != null)
            GameStartManager.Instance.ResetToFirstLaunch();

        Time.timeScale = 1f;
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
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ClearInventory();
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

    public void MarkRunCompleted()
    {
        if (runCompleted)
            return;

        runCompleted = true;
        SaveTimerData();
    }

    public string GetFormattedRunTime()
    {
        int totalSeconds = Mathf.FloorToInt(totalRunTime);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    private void OnApplicationQuit()
    {
        SaveTimerData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveTimerData();
    }
}
