using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace MSE.Core
{
    public class GameController : NetworkBehaviour
    {
        [SerializeField]
        private GameMap m_GameMap;

        private Building m_Building;

        public static Action<Building> OnBuildingSpawned;

        public override void OnNetworkSpawn()
        {
            OnBuildingSpawned += OnBuildingSpawn;
            NetworkObject nplayer = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject;
            nplayer.transform.position = m_GameMap.PlayerSpawnPoints[0].position;

            if (!IsOwner) return;

            GameEventCallbacks.OnBlockBuilt += OnBlockBuilt;

            CreateBuilding();
        }

        private void CreateBuilding()
        {
            StageData stageData = DataManager.GetStageData(0);
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkPrefab: stageData.BuildingPrefab.GetComponent<NetworkObject>(),
                position: m_GameMap.BuildingSpawnPoint.position,
                rotation: Quaternion.identity);
        }

        private void OnBuildingSpawn(Building building)
        {
            m_Building = building;
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
            var nobj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[netBlockId];
            var tblock = m_Building.Blocks[builtIndex];

            nobj.transform.DOMove(tblock.transform.position, 0.5f);
            nobj.transform.DORotate(tblock.transform.rotation.eulerAngles, 0.5f);
        }
    }
}