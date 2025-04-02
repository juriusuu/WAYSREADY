using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

namespace Supercyan.FreeSample
{
    public class jJoystickscript : MonoBehaviour
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
                    // Uncomment this if you have a JumpController component
                    // m_jumpController.SetGrounded(m_isGrounded);
                    break;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            m_isGrounded = false;
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
        }

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
    }
}
