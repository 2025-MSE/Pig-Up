/**
 * Owner: Dongjin Kuk
 */

using Unity.Netcode;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MSE.Core
{
    public class PlayerMovement : NetworkBehaviour
    {
        private CharacterController m_CharacterController;

        [SerializeField] private float m_MoveSpeed = 5f;
        [SerializeField] private float m_JumpHeight = 3f;
        [SerializeField] private float m_MouseSensitivity = 100f;
        private Vector2 m_Direction;
        private Vector2 m_MouseDelta;

        private float m_XRotation = 0f;
        private float m_YRotation = 0f;

        [SerializeField]
        private Transform m_RotTransform;

        private Vector3 m_Velocity = Vector3.zero;
        private bool m_JumpPressed = false;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (!IsOwner) return;

            float mouseX = m_MouseDelta.x * m_MouseSensitivity * Time.deltaTime;
            float mouseY = m_MouseDelta.y * m_MouseSensitivity * Time.deltaTime;

            m_XRotation -= mouseY;
            m_XRotation = Mathf.Clamp(m_XRotation, -90f, 90f);

            m_YRotation += mouseX;

            m_RotTransform.localRotation = Quaternion.Euler(m_XRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            Debug.Log($"IsGrounded: {m_CharacterController.isGrounded}");
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            if (m_CharacterController.isGrounded && m_Velocity.y < 0)
            {
                m_Velocity.y = 0f;
            }

            Vector3 moveDir = transform.right * m_Direction.x + transform.forward * m_Direction.y;
            moveDir.Normalize();

            Vector3 horiVel = moveDir * m_MoveSpeed;
            m_Velocity.x = horiVel.x;
            m_Velocity.z = horiVel.z;

            if (m_JumpPressed && m_CharacterController.isGrounded)
            {
                m_JumpPressed = false;
                Jump();
            }
            ApplyGravity();

            m_CharacterController.Move(m_Velocity * Time.fixedDeltaTime);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;

            m_Direction = context.ReadValue<Vector2>();

            if (m_Direction != null)
            {
                m_Direction.Normalize();
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            if (m_CharacterController.isGrounded) return;

            if (context.started || context.performed)
            {
                m_JumpPressed = true;
            }
        }

        public void OnRotation(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;

            m_MouseDelta = context.ReadValue<Vector2>();
        }

        private void ApplyGravity()
        {
            m_Velocity.y += -9.81f * Time.fixedDeltaTime;
        }

        private void Jump()
        {
            m_Velocity.y += Mathf.Sqrt(m_JumpHeight * -2.0f * -9.81f);
        }
    }
}
