using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

namespace MSE.Core
{
    public class PlayerBuilding : NetworkBehaviour
    {
        [SerializeField] private LayerMask m_BuildLayerMask;
        [SerializeField] private LayerMask m_SelectLayerMask;

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
            SetBlock(DataManager.GetBlock(144));
        }

        private void Update()
        {
            if (!IsOwner) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 15f, m_BuildLayerMask))
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

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;

            if (context.performed)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 15f, m_SelectLayerMask))
                {
                    if (hit.collider.transform.parent.TryGetComponent(out Block block))
                    {
                        SetBlock(DataManager.GetBlock(block.Index));
                    }
                }

                if (Physics.Raycast(ray, out hit, 15f, m_BuildLayerMask))
                {
                    if (hit.collider.transform.parent.TryGetComponent(out Block block))
                    {
                        if (block.IsChecked()) return;

                        ulong nobjIndex = block.GetComponent<NetworkObject>().NetworkObjectId;
                        BreakRpc(nobjIndex);
                    }
                }
            }
        }

        public void OnBuild(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;

            if (context.performed)
            {
                BuildRpc(m_BlockIndex, m_CurrPos, m_BlockSilhoutte.transform.rotation, m_BlockSilhoutte.Detection.DetectedBuiltIndice.ToArray());
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

            if (m_BlockSilhoutte != null)
            {
                Destroy(m_BlockSilhoutte.gameObject);
            }

            m_BlockIndex = block.Index;
            m_BlockSilhoutte = Instantiate(DataManager.GetBlock(block.Index));
            m_BlockSilhoutte.ReadyToBuild();
        }

        [Rpc(SendTo.Server)]
        public void BuildRpc(int blockIndex, Vector3 pos, Quaternion rot, int[] builtIndice, RpcParams rpcParams = default)
        {
            NetworkObject nobj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkPrefab: DataManager.GetBlock(blockIndex).GetComponent<NetworkObject>(),
                ownerClientId: rpcParams.Receive.SenderClientId,
                position: pos,
                rotation: rot);
            Block block = nobj.GetComponent<Block>();
            block.OnBuilt();
            GameEventCallbacks.OnBlockBuilt?.Invoke(block, builtIndice);
        }

        [Rpc(SendTo.Server)]
        public void BreakRpc(ulong nobjIndex)
        {
            NetworkObject tnobj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[nobjIndex];
            tnobj.Despawn();
        }
    }
}

