using System;
using Unity.Netcode;
using UnityEngine;

namespace MSE.Core
{
    public enum BlockStrategyType
    {
        READY_TO_BUILD,
        IN_BUILDING,
        BUILT_BY_PLAYER,
        CHECKED,
        IN_FIELD
    }

    public class Block : NetworkBehaviour
    {
        public int Index = -1;

        private NetworkVariable<int> m_InBuildingIndex = new NetworkVariable<int>();
        public int InBuildingIndex => m_InBuildingIndex.Value;

        private NetworkVariable<BlockStrategyType> m_StrategyType = new NetworkVariable<BlockStrategyType>();
        public BlockStrategyType StrategyType => m_StrategyType.Value;
        private BlockStrategyBase m_Strategy;

        private GameObject m_BoundaryObj;
        public GameObject BoundaryObj => m_BoundaryObj;

        private BlockDetection m_Detection;
        public BlockDetection Detection => m_Detection;

        private GameObject m_DetecteeObj;
        public GameObject DetecteeObj => m_DetecteeObj;

        private GameObject m_SelectionObj;
        public GameObject SelectionObj => m_SelectionObj;

        private BlockRenderer m_Renderer;
        public BlockRenderer Renderer => m_Renderer;

        private bool m_Checked = false;

        private void Awake()
        {
            m_BoundaryObj = transform.Find("Boundary").gameObject;
            m_Detection = GetComponentInChildren<BlockDetection>();
            m_Renderer = GetComponentInChildren<BlockRenderer>();
            m_DetecteeObj = transform.Find("Detectee").gameObject;
            m_SelectionObj = transform.Find("Selection").gameObject;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
            }

            m_InBuildingIndex.OnValueChanged += OnInBuildingIndexChanged;
            m_StrategyType.OnValueChanged += OnStrategyChanged;
        }

        private void OnInBuildingIndexChanged(int prevIndex, int currIndex)
        {
            UpdateInBuildingIndex();
        }

        public void SetInBuildingIndex(int index)
        {
            m_InBuildingIndex.Value = index;
        }

        private void UpdateInBuildingIndex()
        {
        }

        private void OnStrategyChanged(BlockStrategyType prevStrategy, BlockStrategyType currStrategy)
        {
            UpdateStrategy();
        }

        public void SetStrategy(BlockStrategyType strategyType)
        {
            m_StrategyType.Value = strategyType;
            if (!IsSpawned)
            {
                UpdateStrategy();
            }
        }

        private void UpdateStrategy()
        {
            Debug.Log($"Updated Strategy: {m_StrategyType.Value.ToString()}");
            switch (m_StrategyType.Value)
            {
                case BlockStrategyType.READY_TO_BUILD:
                    m_Strategy = new BlockReadyStrategy(this);
                    break;
                case BlockStrategyType.IN_BUILDING:
                    m_Strategy = new BlockInBuildingStrategy(this);
                    break;
                case BlockStrategyType.BUILT_BY_PLAYER:
                    m_Strategy = new BlockBuiltByPlayerStrategy(this);
                    break;
                case BlockStrategyType.CHECKED:
                    m_Strategy = new BlockCheckedStrategy(this);
                    break;
                case BlockStrategyType.IN_FIELD:
                    m_Strategy = new BlockInFieldStrategy(this);
                    break;
            }
            m_Strategy.Initialize();
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
    }
}
