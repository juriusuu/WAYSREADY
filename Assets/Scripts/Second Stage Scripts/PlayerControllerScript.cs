using UnityEngine;
using UnityEngine.InputSystem;

namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2f; // Movement speed
        [SerializeField] private Animator m_animator = null; // Animator reference
        [SerializeField] private Rigidbody m_rigidBody = null; // Rigidbody reference
        [SerializeField] private JumpController m_jumpController = null; // Reference to JumpController

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0f; // Current vertical input
        private float m_currentH = 0f; // Current horizontal input
        private readonly float m_interpolation = 10f; // Interpolation factor
        private bool m_isGrounded; // Is the player grounded?

        private LifeManager lifeManager; // Reference to LifeManager
        private TimerManager timerManager; // Reference to TimerManager
        private PanelManager panelManager; // Reference to PanelManager

        private void Awake()
        {
            // Get components
            if (!m_animator) { m_animator = GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = GetComponent<Rigidbody>(); }
            if (!m_jumpController) { m_jumpController = GetComponent<JumpController>(); }

            // Find the LifeManager, TimerManager, and PanelManager in the scene
            lifeManager = FindFirstObjectByType<LifeManager>();
            timerManager = FindFirstObjectByType<TimerManager>();
            panelManager = FindFirstObjectByType<PanelManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the player is grounded
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    m_isGrounded = true;
                    m_animator.SetBool("Grounded", m_isGrounded); // Update animator state
                    break; // Exit loop once grounded
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // Set grounded to false when the player leaves the ground
            m_isGrounded = false;
            m_animator.SetBool("Grounded", m_isGrounded); // Update animator state
        }

        private void Update()
        {
            // Check if the player is out of bounds
            if (transform.position.y < -5) // Adjust this value as needed
            {
                HandlePlayerDeath();
            }
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded); // Update animator state

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            // Direct control mode update
            DirectUpdate();

            // Debug log for player's position and grounded state
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            direction.y = 0; // Keep the direction flat
            direction.Normalize(); // Normalize the direction

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction); // Rotate player to face direction
                transform.position += direction * m_moveSpeed * Time.deltaTime; // Move player

                m_animator.SetFloat("MoveSpeed", direction.magnitude); // Update animator with movement speed
            }
        }

        // Handle player death
        private void HandlePlayerDeath()
        {
            // Display the fail panel using PanelManager
            if (panelManager != null)
            {
                panelManager.ShowFailPanel(); // Call the method to show the fail panel
            }
            else
            {
                Debug.LogError("PanelManager is not found!");
            }
        }
    }
}

/* using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Make sure to include this
using UnityEngine.InputSystem.Controls; // Make sure to include this

namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2f; // Movement speed
        [SerializeField] private Animator m_animator = null; // Animator reference
        [SerializeField] private Rigidbody m_rigidBody = null; // Rigidbody reference
        [SerializeField] private JumpController m_jumpController = null; // Reference to JumpController

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0f; // Current vertical input
        private float m_currentH = 0f; // Current horizontal input
        private readonly float m_interpolation = 10f; // Interpolation factor
        private bool m_isGrounded; // Is the player grounded?

        private LifeManager lifeManager; // Reference to LifeManager
        private TimerManager timerManager; // Reference to TimerManager

        private void Awake()
        {
            // Get components
            if (!m_animator) { m_animator = GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = GetComponent<Rigidbody>(); }
            if (!m_jumpController) { m_jumpController = GetComponent<JumpController>(); }

            // Find the LifeManager and TimerManager in the scene
            lifeManager = FindFirstObjectByType<LifeManager>();
            timerManager = FindFirstObjectByType<TimerManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the player is grounded
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    m_isGrounded = true;
                    m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
                    break; // Exit loop once grounded
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            m_isGrounded = false;
            m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
        }

        private void Update()
        {
            // Check if the player is out of bounds
            if (transform.position.y < -5) // Adjust this value as needed
            {
                HandlePlayerDeath();
            }

            // Update jump input in JumpController
            m_jumpController.PerformJump();
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded); // Update animator state

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            // Direct control mode update
            DirectUpdate();

            // Debug log for player's position and grounded state
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            direction.y = 0; // Keep the direction flat
            direction.Normalize(); // Normalize the direction

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction); // Rotate player to face direction
                transform.position += direction * m_moveSpeed * Time.deltaTime; // Move player

                m_animator.SetFloat("MoveSpeed", direction.magnitude); // Update animator with movement speed
            }
        }

        // Handle player death
        private void HandlePlayerDeath()
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


        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>(); // Update movement input
        }
    }
} */
/*
namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2f; // Movement speed
        [SerializeField] private Animator m_animator = null; // Animator reference
        [SerializeField] private Rigidbody m_rigidBody = null; // Rigidbody reference
        [SerializeField] private JumpController m_jumpController = null; // Reference to JumpController

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0f; // Current vertical input
        private float m_currentH = 0f; // Current horizontal input
        private readonly float m_interpolation = 10f; // Interpolation factor
        private bool m_isGrounded; // Is the player grounded?

        private LifeManager lifeManager; // Reference to LifeManager
        private TimerManager timerManager; // Reference to TimerManager

        private void Awake()
        {
            // Get components
            if (!m_animator) { m_animator = GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = GetComponent<Rigidbody>(); }
            if (!m_jumpController) { m_jumpController = GetComponent<JumpController>(); }

            // Find the LifeManager and TimerManager in the scene
            lifeManager = FindFirstObjectByType<LifeManager>();
            timerManager = FindFirstObjectByType<TimerManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the player is grounded
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    m_isGrounded = true;
                    m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
                    break; // Exit loop once grounded
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            m_isGrounded = false;
            m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
        }

        private void Update()
        {
            // Check if the player is out of bounds
            if (transform.position.y < -5) // Adjust this value as needed
            {
                     HandlePlayerDeath();
            }

            // Update jump input in JumpController
            m_jumpController.PerformJump();
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded); // Update animator state

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            // Direct control mode update
            DirectUpdate();

            // Debug log for player's position and grounded state
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            direction.y = 0; // Keep the direction flat
            direction.Normalize(); // Normalize the direction

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction); // Rotate player to face direction
                transform.position += direction * m_moveSpeed * Time.deltaTime; // Move player

                m_animator.SetFloat("MoveSpeed", direction.magnitude); // Update animator with movement speed
            }
        }
        

        // Handle player death
        private void HandlePlayerDeath()
        {
            // Call the Die method in LifeManager
            lifeManager.Die(); // Specify the correct method signature if there are overloads

            // Check if the player has lives remaining
            if (Supercyan.FreeSample.GameManager.Instance.currentLives > 0)
            {
                // Start the respawn coroutine in TimerManager
                timerManager.StartRespawnCoroutine((Supercyan.FreeSample.LifeManager)lifeManager);
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
        
        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>(); // Update movement input
        }
    }

    internal class GameManager
    {
    }
}

*/
/*
namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2f; // Movement speed
        [SerializeField] private Animator m_animator = null; // Animator reference
        [SerializeField] private Rigidbody m_rigidBody = null; // Rigidbody reference
        [SerializeField] private JumpController m_jumpController = null; // Reference to JumpController

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0f; // Current vertical input
        private float m_currentH = 0f; // Current horizontal input
        private readonly float m_interpolation = 10f; // Interpolation factor
        private bool m_isGrounded; // Is the player grounded?

        private LifeManager lifeManager; // Reference to LifeManager
        private TimerManager timerManager; // Reference to TimerManager

        private void Awake()
        {
            // Get components
            if (!m_animator) { m_animator = GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = GetComponent<Rigidbody>(); }
            if (!m_jumpController) { m_jumpController = GetComponent<JumpController>(); }

            // Find the LifeManager and TimerManager in the scene
            lifeManager = FindFirstObjectByType<LifeManager>();
            timerManager = FindFirstObjectByType<TimerManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the player is grounded
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    m_isGrounded = true;
                    m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
                    break; // Exit loop once grounded
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            m_isGrounded = false;
            m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
        }

        private void Update()
        {
            // Check if the player is out of bounds
            if (transform.position.y < -5) // Adjust this value as needed
            {
                HandlePlayerDeath();
            }

            // Update jump input in JumpController
            m_jumpController.PerformJump();
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded); // Update animator state

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            // Direct control mode update
            DirectUpdate();

            // Debug log for player's position and grounded state
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            direction.y = 0; // Keep the direction flat
            direction.Normalize(); // Normalize the direction

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction); // Rotate player to face direction
                transform.position += direction * m_moveSpeed * Time.deltaTime; // Move player

                m_animator.SetFloat("MoveSpeed", direction.magnitude); // Update animator with movement speed
            }
        }

        // Handle player death
        private void HandlePlayerDeath()
        {
            lifeManager.Die(); // Call the Die method in LifeManager
            timerManager.StartRespawnCoroutine(lifeManager); // Start the respawn coroutine in TimerManager
            Destroy(gameObject); // Optionally destroy the player object
        }

        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>(); // Update movement input
        }
    }
}

*/

/*
namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2;

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Rigidbody m_rigidBody = null;
        [SerializeField] private JumpController m_jumpController = null; // Reference to JumpController

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;

        private bool m_isGrounded;

        private List<Collider> m_collisions = new List<Collider>();

        private LifeManager lifeManager; // Reference to LifeManager
        private TimerManager timeManager; // Reference to TimeManager

        private void Awake()
        {
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }
            if (!m_jumpController) { m_jumpController = gameObject.GetComponent<JumpController>(); }

            // Find the LifeManager and TimeManager in the scene
            lifeManager = FindFirstObjectByType<LifeManager>();
            timeManager = FindFirstObjectByType<TimerManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    m_isGrounded = true;
                    m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            m_isGrounded = false;
            m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
        }

        private void Update()
        {
            // Check if the player is out of bounds
            if (transform.position.y < -5) // Adjust this value as needed
            {
                // Call the Die method in LifeManager
                lifeManager.Die();
                // Start the respawn coroutine in TimeManager
                timeManager.StartRespawnCoroutine(lifeManager);
                // Optionally destroy the player object
                Destroy(gameObject);
            }

            // Update jump input in JumpController
            m_jumpController.PerformJump();
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            // Direct control mode update
            DirectUpdate();

            // Debug log for player's position and grounded state
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                transform.position += direction * m_moveSpeed * Time.deltaTime;

                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
        }

        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>();
        }
    }
}
*/
/*
namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2;

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Rigidbody m_rigidBody = null;
        [SerializeField] private JumpController m_jumpController = null; // Reference to JumpController

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;

        private bool m_isGrounded;

        private List<Collider> m_collisions = new List<Collider>();

        private void Awake()
        {
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }
            if (!m_jumpController) { m_jumpController = gameObject.GetComponent<JumpController>(); }
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    m_isGrounded = true;
                    m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            m_isGrounded = false;
            m_jumpController.SetGrounded(m_isGrounded); // Set grounded state in JumpController
        }

        private void Update()
        {
            // No need to call PerformJump here, it's handled in JumpController
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            // Get input from the left joystick
            m_movementInput = Gamepad.current.leftStick.ReadValue(); // Read the left joystick input

            // Direct control mode update
            DirectUpdate();

            // Debug log for player's position and grounded state
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                transform.position += direction * m_moveSpeed * Time.deltaTime;

                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
        }

        // This method is called to set the movement input from the Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            m_movementInput = context.ReadValue<Vector2>();
        }

        // This method is called to handle jump input from the Input System
        public void OnJump(InputAction.CallbackContext context)
        {
            m_jumpController.OnJump(context); // Pass the jump input to JumpController
        }
    }
}
*/

/*

namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_jumpForce = 4;

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Rigidbody m_rigidBody = null;

        private Vector2 m_movementInput; // Store joystick input
        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;
        private bool m_jumpInput = false;

        private bool m_isGrounded;

        private List<Collider> m_collisions = new List<Collider>();

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

            // Direct control mode update
            DirectUpdate();

            // Debug log for player's position and grounded state
            Debug.Log($"Player Position: {transform.position}, Is Grounded: {m_isGrounded}");

            m_wasGrounded = m_isGrounded;
            m_jumpInput = false;
        }

        private void DirectUpdate()
        {
            // Get input from the left joystick
            float v = m_movementInput.y; // Up/Down
            float h = m_movementInput.x; // Left/Right

            Transform camera = Camera.main.transform;

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
    }
}
*/
/*
namespace Supercyan.FreeSample
{
    public class PlayerControllerScript : MonoBehaviour
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
    }
}

*/

