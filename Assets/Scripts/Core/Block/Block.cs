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

        private Renderer m_Renderer;

        private bool m_IsPartition = false;
        private bool m_Checked = false;

        private void Awake()
        {
            m_Boundary = GetComponentInChildren<BlockBoundary>();
            m_Detection = GetComponentInChildren<BlockDetection>();
            m_Renderer = GetComponentInChildren<Renderer>();
            m_DetecteeObj = transform.Find("Detectee").gameObject;
        }

        public void ReadyToBuild()
        {
            m_Boundary.SetBoundaryActive(false);
            m_Detection.gameObject.SetActive(true);
            m_DetecteeObj.SetActive(false);
            foreach (var mat in m_Renderer.materials)
            {
                Color color = mat.color;
                color.a = 0.5f;
                mat.color = color;
            }
        }

        public void ConfigBuilding()
        {
            m_Boundary.SetBoundaryActive(false);
            m_Detection.gameObject.SetActive(false);
            m_DetecteeObj.SetActive(true);
            foreach (var mat in m_Renderer.materials)
            {
                Color color = mat.color;
                color.a = 0.2f;
                mat.color = color;
            }

            m_Checked = false;
        }

        public void ConfigPartition()
        {
            m_Boundary.SetBoundaryActive(false);
            m_Detection.gameObject.SetActive(false);
            m_DetecteeObj.SetActive(false);
            m_IsPartition = true;
        }

        public void OnBuilt()
        {
            m_Boundary.SetBoundaryActive(true);
            m_Detection.gameObject.SetActive(false);
            m_DetecteeObj.SetActive(false);
            foreach (var mat in m_Renderer.materials)
            {
                Color color = mat.color;
                color.a = 1f;
                mat.color = color;
            }
        }

        public void SetChecked(bool isChecked)
        {
            m_Checked = isChecked;
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
