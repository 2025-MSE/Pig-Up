using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class GameController : NetworkBehaviour
    {
        private Building m_Building;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            GameEventCallbacks.OnBlockBuilt += OnBlockBuilt;

            CreateBuilding();
        }

        private void CreateBuilding()
        {
            StageData stageData = DataManager.GetStageData(0);
            NetworkObject nbuildingObj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkPrefab: stageData.BuildingPrefab.GetComponent<NetworkObject>(),
                position: Vector3.zero,
                rotation: Quaternion.identity);
            m_Building = nbuildingObj.GetComponent<Building>();
            
            for (int i = 0; i < m_Building.Blocks.Count; i++)
            {
                Block block = m_Building.Blocks[i];
                block.ConfigBuilding();
                block.BuiltIndex = i;
            }
        }

        private void OnBlockBuilt(Block block, int[] builtIndice)
        {
            List<Block> builtBlocks = new List<Block>();
            foreach (int builtIndex in builtIndice)
            {
                builtBlocks.Add(m_Building.Blocks[builtIndex]);
            }

            ulong netBlockId = block.GetComponent<NetworkObject>().NetworkObjectId;

            if (builtIndice.Length > 0)
            {
                AdjustBlockRpc(netBlockId, builtBlocks[0].BuiltIndex);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void AdjustBlockRpc(ulong netBlockId, int builtIndex)
        {
            NetworkManager.Singleton.SpawnManager.SpawnedObjects[netBlockId].transform.localScale = Vector3.one * 1.5f;
            //m_Building.Blocks[builtIndex].gameObject.SetActive(false);
        }
    }
}