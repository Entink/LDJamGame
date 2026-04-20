using UnityEngine;
using System.Collections;

public class PlayerRandomAudio : MonoBehaviour
{
    [SerializeField] private AudioSource randomAudioSource;
    [SerializeField] private AudioClip[] randomClips;

    [SerializeField] private float minDelay = 15f;
    [SerializeField] private float maxDelay = 40f;

    [SerializeField] private float minVolume = 0.65f;
    [SerializeField] private float maxVolume = 1f;

    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool allowSameClipTwiceInRow = false;

    private int lastClipIndex = -1;
    private Coroutine playRoutine;

    private void OnEnable()
    {
        if(playOnStart)
        {
            playRoutine = StartCoroutine(RandomSoundRoutine());
        }
    }

    private void OnDisable()
    {
        if(playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }
    }

    private void PlayRandomClip()
    {
        int clipIndex = GetRandomClipIndex();
        AudioClip clip = randomClips[clipIndex];

        randomAudioSource.pitch = Random.Range(minPitch, maxPitch);
        float volume = Random.Range(minVolume, maxVolume);

        randomAudioSource.PlayOneShot(clip, volume);

        lastClipIndex = clipIndex;
    }

    private int GetRandomClipIndex()
    {
        int clipIndex = Random.Range(0, randomClips.Length);

        if(allowSameClipTwiceInRow)
        {
            return clipIndex;
        }

        while(clipIndex == lastClipIndex)
        {
            clipIndex = Random.Range(0, randomClips.Length);
        }

        return clipIndex;
    }

    private IEnumerator RandomSoundRoutine()
    {
        while(true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            PlayRandomClip();
        }
    }
}
