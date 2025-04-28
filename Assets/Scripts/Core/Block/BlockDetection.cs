using System.Collections.Generic;
using UnityEngine;

namespace MSE.Core
{
    public class BlockDetection : MonoBehaviour
    {
        private Block m_Block;

        private List<int> m_DetectedBuiltIndice = new List<int>();
        public List<int> DetectedBuiltIndice => m_DetectedBuiltIndice;

        private void Awake()
        {
            m_Block = transform.parent.GetComponent<Block>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Block block))
            {
                if (block.Index != m_Block.Index) return;
                if (m_DetectedBuiltIndice.Contains(block.BuiltIndex)) return;

                m_DetectedBuiltIndice.Add(block.BuiltIndex);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Block block))
            {
                if (block.Index != m_Block.Index) return;

                m_DetectedBuiltIndice.Remove(block.BuiltIndex);
            }
        }
    }
}
