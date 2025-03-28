using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the character
    private Vector3 moveDirection; // Direction of movement

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Only process the touch if it is in the Began or Moved phase
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                // Get the touch position and convert it to world space
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));

                // Calculate the direction to move towards
                moveDirection = new Vector3(touchPosition.x - transform.position.x, 0, touchPosition.z - transform.position.z).normalized;

                // Move the character
                MoveCharacter();
            }
        }
    }

    void MoveCharacter()
    {
        // Move the character in the specified direction
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}