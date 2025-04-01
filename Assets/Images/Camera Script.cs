using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow (character)
    public float distance = 5.0f; // Default distance from the target
    public float height = 2.0f; // Height above the target
    public float smoothSpeed = 0.125f; // Speed of the camera movement
    public float wallOffset = 0.3f; // Offset from walls to avoid clipping
    public LayerMask obstacleLayers; // Set this in Unity to include walls

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Compute desired camera position behind the character
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // Check for walls between camera and player
        RaycastHit hit;
        if (Physics.Linecast(target.position + Vector3.up * height, desiredPosition, out hit, obstacleLayers))
        {
            // If there's an obstacle, place the camera just in front of it
            desiredPosition = hit.point + hit.normal * wallOffset;
        }

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // Make the camera look at the target's position
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}
