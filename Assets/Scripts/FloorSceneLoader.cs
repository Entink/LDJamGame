using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorSceneLoader : MonoBehaviour
{
    private void Start()
    {
        int currentFloor = PlayerPrefs.GetInt("CurrentFloor", 0);

        if (currentFloor == 0)
            return;

        if (FloorProgressManager.Instance == null)
            return;

        string sceneName = FloorProgressManager.Instance.GetSceneNameForFloor(currentFloor);

        if(SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

    }
}
