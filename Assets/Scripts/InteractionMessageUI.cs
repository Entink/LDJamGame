using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionMessageUI : MonoBehaviour
{
    public static InteractionMessageUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float messageDuration = 2f;
    //[SerializeField] private float fadeDuration = 0.2f;

    private Coroutine messageRoutine;
    private string currentPrompt = "";
    private bool isShowingTemporaryMessage;

    private void Awake()
    {
        Instance = this;
        HideImmediate();
    }

    public void SetPrompt(string text)
    {
        currentPrompt = text;

        if (isShowingTemporaryMessage)
            return;

        if (string.IsNullOrEmpty(currentPrompt))
            HideImmediate();
        else
            ShowImmediate(currentPrompt);
    }

    public void ClearPrompt()
    {
        currentPrompt = "";

        if (isShowingTemporaryMessage)
            return;

        HideImmediate();
    }

    public void ShowMessage(string message)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        isShowingTemporaryMessage = true;
        ShowImmediate(message);

        yield return new WaitForSeconds(messageDuration);

        isShowingTemporaryMessage = false;

        if (string.IsNullOrEmpty(currentPrompt))
            HideImmediate();
        else
            ShowImmediate(currentPrompt);
    }

    private void ShowImmediate(string text)
    {
        if (messageText != null)
            messageText.text = text;

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    private void HideImmediate()
    {
        if (messageText != null)
            messageText.text = "";

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}