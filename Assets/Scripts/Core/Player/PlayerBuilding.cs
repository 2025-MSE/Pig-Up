using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MSE.Core
{
    public class PlayerBuilding : NetworkBehaviour
    {
        [SerializeField]
        private LayerMask m_LayerMask;

        int m_BlockIndex = 0;
        private Block m_BlockSilhoutte;

        [SerializeField]
        private float m_RotSpeed = 3f;

        private Vector3 m_CurrPos = new Vector3();

        public override void OnNetworkSpawn()
        {
        }

        private void Start()
        {
            if (!IsOwner) return;

            Cursor.lockState = CursorLockMode.Locked;
            SetBlock(DataManager.GetBlock(0));
        }

        private void Update()
        {
            if (!IsOwner) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5.0f, m_LayerMask))
            {
                m_CurrPos = hit.point;
                
                if (m_BlockSilhoutte)
                {
                    m_BlockSilhoutte.gameObject.SetActive(true);
                    m_BlockSilhoutte.transform.position = m_CurrPos;
                }
            }
            else
            {
                m_BlockSilhoutte?.gameObject.SetActive(false);
            }
        }

        public void OnBuild(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;

            if (context.performed)
            {
                BuildRpc(m_BlockIndex, m_CurrPos, m_BlockSilhoutte.transform.rotation);
            }
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;

            float scrollY = context.ReadValue<float>();
            
            if (m_BlockSilhoutte)
            {
                m_BlockSilhoutte.transform.Rotate(transform.up * scrollY * m_RotSpeed);
            }
        }

        public void SetBlock(Block block)
        {
            if (!IsOwner) return;

            m_BlockIndex = block.Index;
            m_BlockSilhoutte = Instantiate(DataManager.GetBlock(block.Index));

            MeshRenderer mrenderer = m_BlockSilhoutte.GetComponentInChildren<MeshRenderer>();
            foreach (Material mat in mrenderer.materials)
            {
                Color color = mat.color;
                color.a = 0.5f;
                mat.color = color;
            }

            m_BlockSilhoutte.ReadyToBuild();
        }

        [Rpc(SendTo.Server)]
        public void BuildRpc(int blockIndex, Vector3 pos, Quaternion rot, RpcParams rpcParams = default)
        {
            NetworkObject nobj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkPrefab: DataManager.GetBlock(blockIndex).GetComponent<NetworkObject>(),
                ownerClientId: rpcParams.Receive.SenderClientId,
                position: pos,
                rotation: rot);
            Block block = nobj.GetComponent<Block>();
            block.OnBuilt();
        }
    }
}

