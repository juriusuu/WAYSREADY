using UnityEngine;

public class GameCompleteTrigger : MonoBehaviour
{
    public PanelManager panelManager; // Reference to the PanelManager script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger is the player
        {
            Debug.Log("Player entered the trigger. Triggering Complete Panel.");
            if (panelManager != null)
            {
                panelManager.ShowFinishPanel(); // Call the method to show the Complete Panel
            }
            else
            {
                Debug.LogError("PanelManager is not assigned in the Inspector!");
            }
            Time.timeScale = 0f; // Pause the game
        }
    }
}