using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player transform
    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y; // Keep the y position of the minimap constant
        transform.position = newPosition; // Update the minimap position to follow the player

    }
}
