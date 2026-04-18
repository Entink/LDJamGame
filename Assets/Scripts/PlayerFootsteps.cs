using UnityEngine;
using System.Collections;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;

    [SerializeField] private float movementThreshold = 0.05f;
    [SerializeField] private float fadeOutDuration = 0.3f;
    [SerializeField] private float pitchMin = 0.95f;
    [SerializeField] private float pitchMax = 1.05f;

    private bool wasMoving;
    private bool isFadingOut;
    private Coroutine fadeCoroutine;
    private float defaultVolume;
    private int lastPlayedIndex = -1;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        audioSource = this.GetComponent<AudioSource>();
        defaultVolume = audioSource.volume;
    }

    private void Update()
    {
        bool isMoving = rb.linearVelocity.magnitude > movementThreshold;

        if(isMoving)
        {
            if(fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }

            isFadingOut = false;
            audioSource.volume = defaultVolume;

            if(!audioSource.isPlaying)
            {
                PlayRandomFootstep();
            }
        }
        else
        {
            if(audioSource.isPlaying && !isFadingOut)
            {
                fadeCoroutine = StartCoroutine(FadeOutStep());
            }
        }
    }
    private void PlayRandomFootstep()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, footstepClips.Length);
        }
        while (randomIndex == lastPlayedIndex);

        lastPlayedIndex = randomIndex;

        audioSource.clip = footstepClips[randomIndex];
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = defaultVolume;
        audioSource.Play();
    }

    private IEnumerator FadeOutStep()
    {
        isFadingOut = true;
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < fadeOutDuration && audioSource.isPlaying)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / fadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = defaultVolume;
        isFadingOut = false;
        fadeCoroutine = null;
    }


}
