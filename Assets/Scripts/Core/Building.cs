using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class Building : MonoBehaviour
    {
        public BuildingParsedData[] ParsedDatas;

        [SerializeField]
        private Transform m_BlockRoot;
        private List<Block> m_Blocks = new List<Block>();
        public List<Block> Blocks => m_Blocks;

        public void AssignBuilding()
        {
            for (int i = 0; i < ParsedDatas.Length; i++)
            {
                BuildingParsedData data = ParsedDatas[i];

                Block blockPrefab = DataManager.GetBlock(data.BlockIndex);

                NetworkObject nBlockObj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(blockPrefab.GetComponent<NetworkObject>(),
                    position: data.Position,
                    rotation: data.Rotation);
                Block block = nBlockObj.GetComponent<Block>();

                nBlockObj.TrySetParent(m_BlockRoot);
                block.SetStrategy(BlockStrategyType.IN_BUILDING);
                block.SetInBuildingIndex(i);

                m_Blocks.Add(block);
            }
        }
    }
}
