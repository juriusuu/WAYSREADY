using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System
using System.Collections; // Required for IEnumerator (coroutines)
using System.Collections.Generic; // Required for List<>
using System.Linq; // Required for LINQ queries
using UnityEngine.SceneManagement; // Add this at the top if not already present

namespace Supercyan.FreeSample
{
    public class Joystickscript : MonoBehaviour
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
        public bool IsGrounded() => m_isGrounded;
        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.05f;
        private bool m_jumpInput = false;

        private bool m_isGrounded;

        private List<Collider> m_collisions = new List<Collider>();

        [SerializeField] private PickupButtonss pickupButtons;
        private List<PickupItems> currentPickupItems = new List<PickupItems>(); // List to track current pickup items

        [SerializeField] private float crouchHeight = 0.5f; // Height when crouching
        [SerializeField] private float crouchSpeed = 1f;    // Movement speed when crouching
        private float originalHeight;                      // Original height of the player
        private float originalSpeed;                       // Original movement speed
        private CapsuleCollider playerCollider;            // Reference to the player's collider
        private bool isCrouching = false;                  // Track crouch state
        public LifeManager lifeManager; // Reference to LifeManager

        // [SerializeField] private InputActionReference moveActionToUse;
        public TimerManager timerManager; // Reference to TimerManager
        [SerializeField]
        private float glideGravityMultiplier = 0.3f;
        private void Awake()
        {
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }

            // Freeze rotation on X and Z axes
            m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;


            // Find the LifeManager and TimerManager in the scene
            lifeManager = FindFirstObjectByType<LifeManager>();
            timerManager = FindFirstObjectByType<TimerManager>();

            // Store the original height and speed
            playerCollider = GetComponent<CapsuleCollider>();
            if (playerCollider != null)
            {
                originalHeight = playerCollider.height;
            }
            originalSpeed = m_moveSpeed;

        }
        public void Crouch()
        {
            if (!isCrouching)
            {
                isCrouching = true;
                if (m_animator != null)
                    m_animator.SetBool("isCrouching", true); // <-- Add this line

                // Adjust the visual model's position to represent crouching
                Transform visualModel = transform.Find("common_people_male_1");
                if (visualModel != null)
                {
                    // Move the visual model down by half the difference in height
                    float heightDifference = originalHeight - crouchHeight;
                    visualModel.localPosition = new Vector3(0, -heightDifference / 2, 0);
                }

                // Reduce the player's movement speed
                m_moveSpeed = crouchSpeed;

                Debug.Log("Player is crouching.");
            }
        }

        public void Stand()
        {
            if (isCrouching)
            {
                isCrouching = false;
                if (m_animator != null)
                    m_animator.SetBool("isCrouching", false); // <-- Add this line
                // Restore the visual model's position to represent standing
                Transform visualModel = transform.Find("common_people_male_1");
                if (visualModel != null)
                {
                    visualModel.localPosition = Vector3.zero; // Reset the visual model's position
                }

                // Restore the player's movement speed
                m_moveSpeed = originalSpeed;

                Debug.Log("Player is standing.");
            }
        }
        public void ToggleCrouch()
        {
            if (isCrouching)
            {
                Stand();
            }
            else
            {
                Crouch();
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            // Check if the object is a pickup item
            PickupItems pickupItems = collision.gameObject.GetComponent<PickupItems>();

            // Check if the object is a pickup item
            if (pickupItems != null)
            {
                // Check if pickupButtons is assigned
                if (pickupButtons == null)
                {
                    Debug.Log("pickupButtons is not assigned!");
                    return; // Exit if pickupButtons is null
                }

                // Add the pickup item to the list
                currentPickupItems.Add(pickupItems);

                // Check if pickupItems is null before passing it
                if (pickupItems != null)
                {
                    pickupButtons.SetPickupItems(new List<PickupItems> { pickupItems }); // Pass only the current pickup item
                    pickupButtons.gameObject.SetActive(true); // Ensure the button is active
                    Debug.Log("Pickup item detected: " + pickupItems.name); // Log the name of the pickup item
                }
            }
            else
            {
                Debug.LogWarning("No PickupItems component found on the collided object.");
            }
            /*      if (pickupItems != null)
                 {
                     // Add the pickup item to the list
                     currentPickupItems.Add(pickupItems);

                     // Set the current items in the PickupButton
                     pickupButtons.SetPickupItems(pickupItems.GetPickupItems()
                         .Select(go => go.GetComponent<PickupItems>())
                         .Where(pi => pi != null)
                         .ToList()); // Convert GameObject list to PickupItems list
                     pickupButtons.gameObject.SetActive(true); // Ensure the button is active
                     Debug.Log("Pickup item detected: " + string.Join(", ", pickupItems.GetPickupItems().Select(item => item.name))); // Use item names
                 } */
            /* 
                        if (pickupItems != null)
                        {
                            // Add the pickup item to the list
                            currentPickupItems.Add(pickupItems);

                            // Set the current items in the PickupButton
                            pickupButtons.SetPickupItems(pickupItems.Items); // Pass the list of PickupItem
                            pickupButtons.gameObject.SetActive(true); // Ensure the button is active
                            Debug.Log("Pickup item detected: " + string.Join(", ", pickupItems.Items.Select(item => item.ItemName)));
                        } */
            /*  if (pickupItems != null)
             {
                 // Add the pickup item to the list
                 currentPickupItems.Add(pickupItems);
                 pickupButtons.SetPickupItems(pickupItems); // Set the current item in the PickupButton
                 pickupButtons.gameObject.SetActive(true); // Ensure the button is active
                 Debug.Log("Pickup item detected: " + pickupItems.ItemNames);
             } */

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

            // --- PUSH ANIMATION LOGIC ---
            if (collision.gameObject.CompareTag("Ball"))
            {
                // Optional: Only trigger if player is moving
                if (m_movementInput.magnitude > 0.1f)
                {
                    m_animator.SetBool("isPushing", true);
                }
                else
                {
                    m_animator.SetBool("isPushing", false);
                }
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
            // Stop push animation when leaving the ball
            if (collision.gameObject.CompareTag("Ball"))
            {
                m_animator.SetBool("isPushing", false);
            }
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


            /*     // Call the HandlePlayerDeath method
                if (transform.position.y < -5) // Adjust this value as needed
                {
                    //  HandlePlayerDeath();
                    m_rigidBody.linearVelocity = Vector3.zero; // Stop any movement
                    transform.position = new Vector3(0, 0.5f, 0);
                    Debug.Log("Player fell and was reset to (0, 0, 0)"); */
            // Snap to ground if possible
            /*             RaycastHit hit;
                        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 2f))
                        {
                            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                        }

                        Debug.Log("Player fell and was reset to (0, 0, 0)"); */
            //  }
            // ...existing code...
            /*          if (transform.position.y < -5) // Adjust this value as needed
                     {
                         m_rigidBody.linearVelocity = Vector3.zero; // Stop any movement

                         // Set to respawn position
                         Vector3 respawnPosition = new Vector3(0, 2f, 0); // Start a bit higher
                         transform.position = respawnPosition;

                         // Snap to ground if possible
                         RaycastHit hit;
                         if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
                         {
                             transform.position = new Vector3(transform.position.x, hit.point.y + 0.01f, transform.position.z);
                             Debug.Log("Player snapped to ground at: " + transform.position);
                         }
                         else
                         {
                             Debug.LogWarning("No ground found below respawn point!");
                         }
                     }
          */           // Check if the player is falling below a certain height
            /*   if (transform.position.y < -5f) // Adjust this value as needed
              {
                  m_rigidBody.velocity = Vector3.zero; // Stop any movement
                  transform.position = new Vector3(0, 2f, 0); // Reset position to a safe point
                  Debug.Log("Player fell and was reset to (0, 2, 0)");
              } */

            /*        // Only run respawn logic in Stage 2 Normal and Stage 3
                   string sceneName = SceneManager.GetActiveScene().name;
                   if ((sceneName == "Stage2Normal" || sceneName == "Stage2Hard") && transform.position.y < -5)
                   {
                       m_rigidBody.linearVelocity = Vector3.zero;
                       Vector3 respawnPosition = new Vector3(0, 0, 0); // Use your ground's X/Z
                       transform.position = respawnPosition;
                       m_rigidBody.isKinematic = false;
                       m_rigidBody.useGravity = true;
                       StartCoroutine(SnapToGroundAfterDelay());
                   }
            */
            /*    string sceneName = SceneManager.GetActiveScene().name;
               if ((sceneName == "Stage2Normal" || sceneName == "Stage2Hard") && transform.position.y < -5)
               {
                   m_rigidBody.linearVelocity = Vector3.zero;

                   // Set your desired respawn X/Z here
                   Vector3 respawnXZ = new Vector3(0, 0, 0);

                   // Get CapsuleCollider and calculate feet offset
                   CapsuleCollider cc = GetComponent<CapsuleCollider>();
                   float feetOffset = 0f;
                   if (cc != null)
                   {
                       feetOffset = cc.center.y - (cc.height / 2f);
                   }

                   // Raycast from above to find the ground
                   RaycastHit hit;
                   if (Physics.Raycast(respawnXZ + Vector3.up * 100f, Vector3.down, out hit, 200f))
                   {
                       // Place the feet just above the ground
                       transform.position = hit.point + Vector3.up * -feetOffset + Vector3.up * 0.01f;
                   }
                   else
                   {
                       // Fallback if no ground found
                       transform.position = respawnXZ + Vector3.up * 1f;
                   }

                   m_rigidBody.isKinematic = false;
                   m_rigidBody.useGravity = true;
               } */
            string sceneName = SceneManager.GetActiveScene().name;
            if ((sceneName == "Stage2Normal" || sceneName == "Stage2Hard") && transform.position.y < -5)
            {
                // Stop any movement
                m_rigidBody.linearVelocity = Vector3.zero;
                m_rigidBody.angularVelocity = Vector3.zero;

                // Set your desired respawn X/Z here
                Vector3 respawnXZ = new Vector3(0, 0, 0);

                // Get CapsuleCollider and calculate feet offset
                CapsuleCollider cc = GetComponent<CapsuleCollider>();
                float feetOffset = 0f;
                if (cc != null)
                {
                    feetOffset = cc.center.y - (cc.height / 2f);
                }

                // Raycast from above to find the ground
                RaycastHit hit;
                if (Physics.Raycast(respawnXZ + Vector3.up * 100f, Vector3.down, out hit, 200f))
                {
                    // Place the feet just above the ground
                    transform.position = hit.point + Vector3.up * -feetOffset + Vector3.up * 0.01f;
                }
                else
                {
                    // Fallback if no ground found
                    transform.position = respawnXZ + Vector3.up * (cc != null ? cc.height / 2f + 0.01f : 1f);
                }

                // Force physics update (important for mobile)
                Physics.SyncTransforms();

                m_rigidBody.isKinematic = false;
                m_rigidBody.useGravity = true;
            }
        }
        private IEnumerator SnapToGroundAfterDelay()
        {
            float rayDistance = 100f;
            int maxTries = 10;
            int tries = 0;
            RaycastHit hit;

            while (tries < maxTries)
            {
                yield return new WaitForSeconds(0.1f);
                Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red, 2f);

                if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
                {
                    Debug.Log("Raycast hit: " + hit.collider.name + " at " + hit.point);
                    transform.position = new Vector3(transform.position.x, hit.point.y + 0.01f, transform.position.z);
                    yield break;
                }
                else
                {
                    Debug.LogWarning("No ground found below respawn point! Try: " + tries);
                }
                tries++;
            }
            Debug.LogError("Failed to find ground after respawn!");
        }
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] private float sprintHoldTime = 1f; // Seconds to hold joystick for sprint
        private float joystickHoldTimer = 0f;
        private bool isSprinting = false;
        /*         private void FixedUpdate()
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
                    // --- GLIDE LOGIC START ---
                    if (!m_isGrounded && m_rigidBody.linearVelocity.y < 0)
                    {
                        // Reduce the effect of gravity while falling
                        Vector3 velocity = m_rigidBody.linearVelocity;
                        velocity.y *= glideGravityMultiplier;
                        m_rigidBody.linearVelocity = velocity;
                    }
                    m_wasGrounded = m_isGrounded;
                    m_jumpInput = false;
                } */
        private void FixedUpdate()
        {
            if (isStunned)
            {
                m_animator.SetFloat("MoveSpeed", 0);
                return; // Skip movement while stunned
            }
            // --- RAYCAST GROUND CHECK ---
            float rayLength = 0.4f; // Adjust as needed for your character's size
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Slightly above feet
            m_isGrounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength);

            m_animator.SetBool("Grounded", m_isGrounded);

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue();

            // --- AUTO SPRINT LOGIC ---
            if (m_movementInput.magnitude > 0.95f) // Joystick fully tilted
            {
                joystickHoldTimer += Time.fixedDeltaTime;
                if (!isSprinting && joystickHoldTimer >= sprintHoldTime)
                {
                    StartSprinting();
                }
            }
            else
            {
                joystickHoldTimer = 0f;
                if (isSprinting)
                {
                    StopSprinting();
                }
            }

            if (m_controlMode == ControlMode.Direct)
            {
                DirectUpdate();
            }
            else
            {
                Debug.LogError("Unsupported state");
            }

            // --- GLIDE LOGIC START ---
            if (!m_isGrounded && m_rigidBody.linearVelocity.y < 0)
            {
                Vector3 velocity = m_rigidBody.linearVelocity;
                velocity.y *= glideGravityMultiplier;
                m_rigidBody.linearVelocity = velocity;
            }
            m_wasGrounded = m_isGrounded;
            //    m_jumpInput = false;
        }

        private void StartSprinting()
        {
            isSprinting = true;
            m_moveSpeed = originalSpeed * sprintMultiplier;
            Debug.Log("Sprinting!");
        }

        private void StopSprinting()
        {
            isSprinting = false;
            m_moveSpeed = originalSpeed;
            Debug.Log("Stopped sprinting.");
        }
        public void Jump()
        {
            // Trigger the jump input when the button is pressed
            if (m_isGrounded && (Time.time - m_jumpTimeStamp) >= m_minJumpInterval)
            {
                m_jumpTimeStamp = Time.time;
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
                Debug.Log("Jump triggered by button!");
            }
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

        /*  public void HandlePlayerDeath()
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
         } */


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
                m_jumpInput = false; // <-- Reset here, only after jump is triggered
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
        /*  private void OnTriggerExit(Collider other)
         {
             // Check if the object is a pickup item
             PickupItems pickupItems = other.GetComponent<PickupItems>();
             if (pickupItems != null)
             {
                 // Remove the pickup item from the list
                 currentPickupItems.Remove(pickupItems);
                 if (currentPickupItems.Count > 0)
                 {
                     // Set the last item in the list as the current pickup item
                     pickupButtons.SetPickupItems(currentPickupItems[currentPickupItems.Count - 1]);
                 }
                 else
                 {
                     // Clear the PickupButton if no items are left
                     pickupButtons.SetPickupItems(null);
                 }
                 Debug.Log("Pickup item exited: " + pickupItems.ItemNames); // Use the property here
             }
         }
     }

  *//* 
        private void OnTriggerExit(Collider other)
        {
            // Check if the object is a pickup item
            PickupItems pickupItems = other.GetComponent<PickupItems>();
            if (pickupItems != null)
            {
                // Remove the pickup item from the list
                currentPickupItems.Remove(pickupItems);

                if (currentPickupItems.Count > 0)
                {
                    // Set the last item's PickupItem list in the PickupButton
                    pickupButtons.SetPickupItems(currentPickupItems[currentPickupItems.Count - 1].GetPickupItems()
                        .Select(go => go.GetComponent<PickupItems>()).Where(pi => pi != null).ToList()); // Convert GameObject list to PickupItems list
                }
                else
                {
                    // Clear the PickupButton if no items are left
                    pickupButtons.SetPickupItems(null);
                }

                Debug.Log("Pickup item exited: " + string.Join(", ", pickupItems.GetPickupItems().Select(item => item.GetComponent<PickupItems>()?.ToString()))); // Replace ItemName with ToString() or another valid property
            }
        } */
        private void OnTriggerExit(Collider other)
        {
            // Check if the object is a pickup item
            PickupItems pickupItems = other.GetComponent<PickupItems>();
            if (pickupItems != null)
            {
                // Remove the pickup item from the list
                currentPickupItems.Remove(pickupItems);

                if (currentPickupItems.Count > 0)
                {
                    // Set the last item's PickupItem list in the PickupButton
                    pickupButtons.SetPickupItems(currentPickupItems[currentPickupItems.Count - 1].Items
                        .Select(go => go.GetComponent<PickupItems>())
                        .Where(pi => pi != null)
                        .ToList());
                }
                else
                {
                    // Clear the PickupButton if no items are left
                    pickupButtons.SetPickupItems(null);
                }

                Debug.Log("Pickup item exited: " + string.Join(", ", pickupItems.Items.Select(item => item.name)));
            }
        }
        private Coroutine slowCoroutine;

        // Call this to apply a slow effect
        public void ApplySlow(float slowMultiplier, float duration)
        {
            if (slowCoroutine != null)
                StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SlowRoutine(slowMultiplier, duration));
        }

        private IEnumerator SlowRoutine(float slowMultiplier, float duration)
        {
            float originalSpeed = m_moveSpeed;
            m_moveSpeed = originalSpeed * slowMultiplier;
            Debug.Log($"Player slowed! Speed is now {m_moveSpeed}");
            yield return new WaitForSeconds(duration);
            m_moveSpeed = originalSpeed;
            Debug.Log("Player speed restored!");
        }
        public void ApplySpeedBoost(float boostMultiplier, float duration)
        {
            StartCoroutine(SpeedBoostCoroutine(boostMultiplier, duration));
        }

        private System.Collections.IEnumerator SpeedBoostCoroutine(float boostMultiplier, float duration)
        {
            float originalSpeed = m_moveSpeed;
            m_moveSpeed *= boostMultiplier;
            Debug.Log("Speed boosted!");
            yield return new WaitForSeconds(duration);
            m_moveSpeed = originalSpeed;
            Debug.Log("Speed boost ended.");
        }

        private bool isStunned = false;

        public void Stun(float duration)
        {
            if (!isStunned)
                StartCoroutine(StunCoroutine(duration));
        }

        private IEnumerator StunCoroutine(float duration)
        {
            isStunned = true;
            // Optionally play a stun animation or effect here
            if (m_animator != null)
                m_animator.SetBool("isStunned", true); // If you have a stun animation
            yield return new WaitForSeconds(duration);
            isStunned = false;
            if (m_animator != null)
                m_animator.SetBool("isStunned", false);
        }

    }
}
// ...existing code...

/* 
namespace Supercyan.FreeSample
{
    public class Joystickscript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2f;
        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Rigidbody m_rigidBody = null;

        private Vector2 m_movementInput;
        private float m_currentV = 0f;
        private float m_currentH = 0f;
        private readonly float m_interpolation = 10f;
        private bool m_isGrounded;

        private void Awake()
        {
            if (!m_animator) { m_animator = GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = GetComponent<Rigidbody>(); }
        }
        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    m_isGrounded = true;
                    break;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // Only set grounded to false if the collision was with the ground
            if (collision.gameObject.CompareTag("Ground")) // Assuming your ground has a "Ground" tag
            {
                m_isGrounded = false;
            }
        }

        private void Update()
        {
            // Add any logic you need inside Update
        } // <-- Missing closing bracket added here

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded);
            m_movementInput = Gamepad.current.leftStick.ReadValue();

            DirectUpdate();
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");
        } */
/* 
        private void DirectUpdate()
        {
            float v = m_movementInput.y;
            float h = m_movementInput.x;

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            direction.y = 0;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                transform.position += direction * m_moveSpeed * Time.deltaTime;
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>();
        }
    } */
/*         private void DirectUpdate()
        {
            float v = m_movementInput.y;
            float h = m_movementInput.x;

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            direction.y = 0;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                // Instead of rotating the character to face the movement direction,
                // we can keep the character's rotation fixed or only allow Y-axis rotation.
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_interpolation);

                // Move the character
                transform.position += direction * m_moveSpeed * Time.deltaTime;
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
        }
    }
} */
/*         private void OnCollisionEnter(Collision collision)
              {
                  foreach (ContactPoint contact in collision.contacts)
                  {
                      if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                      {
                          m_isGrounded = true;
                          // Uncomment this if you have a JumpController component
                          // m_jumpController.SetGrounded(m_isGrounded);
                          break;
                      }
                  }
              }

              private void OnCollisionExit(Collision collision)
              {
                  m_isGrounded = false;
              } */
