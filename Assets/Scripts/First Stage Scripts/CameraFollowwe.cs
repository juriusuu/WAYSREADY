using UnityEngine;
using System.Collections;
using Supercyan.FreeSample;
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
    private Vector3 cachedDesiredPosition;
    private bool wallCheckNeeded = true;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private float yaw = 0f; // Horizontal rotation (left/right)
    private float pitch = 20f; // Vertical rotation (up/down)
    private Vector3 currentVelocity = Vector3.zero;
    private float smoothedY;

    private Vector3 originalLocalPosition;
    private Coroutine shakeCoroutine;

    public Joystickscript playerScript; // Assign in Inspector
    void Awake()
    {
        if (target != null)
            smoothedY = target.position.y;

        //update

        originalLocalPosition = transform.localPosition;

        StartCoroutine(WallCheckRoutine());
    }
    private IEnumerator WallCheckRoutine()
    {
        while (true)
        {
            wallCheckNeeded = true;
            yield return new WaitForSeconds(0.1f); // Adjust as needed
        }
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


    /* 
        void LateUpdate()
        {
            if (target == null) return;

            HandleRotation(); // Handle swipe input

            // Only update smoothedY quickly if grounded, otherwise use slower smoothing
            float lerpSpeed = (playerScript != null && playerScript.IsGrounded()) ? smoothSpeed : smoothSpeed * 0.3f;

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
        } */
    /* 
        void LateUpdate()
        {
            if (target == null) return;

            HandleRotation(); // Handle swipe input

            // Lock camera Y to a fixed height above the target's starting Y (or a set value)
            float fixedY = target.position.y + height; // Or use a constant, e.g., float fixedY = 2.0f;

            // Calculate desired camera position (XZ follows player, Y is fixed)
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
            Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
            desiredPosition.y = fixedY; // Always use fixed Y

            // Prevent camera from clipping into walls
            RaycastHit hit;
            Vector3 wallCheckOrigin = new Vector3(target.position.x, fixedY, target.position.z);
            if (Physics.Linecast(wallCheckOrigin, desiredPosition, out hit, obstacleLayers))
            {
                desiredPosition = hit.point + hit.normal * wallOffset;
            }

            // Smoothly move the camera to the new position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

            // Make the camera look at the player (XZ only, Y is fixed)
            Vector3 lookAtPoint = new Vector3(target.position.x, fixedY * 0.5f, target.position.z);
            transform.LookAt(lookAtPoint);
        } */
    /*     void LateUpdate()
        {
            if (target == null) return;

            HandleRotation(); // Handle swipe input

            // Lock camera Y to a constant world height (e.g., 2.0f)
            float fixedY = height; // Just use the height value as world Y

            // Calculate desired camera position (XZ follows player, Y is fixed)
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
            Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
            desiredPosition.y = fixedY; // Always use fixed Y

            // Prevent camera from clipping into walls
            RaycastHit hit;
            Vector3 wallCheckOrigin = new Vector3(target.position.x, fixedY, target.position.z);
            if (Physics.Linecast(wallCheckOrigin, desiredPosition, out hit, obstacleLayers))
            {
                desiredPosition = hit.point + hit.normal * wallOffset;
            }

            // Smoothly move the camera to the new position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

            // Make the camera look at the player (XZ only, Y is fixed)
            Vector3 lookAtPoint = new Vector3(target.position.x, fixedY * 0.5f, target.position.z);
            transform.LookAt(lookAtPoint);
        } */
    /*   void LateUpdate()
      {
          if (target == null) return;

          HandleRotation(); // Handle swipe input

          // Determine if the player is grounded
          bool isGrounded = playerScript != null && playerScript.IsGrounded();

          // Smoothly follow the player's Y when grounded, otherwise keep last Y
          if (isGrounded)
          {
              smoothedY = Mathf.Lerp(smoothedY, target.position.y, smoothSpeed);
          }
          // else: keep smoothedY as is (camera doesn't follow jump)

          // Calculate desired camera position
          Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
          Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
          Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
          desiredPosition.y = smoothedY + height;

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
      } */
    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation();

        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (scene == "Stage2Easy" || scene == "Stage2Normal" || scene == "Stage2Hard")
        {
            // Follow Y only when grounded
            bool isGrounded = playerScript != null && playerScript.IsGrounded();
            if (isGrounded)
            {
                smoothedY = Mathf.Lerp(smoothedY, target.position.y, smoothSpeed);
            }
            // else: keep smoothedY as is

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
            Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
            desiredPosition.y = smoothedY + height;

            RaycastHit hit;
            Vector3 wallCheckOrigin = new Vector3(target.position.x, smoothedY + height, target.position.z);
            if (Physics.Linecast(wallCheckOrigin, desiredPosition, out hit, obstacleLayers))
            {
                desiredPosition = hit.point + hit.normal * wallOffset;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
            Vector3 lookAtPoint = new Vector3(target.position.x, smoothedY + height * 0.5f, target.position.z);
            transform.LookAt(lookAtPoint);
        }
        else
        {
            // Always smoothly follow the player's Y position (prevents flicker)
            float lerpSpeed = (playerScript != null && playerScript.IsGrounded()) ? smoothSpeed : smoothSpeed * 0.3f;

            smoothedY = Mathf.Lerp(smoothedY, target.position.y, smoothSpeed);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
            Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
            desiredPosition.y = smoothedY + height; // Use smoothed Y

            RaycastHit hit;
            Vector3 wallCheckOrigin = new Vector3(target.position.x, smoothedY + height, target.position.z);
            if (Physics.Linecast(wallCheckOrigin, desiredPosition, out hit, obstacleLayers))
            {
                desiredPosition = hit.point + hit.normal * wallOffset;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

            Vector3 lookAtPoint = new Vector3(target.position.x, smoothedY + height * 0.5f, target.position.z);
            transform.LookAt(lookAtPoint);
        }
    }

    /*    void LateUpdate()
       {
           if (target == null) return;

           HandleRotation();

           string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

           float lerpSpeed = (playerScript != null && playerScript.IsGrounded()) ? smoothSpeed : smoothSpeed * 0.3f;

           if (scene == "Stage2Easy" || scene == "Stage2Normal" || scene == "Stage2Hard")
           {
               // Follow Y only when grounded
               bool isGrounded = playerScript != null && playerScript.IsGrounded();
               if (isGrounded)
               {
                   smoothedY = Mathf.Lerp(smoothedY, target.position.y, smoothSpeed);
               }
               // else: keep smoothedY as is
           }
           else
           {
               // Always smoothly follow the player's Y position (prevents flicker)
               smoothedY = Mathf.Lerp(smoothedY, target.position.y, lerpSpeed);
           }

           Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
           Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
           Vector3 desiredPosition = targetXZ - (rotation * Vector3.forward * distance);
           desiredPosition.y = smoothedY + height;

           // Throttled wall avoidance for all scenes
           if (wallCheckNeeded)
           {
               RaycastHit hit;
               Vector3 wallCheckOrigin = new Vector3(target.position.x, smoothedY + height, target.position.z);
               if (Physics.Linecast(wallCheckOrigin, desiredPosition, out hit, obstacleLayers))
               {
                   desiredPosition = hit.point + hit.normal * wallOffset;
               }
               cachedDesiredPosition = desiredPosition;
               wallCheckNeeded = false;
           }
           else
           {
               desiredPosition = cachedDesiredPosition;
           }

           transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

           Vector3 lookAtPoint = new Vector3(target.position.x, smoothedY + height * 0.5f, target.position.z);
           transform.LookAt(lookAtPoint);
       } */
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

    // Call this method to start the shake
    public void ShakeCamera(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.localPosition;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = startPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;
    }
    public void TriggerEarthquake()
    {
        ShakeCamera(0.5f, 0.2f); // 0.5 seconds, 0.2 magnitude (adjust as needed)
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