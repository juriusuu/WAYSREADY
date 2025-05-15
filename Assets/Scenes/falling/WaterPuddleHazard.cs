using UnityEngine;

public class WaterPuddleHazard : MonoBehaviour
{
    [SerializeField] private float timePenalty = 5f; // Amount of time to subtract
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TaymerManager timerManager = FindObjectOfType<TaymerManager>();
            if (timerManager != null)
            {
                timerManager.AddTime(-timePenalty); // Decrease time
                Debug.Log($"Time decreased by {timePenalty} seconds. Remaining time: {timerManager.remainingTime}");
            }
            Destroy(gameObject); // Destroy the puddle after triggering
        }
    }
}