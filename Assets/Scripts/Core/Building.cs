using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class Building : MonoBehaviour
    {
        [SerializeField]
        private Transform m_BlockRoot;
        private List<Block> m_Blocks = new List<Block>();
        public List<Block> Blocks => m_Blocks;

        void Awake()
        {
            m_Blocks = m_BlockRoot.GetComponentsInChildren<Block>().ToList();
        }

        public void AssignBuilding()
        {
            for (int i = 0; i < m_Blocks.Count; i++)
            {
                Block block = m_Blocks[i];
                NetworkObject nBlockObj = block.GetComponent<NetworkObject>();

                nBlockObj.TrySetParent(m_BlockRoot);
                nBlockObj.Spawn();

                block.SetStrategy(BlockStrategyType.IN_BUILDING);
                block.SetInBuildingIndex(i);
            }
        }
    }
}
