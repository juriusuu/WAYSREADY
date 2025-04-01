using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Make sure to include this

namespace Supercyan.FreeSample
{
    public class CharacterIt : MonoBehaviour
    {
        private enum ControlMode
        {
            Direct
        }

        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Rigidbody m_rigidBody = null;

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;
        private bool m_jumpInput = false;

        private bool m_isGrounded;

        private List<Collider> m_collisions = new List<Collider>();

        [SerializeField] private PickupButton pickupButton;
        private List<PickupItem> currentPickupItems = new List<PickupItem>(); // List to track current pickup items


        public LifeManager lifeManager; // Reference to LifeManager

        // [SerializeField] private InputActionReference moveActionToUse;
        public TimerManager timerManager; // Reference to TimerManager

        private void Awake()
        {
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }

            // Freeze rotation on X and Z axes
            m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;


            // Find the LifeManager and TimerManager in the scene
            lifeManager = FindFirstObjectByType<LifeManager>();
            timerManager = FindFirstObjectByType<TimerManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the object is a pickup item
            PickupItem pickupItem = collision.gameObject.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                // Add the pickup item to the list
                currentPickupItems.Add(pickupItem);
                pickupButton.SetPickupItem(pickupItem); // Set the current item in the PickupButton
                pickupButton.gameObject.SetActive(true); // Ensure the button is active
                Debug.Log("Pickup item detected: " + pickupItem.ItemName);
            }

            // Existing ground detection logic
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = false;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true; break;
                }
            }

            if (validSurfaceNormal)
            {
                m_isGrounded = true;
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
            }
            else
            {
                if (m_collisions.Contains(collision.collider))
                {
                    m_collisions.Remove(collision.collider);
                }
                if (m_collisions.Count == 0) { m_isGrounded = false; }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            // Existing ground detection logic remains unchanged
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = false;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true; break;
                }
            }

            if (validSurfaceNormal)
            {
                m_isGrounded = true;
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
            }
            else
            {
                if (m_collisions.Contains(collision.collider))
                {
                    m_collisions.Remove(collision.collider);
                }
                if (m_collisions.Count == 0) { m_isGrounded = false; }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // Existing ground detection logic remains unchanged
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }

        /*         private void OnCollisionEnter(Collision collision)
                {
                    ContactPoint[] contactPoints = collision.contacts;
                    for (int i = 0; i < contactPoints.Length; i++)
                    {
                        if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                        {
                            if (!m_collisions.Contains(collision.collider))
                            {
                                m_collisions.Add(collision.collider);
                            }
                            m_isGrounded = true;
                        }
                    }
                } */
        /* 
                private void OnCollisionStay(Collision collision)
                {
                    ContactPoint[] contactPoints = collision.contacts;
                    bool validSurfaceNormal = false;
                    for (int i = 0; i < contactPoints.Length; i++)
                    {
                        if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                        {
                            validSurfaceNormal = true; break;
                        }
                    }

                    if (validSurfaceNormal)
                    {
                        m_isGrounded = true;
                        if (!m_collisions.Contains(collision.collider))
                        {
                            m_collisions.Add(collision.collider);
                        }
                    }
                    else
                    {
                        if (m_collisions.Contains(collision.collider))
                        {
                            m_collisions.Remove(collision.collider);
                        }
                        if (m_collisions.Count == 0) { m_isGrounded = false; }
                    }
                }

                private void OnCollisionExit(Collision collision)
                {
                    if (m_collisions.Contains(collision.collider))
                    {
                        m_collisions.Remove(collision.collider);
                    }
                    if (m_collisions.Count == 0) { m_isGrounded = false; }
                }

         */

        private void Update()
        {
            // Check for jump input from the gamepad
            if (!m_jumpInput && Gamepad.current.buttonSouth.wasPressedThisFrame) // Typically the "A" button
            {
                m_jumpInput = true;
            }


            // Call the HandlePlayerDeath method
            if (transform.position.y < -5) // Adjust this value as needed
            {
                HandlePlayerDeath();
            }

        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            if (m_controlMode == ControlMode.Direct)
            {
                DirectUpdate();
            }
            else
            {
                Debug.LogError("Unsupported state");
            }

            m_wasGrounded = m_isGrounded;
            m_jumpInput = false;
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;
            if (Gamepad.current.leftShoulder.isPressed) // Use left shoulder button for walking
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            // Normalize the direction vector and set its Y component to 0
            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                // Rotate the character to face the movement direction
                transform.rotation = Quaternion.LookRotation(m_currentDirection);

                // Move the character using Rigidbody
                Vector3 movePosition = m_currentDirection * m_moveSpeed * Time.deltaTime;
                m_rigidBody.MovePosition(m_rigidBody.position + movePosition);

                // Set the animator's MoveSpeed parameter
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }

            JumpingAndLanding();
        }
        public void HandlePlayerDeath()
        {
            // Check if lifeManager is not null before calling Die
            if (lifeManager != null)
            {
                lifeManager.Die(); // Call the Die method in LifeManager
            }
            else
            {
                Debug.LogError("LifeManager is not found!");
                return; // Exit if lifeManager is null
            }

            // Check if InventoryManager is accessible
            if (InventoryManager.Instance == null)
            {
                Debug.LogError("InventoryManager is null before player destruction!");
            }

            // Check if the player has lives remaining
            if (GameeManager.Instance.currentLives > 0)
            {
                // Start the respawn coroutine in TimerManager
                timerManager.StartRespawnCoroutine(lifeManager);
            }
            else
            {
                // If no lives left, handle game over logic
                Debug.Log("No lives left. Game Over!");
                // You can add additional game over handling here if needed
            }

            // Optionally destroy the player object
            Destroy(gameObject);
        }
        /* 
                // Handle player death
                public void HandlePlayerDeath()
                {
                    // Check if lifeManager is not null before calling Die
                    if (lifeManager != null)
                    {
                        lifeManager.Die(); // Call the Die method in LifeManager
                    }
                    else
                    {
                        Debug.LogError("LifeManager is not found!");
                        return; // Exit if lifeManager is null
                    }

                    // Check if the player has lives remaining
                    if (GameManager.Instance.currentLives > 0)
                    {
                        // Start the respawn coroutine in TimerManager
                        timerManager.StartRespawnCoroutine(lifeManager);
                    }
                    else
                    {
                        // If no lives left, handle game over logic
                        Debug.Log("No lives left. Game Over!");
                        // You can add additional game over handling here if needed
                    }

                    // Optionally destroy the player object
                    Destroy(gameObject);
                }
         */
        private void JumpingAndLanding()
        {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_isGrounded && m_jumpInput)
            {
                m_jumpTimeStamp = Time.time;
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }
        }

        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>();
        }

        // Detect pickup items when entering the trigger
        /*         private void OnTriggerEnter(Collider other)
                {
                    // Check if the object is a pickup item
                    PickupItem pickupItem = other.GetComponent<PickupItem>();
                    if (pickupItem != null)
                    {
                        // Add the pickup item to the list
                        currentPickupItems.Add(pickupItem);
                        pickupButton.SetPickupItem(pickupItem); // Set the current item in the PickupButton
                        pickupButton.gameObject.SetActive(true); // Ensure the button is active
                        Debug.Log("Pickup item detected: " + pickupItem.ItemName);
                    }

                } */

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


}

/*    public void HandlePlayerDeath()
   {
       // Check if lifeManager is not null before calling Die
       if (lifeManager != null)
       {
           lifeManager.Die(); // Call the Die method in LifeManager
       }
       else
       {
           Debug.LogError("LifeManager is not found!");
           return; // Exit if lifeManager is null
       }

       // Check if the player has lives remaining
       if (GameManager.Instance.currentLives > 0)
       {
           // Optionally disable the player object instead of destroying it
           gameObject.SetActive(false); // Disable the player object

           // Start the respawn coroutine in TimerManager
           timerManager.StartRespawnCoroutine(this); // Pass the current instance to the coroutine
       }
       else
       {
           // If no lives left, handle game over logic
           Debug.Log("No lives left. Game Over!");
           // You can add additional game over handling here if needed
       }
   } */


/*
namespace Supercyan.FreeSample
{
    public class CharacterIt : MonoBehaviour
    {
        private enum ControlMode
        {
            Direct
        }

        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Rigidbody m_rigidBody = null;

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;
        private bool m_jumpInput = false;

        private bool m_isGrounded;

        private List<Collider> m_collisions = new List<Collider>();

        [SerializeField] private PickupButton pickupButton;
        private List<PickupItem> currentPickupItems = new List<PickupItem>(); // List to track current pickup items

        private void Awake()
        {
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }

            // Freeze rotation on X and Z axes
            m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    if (!m_collisions.Contains(collision.collider))
                    {
                        m_collisions.Add(collision.collider);
                    }
                    m_isGrounded = true;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = false;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true; break;
                }
            }

            if (validSurfaceNormal)
            {
                m_isGrounded = true;
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
            }
            else
            {
                if (m_collisions.Contains(collision.collider))
                {
                    m_collisions.Remove(collision.collider);
                }
                if (m_collisions.Count == 0) { m_isGrounded = false; }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }

        private void Update()
        {
            // Check for jump input from the gamepad
            if (!m_jumpInput && Gamepad.current.buttonSouth.wasPressedThisFrame) // Typically the "A" button
            {
                m_jumpInput = true;
            }
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            if (m_controlMode == ControlMode.Direct)
            {
                DirectUpdate();
            }
            else
            {
                Debug.LogError("Unsupported state");
            }

            m_wasGrounded = m_isGrounded;
            m_jumpInput = false;
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;
            if (Gamepad.current.leftShoulder.isPressed) // Use left shoulder button for walking
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            // Normalize the direction vector and set its Y component to 0
            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                // Rotate the character to face the movement direction
                transform.rotation = Quaternion.LookRotation(m_currentDirection);

                // Move the character using Rigidbody
                Vector3 movePosition = m_currentDirection * m_moveSpeed * Time.deltaTime;
                m_rigidBody.MovePosition(m_rigidBody.position + movePosition);

                // Set the animator's MoveSpeed parameter
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }

            JumpingAndLanding();
        }

        private void JumpingAndLanding()
        {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_isGrounded && m_jumpInput)
            {
                m_jumpTimeStamp = Time.time;
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }
        }

        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>();
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
}
*/
/*
namespace Supercyan.FreeSample
{
    public class CharacterIt : MonoBehaviour
    {
        private enum ControlMode
        {
            Tank,
            Direct
        }

        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Rigidbody m_rigidBody = null;

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;
        private readonly float m_backwardsWalkScale = 0.16f;
        private readonly float m_backwardRunScale = 0.66f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;
        private bool m_jumpInput = false;

        private bool m_isGrounded;

        private List<Collider> m_collisions = new List<Collider>();

        [SerializeField] private PickupButton pickupButton;
        private List<PickupItem> currentPickupItems = new List<PickupItem>(); // List to track current pickup items


        private void Awake()
        {
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    if (!m_collisions.Contains(collision.collider))
                    {
                        m_collisions.Add(collision.collider);
                    }
                    m_isGrounded = true;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = false;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true; break;
                }
            }

            if (validSurfaceNormal)
            {
                m_isGrounded = true;
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
            }
            else
            {
                if (m_collisions.Contains(collision.collider))
                {
                    m_collisions.Remove(collision.collider);
                }
                if (m_collisions.Count == 0) { m_isGrounded = false; }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }

        private void Update()
        {
            // Check for jump input from the gamepad
            if (!m_jumpInput && Gamepad.current.buttonSouth.wasPressedThisFrame) // Typically the "A" button
            {
                m_jumpInput = true;
            }
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }

            m_wasGrounded = m_isGrounded;
            m_jumpInput = false;
        }

        private void TankUpdate()
        {
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            bool walk = Gamepad.current.leftShoulder.isPressed; // Use left shoulder button for walking

            if (v < 0)
            {
                if (walk) { v *= m_backwardsWalkScale; }
                else { v *= m_backwardRunScale; }
            }
            else if (walk)
            {
                v *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            // Move the character
            transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
            transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

            // Set the animator's MoveSpeed parameter
            m_animator.SetFloat("MoveSpeed", m_currentV);

            JumpingAndLanding();
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

            if (Gamepad.current.leftShoulder.isPressed) // Use left shoulder button for walking
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            // Normalize the direction vector and set its Y component to 0
            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                // Rotate the character to face the movement direction
                transform.rotation = Quaternion.LookRotation(m_currentDirection);

                // Move the character using Rigidbody
                Vector3 movePosition = m_currentDirection * m_moveSpeed * Time.deltaTime;
                m_rigidBody.MovePosition(m_rigidBody.position + movePosition);

                // Set the animator's MoveSpeed parameter
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }

            JumpingAndLanding();
        }

        /*
                private void DirectUpdate()
                {
                    // Get input from the left joystick
                    float v = m_movementInput.y; // Up/Down
                    float h = m_movementInput.x; // Left/Right
                    Vector3 moveDirection = m_currentDirection * m_moveSpeed * Time.deltaTime;
                    m_rigidBody.MovePosition(m_rigidBody.position + moveDirection);

                    Transform camera = Camera.main.transform;

                    if (Gamepad.current.leftShoulder.isPressed) // Use left shoulder button for walking
                    {
                        v *= m_walkScale;
                        h *= m_walkScale;
                    }

                    m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
                    m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

                    Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

                    float directionLength = direction.magnitude;
                    direction.y = 0;
                    direction = direction.normalized * directionLength;

                    if (direction != Vector3.zero)
                    {
                        m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                        transform.rotation = Quaternion.LookRotation(m_currentDirection);
                        transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

                        m_animator.SetFloat("MoveSpeed", direction.magnitude);
                    }

                    JumpingAndLanding();
                }
                */
/*
        private void JumpingAndLanding()
        {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_isGrounded && m_jumpInput)
            {
                m_jumpTimeStamp = Time.time;
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }
        }

        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>();
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
}

*/