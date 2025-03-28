using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Include this to work with UI elements
using UnityEngine.InputSystem; // Include this to use the new Input System

public class JumpController : MonoBehaviour
{
    [SerializeField] private float m_jumpForce = 8;
    [SerializeField] private float m_minJumpInterval = 0.25f;

    private Rigidbody m_rigidBody;
    private bool m_isGrounded;
    private float m_jumpTimeStamp = 0;
    private bool m_jumpInput = false;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Perform jump if the jump input is set
        PerformJump();
    }

    public void SetGrounded(bool isGrounded)
    {
        m_isGrounded = isGrounded;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_jumpInput = true; // Set jump input when the jump button is pressed
        }
    }

    public void PerformJump()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && m_jumpInput)
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            m_jumpInput = false; // Reset jump input after jumping
        }
    }
}
/*
public class JumpController : MonoBehaviour
{
    public bool canJump;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Optionally, you can find the button and add a listener to it
        Button jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();
        jumpButton.onClick.AddListener(OnJumpButtonClicked);
    }

    private void Update()
    {
        // No need to check for key input here anymore
    }

    public void OnJumpButtonClicked()
    {
        Debug.Log("Jump button clicked");
        if (canJump)
        {
            Debug.Log("Jumping");
            rb.AddForce(Vector3.up * 4, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded");
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Not Grounded");
            canJump = false;
        }
    }
}
/*
/*
public class JumpController : MonoBehaviour
{
    public float jumpForce = 5f; // Adjust the jump force as needed
    public float groundCheckRadius = 0.1f; // Radius of the capsule ends
    public LayerMask groundLayer; // Layer mask to specify which layers are considered ground

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckIfGrounded()); // Start the coroutine to check grounded state
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Set isGrounded to false until the player lands
        }
    }
    private IEnumerator CheckIfGrounded()
    {
        while (true)
        {
            Vector3 bottom = GetCapsuleBottom();
            Vector3 top = GetCapsuleTop();

            // Check if the player is grounded using a capsule check
            isGrounded = Physics.CheckCapsule(bottom, top, groundCheckRadius, groundLayer);

            Debug.Log($"Bottom: {bottom}, Top: {top}, Is Grounded: {isGrounded}"); // Log the positions and grounded state
            yield return new WaitForSeconds(0.1f); // Wait for a short duration before checking again
        }
    }

    private Vector3 GetCapsuleBottom()
    {
        // Adjust the position based on the player's collider size
        return transform.position + new Vector3(0, -1.0f, 0); // Adjust the Y value based on your player's height
    }

    private Vector3 GetCapsuleTop()
    {
        // Adjust the position based on the player's collider size
        return transform.position + new Vector3(0, 0.5f, 0); // Adjust the Y value based on your player's height
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is colliding with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set isGrounded to true when touching the ground
            Debug.Log("Player is grounded.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the player is no longer touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Set isGrounded to false when leaving the ground
            Debug.Log("Player is no longer grounded.");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Prevent passing through the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the capsule check in the scene view
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GetCapsuleBottom(), GetCapsuleTop());
        Gizmos.DrawWireSphere(GetCapsuleBottom(), groundCheckRadius);
        Gizmos.DrawWireSphere(GetCapsuleTop(), groundCheckRadius);
    }
}

*/

/*
public class JumpController : MonoBehaviour
{
    public float jumpForce = 5f; // Adjust the jump force as needed
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckIfGrounded()); // Start the coroutine to check grounded state
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Set isGrounded to false until the player lands
        }
    }

    private IEnumerator CheckIfGrounded()
    {
        while (true)
        {
            // Check if the player is grounded using a raycast
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
            Debug.Log("Is Grounded: " + isGrounded); // Log the grounded state
            yield return new WaitForSeconds(0.1f); // Wait for a short duration before checking again
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is colliding with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set isGrounded to true when touching the ground
            Debug.Log("Player is grounded.");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Prevent passing through the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the raycast in the scene view
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);
    }
}

*/