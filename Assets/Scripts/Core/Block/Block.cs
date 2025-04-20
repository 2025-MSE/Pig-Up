using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class Block : NetworkBehaviour
    {
        public int Index = -1;

        private BlockBoundary m_Boundary;

        private void Awake()
        {
            m_Boundary = GetComponentInChildren<BlockBoundary>();
        }

        public void ReadyToBuild()
        {
            m_Boundary.SetBoundaryActive(false);
        }

        public void OnBuilt()
        {
            m_Boundary.SetBoundaryActive(true);
        }
    }
}
