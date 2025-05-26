using UnityEngine;

public class HouseEntrance : MonoBehaviour
{
    public string requiredKeyName; // Set this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerKeyHolder keyHolder = other.GetComponent<PlayerKeyHolder>();
            if (keyHolder != null && keyHolder.HasKey(requiredKeyName))
            {
                Debug.Log("You have the correct key! Entering the house...");
                // Allow entry (e.g., open door, load scene, etc.)
            }
            else
            {
                Debug.Log("You need the correct key to enter!");
                // Optionally, show a UI message
            }
        }
    }
}