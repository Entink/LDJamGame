using UnityEngine;

public class EnemyFootsteps : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepsClips;

    [Header("Movement Check")]
    [SerializeField] private float minVelocityForStep = 0.05f;

    [Header("Timing")]
    [SerializeField] private float randomIntervalOffset = 0.05f;
    [SerializeField] private float minStepInterval = 0.45f;
    [SerializeField] private float maxStepInterval = 0.8f;
    [SerializeField] private float speedForFastestSteps = 2.5f;

    [Header("Audio")]
    [SerializeField] private float volume = 1f;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    private float stepTimer;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (footstepSource == null)
            footstepSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (rb == null || footstepSource == null)
            return;

        if (footstepsClips == null || footstepsClips.Length == 0)
            return;

        float speed = rb.linearVelocity.magnitude;

        if (speed < minVelocityForStep)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer > 0f)
            return;

        PlayStep();

        float speedLerp = Mathf.Clamp01(speed / speedForFastestSteps);
        float baseInterval = Mathf.Lerp(maxStepInterval, minStepInterval, speedLerp);

        stepTimer = baseInterval + Random.Range(-randomIntervalOffset, randomIntervalOffset);
    }

    private void PlayStep()
    {
        AudioClip clip = footstepsClips[Random.Range(0, footstepsClips.Length)];
        footstepSource.pitch = Random.Range(minPitch, maxPitch);
        footstepSource.PlayOneShot(clip, volume);
    }
}