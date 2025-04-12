using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MSE.Core
{
    public class PlayerBuilding : NetworkBehaviour
    {
        [SerializeField]
        private NetworkObject m_BlockPrefab;

        private Vector3 m_CurrPos;

        public override void OnNetworkSpawn()
        {
            enabled = IsOwner;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10.0f))
            {
                Debug.DrawRay(transform.position, transform.forward, Color.yellow);
                m_CurrPos = hit.point;
            }
        }

        public void OnBuild(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                BuildRpc(m_CurrPos);
            }
        }

        [Rpc(SendTo.Server)]
        public void BuildRpc(Vector3 pos)
        {
            BuildFromServer(pos);
        }

        public void BuildFromServer(Vector3 pos, RpcParams rpcParams = default)
        {
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkPrefab: m_BlockPrefab,
                ownerClientId: rpcParams.Receive.SenderClientId,
                position: pos);
        }
    }
}

