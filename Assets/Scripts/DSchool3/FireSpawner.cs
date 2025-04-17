using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    public GameObject firePrefab; // Reference to the fire prefab
    public Transform[] spawnPoints; // Array of spawn points for the fires
    public Transform player; // Reference to the player

    private void Start()
    {
        // Automatically spawn fires when the game starts
        SpawnFires();
    }

    public void SpawnFires()
    {
        if (firePrefab != null && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Debug.Log($"Spawning fire at: {spawnPoint.position}");
                GameObject fire = Instantiate(firePrefab, spawnPoint.position, Quaternion.identity);

                FireChasePlayer fireChase = fire.GetComponent<FireChasePlayer>();
                if (fireChase != null)
                {
                    fireChase.player = player;
                    Debug.Log("Assigned player to FireChasePlayer script.");
                }
                else
                {
                    Debug.LogWarning("FireChasePlayer script not found on fire prefab.");
                }
            }

            Debug.Log("All fires spawned successfully!");
        }
        else
        {
            Debug.LogError("FirePrefab or SpawnPoints are not assigned.");
        }
    }
}