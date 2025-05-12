using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class Building : NetworkBehaviour
    {
        [SerializeField]
        private Transform m_BlockRoot;
        private List<Block> m_Blocks = new List<Block>();
        public List<Block> Blocks => m_Blocks;

        public static Action<Building> OnSpawned;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            for (int i = 0; i < m_BlockRoot.childCount; i++)
            {
                Block block = m_BlockRoot.GetChild(i).GetComponent<Block>();
                int blockIndex = block.Index;

                NetworkObject nBlockObj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                    DataManager.GetBlock(blockIndex).GetComponent<NetworkObject>(),
                    position: block.transform.position,
                    rotation: block.transform.rotation);
                Block nBlock = nBlockObj.GetComponent<Block>();
                nBlock.SetStrategy(BlockStrategyType.IN_BUILDING);
                nBlock.SetInBuildingIndex(i);

                m_Blocks.Add(nBlock);

                Destroy(block.gameObject);
            }

            OnSpawned?.Invoke(this);
        }
    }
}
