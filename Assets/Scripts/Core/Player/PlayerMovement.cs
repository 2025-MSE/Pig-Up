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
        [SerializeField] private float m_MouseSensitivity = 100f;
        private Vector2 m_Direction;
        private Vector2 m_MouseDelta;

        private float m_XRotation = 0f;
        private float m_YRotation = 0f;

        [SerializeField]
        private Transform m_RotTransform;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float mouseX = m_MouseDelta.x * m_MouseSensitivity * Time.deltaTime;
            float mouseY = m_MouseDelta.y * m_MouseSensitivity * Time.deltaTime;

            m_XRotation -= mouseY;
            m_XRotation = Mathf.Clamp(m_XRotation, -90f, 90f);

            m_YRotation += mouseX;

            m_RotTransform.localRotation = Quaternion.Euler(m_XRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void FixedUpdate()
        {
            Vector3 movePos = new Vector3(m_Direction.x, 0f, m_Direction.y) * m_MoveSpeed;
            m_CharacterController.Move(movePos * Time.fixedDeltaTime);
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

        public void OnRotation(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;

            m_MouseDelta = context.ReadValue<Vector2>();
        }
    }

}
