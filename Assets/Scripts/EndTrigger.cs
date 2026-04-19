using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject endScreenUI;

    private void Awake()
    {
        // Automatically hide the End Screen immediately
        if (endScreenUI != null)
        {
            endScreenUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            // Turn on the End Screen UI
            endScreenUI.SetActive(true);
            // Stop everything
            Time.timeScale = 0f; 
        }
    }
}