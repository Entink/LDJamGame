using UnityEngine;
using TMPro;

public class EndTrigger : MonoBehaviour
{
    public GameObject endScreenUI;
    public TextMeshProUGUI totalTimeText;

    private bool triggered = false;

    private void Awake()
    {
        if (endScreenUI != null)
        {
            endScreenUI.SetActive(false);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        if(FloorProgressManager.Instance != null)
        {
            FloorProgressManager.Instance.MarkRunCompleted();

            if (totalTimeText != null)
                totalTimeText.text = "Total Time: " + FloorProgressManager.Instance.GetFormattedRunTime();
        }

        if (endScreenUI != null)
            endScreenUI.SetActive(true);

        Time.timeScale = 0f;

        
    }
}