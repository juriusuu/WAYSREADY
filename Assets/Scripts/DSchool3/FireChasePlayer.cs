using UnityEngine;

public class FireChasePlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float speed = 2f; // Speed at which the fire moves
    public float stopDistance = 1.5f; // Distance at which the fire stops chasing

    private void Update()
    {
        if (player != null)
        {
            // Calculate the distance to the player
            float distance = Vector3.Distance(transform.position, player.position);

            // Move towards the player if the distance is greater than the stop distance
            if (distance > stopDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
}