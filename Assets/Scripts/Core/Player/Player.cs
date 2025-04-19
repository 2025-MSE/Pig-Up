/**
 * Owner: Dongjin Kuk
 */

using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MSE.Core
{
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private Camera m_Camera;

        private Animator m_Animator;
        public Animator Animator => m_Animator;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            m_Camera.gameObject.SetActive(true);
        }
    }

}
