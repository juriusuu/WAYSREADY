using UnityEngine;

public class Fire : MonoBehaviour
{
    public float extinguishTime = 3f; // Time required to extinguish the fire

    private ParticleSystem fireParticleSystem; // Reference to the particle system
    private float extinguishProgress = 0f; // Progress of extinguishing
    private bool isExtinguished = false;

    // Public property to access extinguishProgress
    public float ExtinguishProgress => extinguishProgress;

    void Start()
    {
        // Get the ParticleSystem component attached to this GameObject
        fireParticleSystem = GetComponent<ParticleSystem>();
        if (fireParticleSystem == null)
        {
            Debug.LogError("No ParticleSystem found on the Fire GameObject!");
        }
    }

    void Update()
    {
        if (isExtinguished && fireParticleSystem.isPlaying)
        {
            fireParticleSystem.Stop(); // Stop the particle system when extinguished
        }
    }

    public void Extinguish(float amount)
    {
        if (isExtinguished) return;

        extinguishProgress += amount * Time.deltaTime;

        if (extinguishProgress >= extinguishTime)
        {
            isExtinguished = true;
        }
    }
}