using UnityEngine;

public class PingReactiveItem : MonoBehaviour
{
    [Header("Reaction")]
    [SerializeField] private AudioClip pingDetectedSound;
    [SerializeField] private ParticleSystem hitVfxPrefab;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float volume = 1f;

    [Header("Spam Protection")]
    [SerializeField] private float cooldown = 0.15f;

    private float lastTriggerTime = -999f;

    private void Awake()
    {

        audioSource = GetComponent<AudioSource>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (Time.time < lastTriggerTime + cooldown)
            return;

        lastTriggerTime = Time.time;

        PlaySound();
        SpawnHitVfx();
    }

    private void PlaySound()
    {
        if (audioSource != null && pingDetectedSound != null)
            audioSource.PlayOneShot(pingDetectedSound, volume);
    }

    private void SpawnHitVfx()
    {
        if (hitVfxPrefab == null)
            return;

        ParticleSystem spawnedVfx = Instantiate(
            hitVfxPrefab,
            transform.position,
            Quaternion.identity
        );

        spawnedVfx.Play();

        float destroyAfter = spawnedVfx.main.duration + spawnedVfx.main.startLifetime.constantMax + 0.5f;
        Destroy(spawnedVfx.gameObject, destroyAfter);
    }
}
