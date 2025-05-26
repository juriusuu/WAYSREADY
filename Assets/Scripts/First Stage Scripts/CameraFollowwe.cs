using UnityEngine;
using UnityEngine;

public class CameraFollowe : MonoBehaviour
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
    private float smoothedY;

    void Awake()
    {
        if (target != null)
            smoothedY = target.position.y;
    }
    /*     void LateUpdate()
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
        } *//* 
    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation(); // Handle swipe input

        // Calculate desired camera position (XZ follows player, Y is fixed)
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
        Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
        desiredPosition.y = height; // Keep camera at fixed height

        // Prevent camera from clipping into walls
        RaycastHit hit;
        Vector3 wallCheckOrigin = new Vector3(target.position.x, height, target.position.z);
        if (Physics.Linecast(wallCheckOrigin, desiredPosition, out hit, obstacleLayers))
        {
            desiredPosition = hit.point + hit.normal * wallOffset;
        }

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // Make the camera look at the player (XZ only, Y is fixed)
        Vector3 lookAtPoint = new Vector3(target.position.x, height, target.position.z);
        transform.LookAt(lookAtPoint);
    } */


    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation(); // Handle swipe input

        // Smoothly follow the player's Y position (prevents flicker)
        smoothedY = Mathf.Lerp(smoothedY, target.position.y, smoothSpeed);

        // Calculate desired camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
        Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
        desiredPosition.y = smoothedY + height; // Use smoothed Y

        // Prevent camera from clipping into walls
        RaycastHit hit;
        Vector3 wallCheckOrigin = new Vector3(target.position.x, smoothedY + height, target.position.z);
        if (Physics.Linecast(wallCheckOrigin, desiredPosition, out hit, obstacleLayers))
        {
            desiredPosition = hit.point + hit.normal * wallOffset;
        }

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        // Make the camera look at the player (use smoothed Y)
        Vector3 lookAtPoint = new Vector3(target.position.x, smoothedY + height * 0.5f, target.position.z);
        transform.LookAt(lookAtPoint);
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
/* 
    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position in front of the target
            Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

            // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Update camera position
            transform.position = smoothedPosition;using UnityEngine;

public class CameraFollow : MonoBehaviour
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

            // Make the camera look at the target
            transform.LookAt(target);
        }
    }
} */