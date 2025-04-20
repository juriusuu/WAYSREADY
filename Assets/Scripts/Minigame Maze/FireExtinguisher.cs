using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public ParticleSystem extinguisherEffect; // Reference to the particle system
    public float range = 5f; // Range of the extinguisher spray
    public float extinguishAmount = 1f; // Amount to extinguish per second

    void Start()
    {
        // Ensure the particle system is stopped at the start
        if (extinguisherEffect != null)
        {
            extinguisherEffect.Stop();
        }
    }

    public void UseExtinguisher()
    {
        // Play the extinguisher effect
        if (extinguisherEffect != null && !extinguisherEffect.isPlaying)
        {
            extinguisherEffect.Play();
        }

        // Cast a ray to detect fires
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Fire fire = hit.collider.GetComponent<Fire>();
            if (fire != null)
            {
                // Gradually extinguish the fire
                fire.Extinguish(extinguishAmount);
                if (fire.ExtinguishProgress >= fire.extinguishTime)
                {
                    Destroy(fire.gameObject); // Destroy the fire GameObject if fully extinguished
                }
            }
        }

        // Stop the extinguisher effect after a short delay
        Invoke(nameof(StopExtinguisherEffect), 1f);
    }

    private void StopExtinguisherEffect()
    {
        if (extinguisherEffect != null && extinguisherEffect.isPlaying)
        {
            extinguisherEffect.Stop();
        }
    }
}