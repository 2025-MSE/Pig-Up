using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class GameController : NetworkBehaviour
    {
        [SerializeField]
        private GameMap m_GameMap;
        private Building m_Building;

        private float m_StartTime = 0f;

        public static Action<Building> OnBuildingSpawned;

        [SerializeField]
        private UIStageResult m_ResultPanel;

        public override void OnNetworkSpawn()
        {
            OnBuildingSpawned += OnBuildingSpawn;

            m_StartTime = Time.time;

            NetworkObject nplayer = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject;
            nplayer.transform.position = m_GameMap.PlayerSpawnPoints[0].position;

            GameEventCallbacks.OnBlockBuilt += OnBlockBuilt;

            CreateBuilding();
        }

        public override void OnNetworkDespawn()
        {
            OnBuildingSpawned -= OnBuildingSpawn;
            GameEventCallbacks.OnBlockBuilt -= OnBlockBuilt;
        }

        private void CreateBuilding()
        {
            StageData stageData = DataManager.CurrStageData;
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
                block.ConfigBuildingRpc();
                block.BuiltIndex = i;
            }

            SpawnPartitionBlocksRpc();
        }

        [Rpc(SendTo.Server)]
        private void SpawnPartitionBlocksRpc()
        {
            SortedSet<int> spawnedBlockIndice = new SortedSet<int>();
            foreach (Block block in m_Building.Blocks)
            {
                spawnedBlockIndice.Add(block.Index);
            }

            List<int> spIndice = new List<int>();
            for (int i = 0; i < m_GameMap.PBlockSpawnPoints.Length; i++)
            {
                spIndice.Add(i);
            }
            spIndice.Shuffle();

            int index = 0;
            foreach (int sbIndex in spawnedBlockIndice)
            {
                Transform spawnpoint = m_GameMap.PBlockSpawnPoints[spIndice[index]];
                NetworkObject npbobj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                    networkPrefab: DataManager.GetBlock(sbIndex).GetComponent<NetworkObject>(),
                    position: spawnpoint.position,
                    rotation: spawnpoint.rotation);
                Block pblock = npbobj.GetComponent<Block>();
                pblock.ConfigPartitionRpc();

                index += 1;
            }
        }

        private void OnBlockBuilt(Block block, int[] builtIndice)
        {
            List<Block> builtBlocks = new List<Block>();
            foreach (int builtIndex in builtIndice)
            {
                if (m_Building.Blocks[builtIndex].IsChecked()) continue;
                builtBlocks.Add(m_Building.Blocks[builtIndex]);
            }

            if (builtBlocks.Count == 0) return;

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
            var nblock = nobj.GetComponent<Block>();
            var tblock = m_Building.Blocks[builtIndex];

            nobj.transform.DOMove(tblock.transform.position, 0.5f);
            nobj.transform.DORotate(tblock.transform.rotation.eulerAngles, 0.5f);

            nblock.SetChecked(true);
            tblock.SetChecked(true);
            CheckBuildingRpc();
        }

        [Rpc(SendTo.Server)]
        private void CheckBuildingRpc()
        {
            int checkCount = 0;
            foreach (Block block in m_Building.Blocks)
            {
                if (block.IsChecked())
                    checkCount += 1;
            }

            if (checkCount >= m_Building.Blocks.Count)
            {
                float elapsedTime = Time.time - m_StartTime;
                CompleteStageRpc(elapsedTime);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void CompleteStageRpc(float elapsedTime)
        {
            Cursor.lockState = CursorLockMode.None;
            m_ResultPanel.ShowResult(0, elapsedTime, true);
        }
    }
}