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

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            m_Camera.gameObject.SetActive(true);
            Debug.Log($"IsOwner: {IsOwner}");
        }
    }

}
