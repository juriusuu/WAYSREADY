using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Button spawnButton; // Reference to the UI Button
    public SpawnManager spawnManager; // Reference to the SpawnManager

    void Start()
    {
        // If the button is not assigned, find it in the children
        if (spawnButton == null)
        {
            spawnButton = GetComponent<Button>();
        }

        // Ensure the button is not null
        if (spawnButton != null)
        {
            // Add a listener to the button to call the StartSpawning method when clicked
            spawnButton.onClick.AddListener(OnSpawnButtonClicked);
            Debug.Log("Button listener added.");
        }
        else
        {
            Debug.LogWarning("Spawn Button is not assigned in the Inspector.");
        }
    }

    private void OnSpawnButtonClicked()
    {
        Debug.Log("Panel clicked!"); // Log when the panel is clicked

        // Check if the spawn manager is assigned
        if (spawnManager != null)
        {
            spawnManager.StartSpawning(); // Call the method to start spawning
            Debug.Log("SpawnManager StartSpawning called.");
        }
        else
        {
            Debug.LogWarning("SpawnManager is not assigned in the Inspector.");
        }
    }
}