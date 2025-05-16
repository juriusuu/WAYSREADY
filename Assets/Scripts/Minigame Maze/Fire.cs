using UnityEngine;

public class Fire : MonoBehaviour
{
    public float extinguishTime = 3f; // Time required to extinguish the fire

    private ParticleSystem fireParticleSystem; // Reference to the particle system
    private float extinguishProgress = 0f; // Progress of extinguishing
    private bool isExtinguished = false;

    // Public property to access extinguishProgress
    public float ExtinguishProgress => extinguishProgress;
    public float timePenalty = 1f; // Time to subtract on collision
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
    private float penaltyCooldown = 1f; // Time in seconds between penalties
    private float penaltyTimer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            penaltyTimer += Time.deltaTime;
            if (penaltyTimer >= penaltyCooldown)
            {
                TaymerManagerFireEscape timerManager = FindObjectOfType<TaymerManagerFireEscape>();
                if (timerManager != null)
                {
                    timerManager.DecreaseTime(timePenalty);
                    Debug.Log($"Player is inside fire! Timer decreased by {timePenalty} seconds.");
                }
                penaltyTimer = 0f; // Reset timer
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            penaltyTimer = 0f; // Reset timer when player leaves
        }
    }
    /* 
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TaymerManagerFireEscape timerManager = FindObjectOfType<TaymerManagerFireEscape>();
                if (timerManager != null)
                {
                    timerManager.DecreaseTime(timePenalty);
                    Debug.Log($"Player touched fire! Timer decreased by {timePenalty} seconds.");
                }
            }
        } */
}