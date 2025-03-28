using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerControlScript : MonoBehaviour
{
    private Camera mainCam;
    private float moveSpeed = 3f;
    private bool isGrounded;

    [SerializeField] private InputActionReference moveActionToUse; // Reference to the input action for movement
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private float groundCheckDistance = 0.1f; // Distance to check for ground

    // Reference to the PickupButton script
    [SerializeField] private PickupButton pickupButton;
    private List<PickupItem> currentPickupItems = new List<PickupItem>(); // List to track current pickup items

    private void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Start the movement coroutine
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        while (true) // Loop indefinitely
        {
            // Check if the character is grounded
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

            // Get movement input
            Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
            Debug.Log("Move Direction: " + moveDirection);

            // Calculate movement based on joystick input
            Vector3 move = new Vector3(-moveDirection.x, 0, -moveDirection.y) * moveSpeed * Time.deltaTime;

            // Move the character relative to the camera's orientation
            Vector3 moveRelativeToCamera = mainCam.transform.TransformDirection(move);
            moveRelativeToCamera.y = 0; // Keep the movement on the horizontal plane

            // Update the character's position
            transform.position += moveRelativeToCamera;

            // Rotate the character to face the direction of movement
            if (moveRelativeToCamera != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveRelativeToCamera);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
            }

            Debug.Log("Character position: " + transform.position);

            yield return null; // Wait for the next frame
        }
    }

    private void OnEnable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Enable();
            Debug.Log("Move action enabled!");
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
    }

    private void OnDisable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Disable();
            Debug.Log("Move action disabled!");
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
    }

    // Detect pickup items when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a pickup item
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null)
        {
            // Add the pickup item to the list
            currentPickupItems.Add(pickupItem);
            pickupButton.SetPickupItem(pickupItem); // Set the current item in the PickupButton
            Debug.Log("Pickup item detected: " + pickupItem.ItemName); // Use the property here
        }
    }

    // Clear the current item when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        // Check if the object is a pickup item
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null)
        {
            // Remove the pickup item from the list
            currentPickupItems.Remove(pickupItem);
            if (currentPickupItems.Count > 0)
            {
                // Set the last item in the list as the current pickup item
                pickupButton.SetPickupItem(currentPickupItems[currentPickupItems.Count - 1]);
            }
            else
            {
                // Clear the PickupButton if no items are left
                pickupButton.SetPickupItem(null);
            }
            Debug.Log("Pickup item exited: " + pickupItem.ItemName); // Use the property here
        }
    }
}

// WITHOUT COROUTINE
/* public class PlayerControlScript : MonoBehaviour
{
    private Camera mainCam;
    private float moveSpeed = 3f;
    private bool isGrounded;

    [SerializeField] private InputActionReference moveActionToUse; // Reference to the input action for movement
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private float groundCheckDistance = 0.1f; // Distance to check for ground

    // Reference to the PickupButton script
    [SerializeField] private PickupButton pickupButton;
    private List<PickupItem> currentPickupItems = new List<PickupItem>(); // List to track current pickup items

    private void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }
    }

    private void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Get movement input
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        Debug.Log("Move Direction: " + moveDirection);

        // Calculate movement based on joystick input
        Vector3 move = new Vector3(-moveDirection.x, 0, -moveDirection.y) * moveSpeed * Time.deltaTime;

        // Move the character relative to the camera's orientation
        Vector3 moveRelativeToCamera = mainCam.transform.TransformDirection(move);
        moveRelativeToCamera.y = 0; // Keep the movement on the horizontal plane

        // Update the character's position
        transform.position += moveRelativeToCamera;

        // Rotate the character to face the direction of movement
        if (moveRelativeToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveRelativeToCamera);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
        }

        Debug.Log("Character position: " + transform.position);
    }

    private void OnEnable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Enable();
            Debug.Log("Move action enabled!");
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
    }

    private void OnDisable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Disable();
            Debug.Log("Move action disabled!");
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
    }

    // Detect pickup items when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a pickup item
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null)
        {
            // Add the pickup item to the list
            currentPickupItems.Add(pickupItem);
            pickupButton.SetPickupItem(pickupItem); // Set the current item in the PickupButton
            Debug.Log("Pickup item detected: " + pickupItem.ItemName); // Use the property here
        }
    }

    // Clear the current item when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        // Check if the object is a pickup item
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null)
        {
            // Remove the pickup item from the list
            currentPickupItems.Remove(pickupItem);
            if (currentPickupItems.Count > 0)
            {
                // Set the last item in the list as the current pickup item
                pickupButton.SetPickupItem(currentPickupItems[currentPickupItems.Count - 1]);
            }
            else
            {
                // Clear the PickupButton if no items are left
                pickupButton.SetPickupItem(null);
            }
            Debug.Log("Pickup item exited: " + pickupItem.ItemName); // Use the property here
        }
    }
}
*/
/*
public class PlayerControlScript : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 offset;
    private float maxLeft;
    private float maxRight;

    private float maxUp;

    private float maxDown;

    [SerializeField] private InputActionReference moveActionToUse;

    private float minX = -10f;
    private float maxX = 10f;
    private float minY = -10f;
    private float maxY = 10f;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        StartCoroutine(SetBoundaries());
        Debug.Log("PlayerControlScript started");
    }

    void LateUpdate()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        Debug.Log("Move Direction: " + moveDirection);

        // Scale up moveDirection values
        float newX = Mathf.Clamp(transform.position.x + moveDirection.x * 0.1f, minX, maxX);
        float newY = Mathf.Clamp(transform.position.y + moveDirection.y * 0.1f, minY, maxY);

        // Keep the z position the same
        transform.position = new Vector3(newX, newY, transform.position.z);

        Debug.Log("Character position: " + transform.position);
    }

    void OnEnable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Enable();
            Debug.Log("Move action enabled!");
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
    }

    void OnDisable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Disable();
            Debug.Log("Move action disabled!");
            Debug.Log("moveActionToUse: " + moveActionToUse);
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
    }

    private IEnumerator SetBoundaries()
    {
        yield return new WaitForEndOfFrame();

        offset = new Vector3(0f, 0f, -mainCam.transform.position.z);
        Vector3 min = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, mainCam.transform.position.z));
        Vector3 max = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCam.transform.position.z));

        maxLeft = min.x;
        maxRight = max.x;
        maxDown = min.y;
        maxUp = max.y;

        Debug.LogFormat(" {1} {2} {4}", " boundaries", " left boundary ", min.x, "right boundary", max.x, "down boundary", min.y, "up boundary", max.y);
    }
}
*/
/*
public class PlayerControlScript : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 offset;

    private float maxLeft;
    private float maxRight;

    private float maxUp;

    private float maxDown;

    [SerializeField] private InputActionReference moveActionToUse;
    //[SerializeField] private float speed = 3f;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        StartCoroutine(SetBoundaries());
        Debug.Log("PlayerControlScript started");
    }

    void LateUpdate()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        Debug.Log("Move Direction: " + moveDirection);

        // Scale up moveDirection values
        float newX = transform.position.x + moveDirection.x * .10f;
        float newY = transform.position.y + moveDirection.y * .10f;

        Debug.Log("New X: " + newX);
        Debug.Log("New Y: " + newY);

        // Assign new position directly without clamping
        transform.position = new Vector3(newX, newY, transform.position.z);

        Debug.Log("Character position: " + transform.position);
    }

    void OnEnable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Enable();
            Debug.Log("Move action enabled!");
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
    }

    void OnDisable()
    {
        if (moveActionToUse != null)
        {
            moveActionToUse.action.Disable();
            Debug.Log("Move action disabled!");
            Debug.Log("moveActionToUse: " + moveActionToUse);
        }
        else
        {
            Debug.LogError("Move action reference is null!");
        }
#if false
// No code here now as per requirement of avoiding url and few other things in prompt.
#endif

    }

    private IEnumerator SetBoundaries()
    {
        yield return new WaitForEndOfFrame();

        offset = new Vector3(0f, 0f, -mainCam.transform.position.z);
        Vector3 min = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, mainCam.transform.position.z));
        Vector3 max = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCam.transform.position.z));

        maxLeft = min.x;
        maxRight = max.x;
        maxDown = min.y;
        maxUp = max.y;

        Debug.LogFormat(" {1} {2} {4}", " boundaries", " left boundary ", min.x, "right boundary", max.x, "down boundary", min.y, "up boundary", max.y);
    }
}

*/
/*
public class PlayerControlScript : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    private Vector2 moveInput; // Store the input from the joystick
    private CharacterController characterController; // Reference to the CharacterController

    private void Awake()
    {
        // Get the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    // This method is called when the player moves
    public void OnMove(InputAction.CallbackContext context)
    {
        // Read the input value as a Vector2
        moveInput = context.ReadValue<Vector2>();

        // Debugging: Log the move input to the console
        Debug.Log("Move Input: " + moveInput);
    }

    private void Update()
    {
        // Convert the input to a 3D direction
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection); // Align with camera direction
        moveDirection.y = 0; // Keep the movement on the ground plane
        moveDirection.Normalize(); // Normalize to prevent faster diagonal movement

        // Debugging: Check the move direction
        Debug.Log("Move Direction: " + moveDirection);

        // Move the player
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
*/

