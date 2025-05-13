using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

namespace MSE.Core
{
    public class GameController : NetworkBehaviour
    {
        [SerializeField]
        private NetworkObject m_PlayerPrefab;

        [SerializeField]
        private GameMap m_GameMap;
        private Building m_Building;

        private float m_StartTime = 0f;

        [SerializeField]
        private UIStageResult m_ResultPanel;

        public override void OnNetworkSpawn()
        {
            SpawnPlayerRpc();

            if (!IsServer) return;

            GameEventCallbacks.OnBlockBuilt += OnBlockBuilt;

            m_StartTime = Time.time;
            CreateBuilding();
        }

        public override void OnNetworkDespawn()
        {
            if (!IsServer) return;

            GameEventCallbacks.OnBlockBuilt -= OnBlockBuilt;
        }

        [Rpc(SendTo.Server)]
        private void SpawnPlayerRpc(RpcParams rpcParams = default)
        {
            NetworkObject nObject = Instantiate(m_PlayerPrefab);
            nObject.SpawnAsPlayerObject(rpcParams.Receive.SenderClientId);
        }

        private void CreateBuilding()
        {
            StageData stageData = DataManager.CurrStageData;
            m_Building = Instantiate(stageData.BuildingPrefab, Vector3.zero, Quaternion.identity);
            m_Building.AssignBuilding();

            SpawnInFieldBlocks();
        }

        private void SpawnInFieldBlocks()
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
                SpawnInFieldBlock(sbIndex, spawnpoint.position, spawnpoint.rotation);

                index += 1;
            }
        }

        private void SpawnInFieldBlock(int sbIndex, Vector3 position, Quaternion rotation, RpcParams rpcParams = default)
        {
            NetworkObject nBlockObj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                DataManager.GetBlock(sbIndex).GetComponent<NetworkObject>(),
                position: position,
                rotation: rotation);
            Block block = nBlockObj.GetComponent<Block>();
            block.SetStrategy(BlockStrategyType.IN_FIELD);
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
                block.SetChecked(true);
                builtBlocks[0].SetChecked(true);
                AdjustBlockRpc(netBlockId, builtBlocks[0].transform.position, builtBlocks[0].transform.eulerAngles);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void AdjustBlockRpc(ulong netBlockId, Vector3 position, Vector3 eulerAngles)
        {
            var nobj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[netBlockId];

            nobj.transform.DOMove(position, 0.5f);
            nobj.transform.DORotate(eulerAngles, 0.5f);

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

            try
            {
                API.SaveStageClearData(AuthenticationService.Instance.PlayerId, DataManager.CurrStageData.Name, (long)elapsedTime);
                Debug.Log("Successfully saved stage clear data!");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}