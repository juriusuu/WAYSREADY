using UnityEngine;

public class CanvasFollow : MonoBehaviour
{
    public Transform player; // Assign the player transform in the Inspector
    public Camera mainCamera; // Assign the main camera in the Inspector
    public Vector3 offset; // Offset to position the canvas above the player

    void Update()
    {
        if (player != null)
        {
            // Set the position of the canvas to be above the player
            transform.position = player.position + offset;

            // Make the canvas face the camera
            transform.LookAt(mainCamera.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Keep it upright
        }
    }
}