using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitMarkerColorizer2D : MonoBehaviour
{
    [Header("Colors (Alpha is inherited natively!)")]
    public Color enemyColor;
    public Color interactableColor;
    public Color respawnColor;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        // Initialize the array once to prevent lag
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    // LateUpdate runs after the physics engine has moved and spawned the particles
    void LateUpdate()
    {
        int count = ps.GetParticles(particles);
        bool needsUpdate = false;

        for (int i = 0; i < count; i++)
        {
            // Only check particles that were JUST born (less than 0.1 seconds old)
            // This keeps the script incredibly fast!
            float age = particles[i].startLifetime - particles[i].remainingLifetime;
            
            if (age < 0.1f)
            {
                // Grab the beautiful perfect Alpha that the Native Sub-Emitter just inherited
                float inheritedAlpha = particles[i].startColor.a;

                // Draw a tiny invisible circle around the particle to see what it landed on
                Collider2D hit = Physics2D.OverlapCircle(particles[i].position, 0.1f);

                if (hit != null)
                {
                    // Change the RGB color, but KEEP the inherited Alpha!
                    if (hit.CompareTag("Enemy"))
                    {
                        particles[i].startColor = new Color(enemyColor.r, enemyColor.g, enemyColor.b, inheritedAlpha);
                        needsUpdate = true;
                    }
                    else if (hit.CompareTag("Interactable"))
                    {
                        particles[i].startColor = new Color(interactableColor.r, interactableColor.g, interactableColor.b, inheritedAlpha);
                        needsUpdate = true;
                    }
                    else if (hit.CompareTag("RespawnAnchor"))
                    {
                        particles[i].startColor = new Color(respawnColor.r, respawnColor.g, respawnColor.b, inheritedAlpha);
                        needsUpdate = true;
                    }
                }
            }
        }

        // If we painted any particles, update the system
        if (needsUpdate)
        {
            ps.SetParticles(particles, count);
        }
    }
}