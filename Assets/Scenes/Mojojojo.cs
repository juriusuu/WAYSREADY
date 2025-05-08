/* using UnityEngine;

public class Mojojojo : MonoBehaviour
{
    public Transform target; // Target to follow (e.g., player)
    
    [Header("Camera Position")]
    public float distance = 5.0f; // Distance from target
    public float height = 2.0f; // Height above target
    public float smoothSpeed = 0.125f; // Camera follow smoothness

    [Header("Wall Avoidance")]
    public float wallOffset = 0.3f; // Offset from walls to prevent clipping
    public LayerMask obstacleLayers; // Set this to include walls

    [Header("Swipe Rotation")]
    public float rotationSpeed = 200f; // Sensitivity of swipe rotation
    public float minPitch = -30f; // Lower limit for vertical rotation
    public float maxPitch = 60f; // Upper limit for vertical rotation

    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private float yaw = 0f; // Horizontal rotation (left/right)
    private float pitch = 20f; // Vertical rotation (up/down)
    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation(); // Handle swipe input

        // Calculate desired camera position based on yaw and pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

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

    void HandleRotation()
    {
        // Mouse or Touch Input
        if (Input.GetMouseButtonDown(0)) // Start dragging
        {
            isDragging = true;
            lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Stop dragging
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            lastTouchPosition = Input.mousePosition;

            float rotateY = delta.x * rotationSpeed * Time.deltaTime; // Left/Right swipe (Yaw)
            float rotateX = -delta.y * rotationSpeed * Time.deltaTime; // Up/Down swipe (Pitch)

            yaw += rotateY;
            pitch = Mathf.Clamp(pitch + rotateX, minPitch, maxPitch); // Limit vertical movement
        }
    }
}
 *//* 

 using UnityEngine;

public class Mojojojo : MonoBehaviour
{
    public Transform target; // Target to follow (e.g., player)

    [Header("Camera Position")]
    public float distance = 5.0f; // Distance from target
    public float height = 2.0f; // Height above target
    public float smoothSpeed = 0.125f; // Camera follow smoothness

    [Header("Wall Avoidance")]
    public float wallOffset = 0.3f; // Offset from walls to prevent clipping

    [Header("Swipe Rotation")]
    public float rotationSpeed = 200f; // Sensitivity of swipe rotation
    public float minPitch = -30f; // Lower limit for vertical rotation
    public float maxPitch = 60f; // Upper limit for vertical rotation

    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private float yaw = 0f; // Horizontal rotation (left/right)
    private float pitch = 20f; // Vertical rotation (up/down)
    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation(); // Handle swipe input

        // Calculate desired camera position based on yaw and pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        // Prevent camera from clipping into walls
        RaycastHit hit;
        Vector3 targetPosition = target.position + Vector3.up * height; // Start position of the raycast
        if (Physics.Linecast(targetPosition, desiredPosition, out hit)) // No LayerMask used
        {
            // Adjust the camera's position to the hit point, offset by the wallOffset
            desiredPosition = hit.point + hit.normal * wallOffset;

            Debug.Log($"Wall detected at {hit.point}. Adjusting camera to {desiredPosition}.");
        }

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // Make the camera look at the target
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }

    void HandleRotation()
    {
        // Mouse or Touch Input
        if (Input.GetMouseButtonDown(0)) // Start dragging
        {
            isDragging = true;
            lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Stop dragging
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            lastTouchPosition = Input.mousePosition;

            float rotateY = delta.x * rotationSpeed * Time.deltaTime; // Left/Right swipe (Yaw)
            float rotateX = -delta.y * rotationSpeed * Time.deltaTime; // Up/Down swipe (Pitch)

            yaw += rotateY;
            pitch = Mathf.Clamp(pitch + rotateX, minPitch, maxPitch); // Limit vertical movement
        }
    }
} */

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Target to follow (e.g., player)

    [Header("Camera Position")]
    public float distance = 5.0f; // Distance from target
    public float height = 2.0f; // Height above target
    public float smoothSpeed = 0.125f; // Camera follow smoothness

    [Header("Wall and Ceiling Avoidance")]
    public float wallOffset = 0.3f; // Offset from walls and ceilings to prevent clipping
    public LayerMask obstacleLayers; // Set this to include walls and ceilings

    [Header("Swipe Rotation")]
    public float rotationSpeed = 200f; // Sensitivity of swipe rotation
    public float minPitch = -30f; // Lower limit for vertical rotation
    public float maxPitch = 60f; // Upper limit for vertical rotation

    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private float yaw = 0f; // Horizontal rotation (left/right)
    private float pitch = 20f; // Vertical rotation (up/down)
    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation(); // Handle swipe input

        // Calculate desired camera position based on yaw and pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        // Prevent camera from clipping into walls
        RaycastHit hit;
        Vector3 targetPosition = target.position + Vector3.up * height; // Start position of the raycast
        if (Physics.Linecast(targetPosition, desiredPosition, out hit, obstacleLayers)) // Check for walls
        {
            // Adjust the camera's position to the hit point, offset by the wallOffset
            desiredPosition = hit.point + hit.normal * wallOffset;

            Debug.Log($"Wall detected at {hit.point}. Adjusting camera to {desiredPosition}.");
        }

        // Prevent camera from clipping into ceilings
        if (Physics.Raycast(desiredPosition, Vector3.up, out hit, height, obstacleLayers)) // Check for ceilings
        {
            // Adjust the camera's position downward to avoid the ceiling
            desiredPosition.y = hit.point.y - wallOffset;

            Debug.Log($"Ceiling detected at {hit.point}. Adjusting camera to {desiredPosition}.");
        }

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // Make the camera look at the target
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }

    void HandleRotation()
    {
        // Mouse or Touch Input
        if (Input.GetMouseButtonDown(0)) // Start dragging
        {
            isDragging = true;
            lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Stop dragging
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            lastTouchPosition = Input.mousePosition;

            float rotateY = delta.x * rotationSpeed * Time.deltaTime; // Left/Right swipe (Yaw)
            float rotateX = -delta.y * rotationSpeed * Time.deltaTime; // Up/Down swipe (Pitch)

            yaw += rotateY;
            pitch = Mathf.Clamp(pitch + rotateX, minPitch, maxPitch); // Limit vertical movement
        }
    }
}