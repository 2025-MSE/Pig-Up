using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class Block : NetworkBehaviour
    {
        public int Index = -1;

        private BlockBoundary m_Boundary;

        private Renderer m_Renderer;

        private void Awake()
        {
            m_Boundary = GetComponentInChildren<BlockBoundary>();
            m_Renderer = GetComponentInChildren<Renderer>();
        }

        public void ReadyToBuild()
        {
            m_Boundary.SetBoundaryActive(false);
            foreach (var mat in m_Renderer.materials)
            {
                Color color = mat.color;
                color.a = 0.5f;
                mat.color = color;
            }
        }

        public void OnBuilt()
        {
            m_Boundary.SetBoundaryActive(true);
            foreach (var mat in m_Renderer.materials)
            {
                Color color = mat.color;
                color.a = 1f;
                mat.color = color;
            }
        }
    }
}
