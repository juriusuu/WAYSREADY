using UnityEngine;
using UnityEngine.InputSystem;

namespace Supercyan.FreeSample
{
    public class JoystickscriptX : MonoBehaviour
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

        private void Awake()
        {
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }

            // Freeze rotation on X and Z axes
            m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
    }
}