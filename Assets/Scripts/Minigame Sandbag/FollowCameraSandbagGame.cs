using UnityEngine;

public class FollowCameraSandbagGame : MonoBehaviour
{
    public Transform target; // Target to follow (e.g., player)
    public Transform targetForwardReference; // Reference to determine where the player is facing

    [Header("Camera Position")]
    public float distance = 5.0f; // Distance from target
    public float height = 2.0f; // Height above target
    public float smoothSpeed = 0.125f; // Camera follow smoothness

    [Header("Wall Avoidance")]
    public float wallOffset = 0.3f; // Offset from walls to prevent clipping
    public LayerMask obstacleLayers; // Set this to include walls

    [Header("Swipe Rotation")]
    public float rotationSpeed = 200f; // Sensitivity of swipe rotation
    public float minPitch = -10f; // Lower limit for vertical rotation
    public float maxPitch = 30f; // Upper limit for vertical rotation

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null || targetForwardReference == null) return;

        // Calculate the desired camera position based on the player's facing direction
        Vector3 forwardDirection = targetForwardReference.forward;
        Vector3 desiredPosition = target.position - forwardDirection * distance + Vector3.up * height;

        // Prevent camera from clipping into walls
        RaycastHit hit;
        if (Physics.Linecast(target.position + Vector3.up * height, desiredPosition, out hit, obstacleLayers))
        {
            desiredPosition = hit.point + hit.normal * wallOffset;
        }

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // Make the camera look at the target
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}