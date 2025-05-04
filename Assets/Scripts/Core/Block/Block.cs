using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public class Block : NetworkBehaviour
    {
        public int Index = -1;
        [HideInInspector]
        public int BuiltIndex = -1;

        private BlockBoundary m_Boundary;

        private BlockDetection m_Detection;
        public BlockDetection Detection => m_Detection;

        private GameObject m_DetecteeObj;

        private GameObject m_SelectionObj;

        private BlockRenderer m_Renderer;
        public BlockRenderer Renderer => m_Renderer;

        private bool m_IsPartition = false;
        private bool m_Checked = false;

        private void Awake()
        {
            m_Boundary = GetComponentInChildren<BlockBoundary>();
            m_Detection = GetComponentInChildren<BlockDetection>();
            m_Renderer = GetComponentInChildren<BlockRenderer>();
            m_DetecteeObj = transform.Find("Detectee").gameObject;
            m_SelectionObj = transform.Find("Selection").gameObject;
        }

        /// <summary>
        /// Invoked when the block is spawned with silhouette.
        /// </summary>
        public void ReadyToBuild()
        {
            m_Boundary.SetBoundaryActive(false);
            m_Detection.gameObject.SetActive(true);
            m_DetecteeObj.SetActive(false);
            m_SelectionObj.SetActive(false);
            m_Renderer.SetTransparency(0.5f);
        }

        /// <summary>
        /// Invoked when the block is spawned in building.
        /// </summary>
        [Rpc(SendTo.ClientsAndHost)]
        public void ConfigBuildingRpc()
        {
            m_Boundary.SetBoundaryActive(false);
            m_Detection.gameObject.SetActive(false);
            m_DetecteeObj.SetActive(true);
            m_SelectionObj.SetActive(false);
            m_Renderer.SetTransparency(0.2f);

            m_Checked = false;
        }

        /// <summary>
        /// Invoked when the block is spawned with partition.
        /// </summary>
        [Rpc(SendTo.ClientsAndHost)]
        public void ConfigPartitionRpc()
        {
            m_Boundary.SetBoundaryActive(false);
            m_Detection.gameObject.SetActive(false);
            m_DetecteeObj.SetActive(false);
            m_SelectionObj.SetActive(true);
            m_IsPartition = true;
        }

        /// <summary>
        /// Invoked when the block is built by the player.
        /// </summary>
        public void OnBuilt()
        {
            m_Boundary.SetBoundaryActive(true);
            m_Detection.gameObject.SetActive(false);
            m_DetecteeObj.SetActive(false);
            m_SelectionObj.SetActive(false);
            m_Renderer.SetTransparency(1f);
        }

        /// <summary>
        /// When the building block is placed(by the silhouette), it will be checked.
        /// </summary>
        /// <param name="isChecked"></param>
        public void SetChecked(bool isChecked)
        {
            m_Checked = isChecked;
            m_DetecteeObj.SetActive(false);
        }
        public bool IsChecked()
        {
            return m_Checked;
        }

        public bool IsPartition()
        {
            return m_IsPartition;
        }
    }
}
