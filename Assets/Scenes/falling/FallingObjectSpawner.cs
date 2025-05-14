using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    public GameObject fallingObjectPrefab; // Assign the falling object prefab in the Inspector
    public float spawnInterval = 2f; // Time between spawns
    public BoxCollider spawnArea; // Assign a BoxCollider in the Inspector to define the spawn area

    private void Start()
    {
        if (spawnArea == null)
        {
            Debug.LogError("Spawn Area (BoxCollider) is not assigned!");
            return;
        }

        InvokeRepeating(nameof(SpawnFallingObject), 0f, spawnInterval); // Repeatedly spawn objects
    }

    private void SpawnFallingObject()
    {
        // Get a random position within the BoxCollider bounds
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
            Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
        );

        // Instantiate the falling object
        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnArea != null)
        {
            // Draw the spawn area in the Scene view for visualization
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}