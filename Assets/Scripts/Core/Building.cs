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

        public override void OnNetworkSpawn()
        {
            m_Blocks = m_BlockRoot.GetComponentsInChildren<Block>().ToList();
            GameController.OnBuildingSpawned?.Invoke(this);
        }
    }
}
