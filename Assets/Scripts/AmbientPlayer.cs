using UnityEngine;
using System.Collections;

public class AmbientPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;
    [SerializeField] private AudioClip[] ambientClips;

    [SerializeField] private float targetVolume = 0.7f;
    [SerializeField] private float crossfadeDuration = 2f;
    [SerializeField] private float nextTrackEarlyTime = 1.5f;
    [SerializeField] private bool playOnStart = true;

    private AudioSource currentSource;
    private AudioSource nextSource;
    private int lastClipIndex = -1;
    private Coroutine playbackCoroutine;

    private void Awake()
    {
        if(sourceA == null || sourceB == null)
        {
            AudioSource[] sources = GetComponents<AudioSource>();

            if(sources.Length >= 2)
            {
                sourceA = sources[0];
                sourceB = sources[1];
            }
        }

        sourceA.loop = false;
        sourceB.loop = false;
        sourceA.playOnAwake = false;
        sourceB.playOnAwake = false;

        sourceA.volume = 0f;
        sourceB.volume = 0f;

        currentSource = sourceA;
        nextSource = sourceB;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(playOnStart)
        {
            StartAmbient();
        }
    }

    public void StartAmbient()
    {
        if (playbackCoroutine != null)
            StopCoroutine(playbackCoroutine);

        playbackCoroutine = StartCoroutine(AmbientLoop());
    }

    public void StopAmbient(float fadeOutTime = 2f)
    {
        if(playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }

        StartCoroutine(FadeOutAndStopAll(fadeOutTime));
    }

    private IEnumerator AmbientLoop()
    {
        AudioClip firstClip = GetRandomClip();
        currentSource.clip = firstClip;
        currentSource.volume = 0f;
        currentSource.Play();

        yield return StartCoroutine(Fade(currentSource, 0f, targetVolume, crossfadeDuration));

        while (true)
        {
            float waitTime = currentSource.clip.length - nextTrackEarlyTime - crossfadeDuration;

            if (waitTime < 0f)
                waitTime = 0f;

            yield return new WaitForSeconds(waitTime);

            AudioClip newClip = GetRandomClip();
            nextSource.clip = newClip;
            nextSource.volume = 0f;
            nextSource.Play();

            float fadeTime = Mathf.Min(crossfadeDuration, currentSource.clip.length);
            yield return StartCoroutine(Crossfade(currentSource, nextSource, fadeTime));

            currentSource.Stop();
            currentSource.volume = 0f;

            AudioSource temp = currentSource;
            currentSource = nextSource;
            nextSource = temp;
        }
    }

    private AudioClip GetRandomClip()
    {
        if (ambientClips.Length == 1)
            return ambientClips[0];

        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, ambientClips.Length);
        }
        while (randomIndex == lastClipIndex);

        lastClipIndex = randomIndex;
        return ambientClips[randomIndex];
    }

    private IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, float duration)
    {
        float time = 0f;
        float fromStartVolume = fromSource.volume;
        float toStartVolume = toSource.volume;

        while(time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            fromSource.volume = Mathf.Lerp(fromStartVolume, 0f, t);
            toSource.volume = Mathf.Lerp(toStartVolume, 0f, t);

            yield return null;
        }

        fromSource.volume = 0f;
        toSource.volume = targetVolume;
    }

    private IEnumerator Fade(AudioSource source, float startVolume, float endVolume, float duration)
    {
        float time = 0f;
        source.volume = startVolume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            source.volume = Mathf.Lerp(startVolume, endVolume, t);
            yield return null;
        }

        source.volume = endVolume;
    }

    private IEnumerator FadeOutAndStopAll(float duration)
    {
        float startVolumeA = sourceA.volume;
        float startVolumeB = sourceB.volume;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            sourceA.volume = Mathf.Lerp(startVolumeA, 0f, t);
            sourceB.volume = Mathf.Lerp(startVolumeB, 0f, t);

            yield return null;
        }

        sourceA.volume = 0f;
        sourceB.volume = 0f;

        sourceA.Stop();
        sourceB.Stop();
    }
}
