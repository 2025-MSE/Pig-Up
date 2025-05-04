using System.Collections.Generic;
using UnityEngine;

namespace MSE.Core
{
    public class DataManager : MonoBehaviour
    {
        private static bool m_Initialized = false;

        [SerializeField]
        private BlockData m_BlockData;
        public static BlockData BlockData;

        [SerializeField]
        private List<StageData> m_StageDatas;
        public static List<StageData> StageDatas;
        public static StageData CurrStageData;

        private void Awake()
        {
            if (m_Initialized)
            {
                Destroy(gameObject);
                return;
            }

            m_Initialized = true;
            DontDestroyOnLoad(gameObject);
            InitializeBlockData();
            InitializeStageData();
        }

        #region BlockData
        public void InitializeBlockData()
        {
            BlockData = m_BlockData;
        }

        public static Block GetBlock(int index)
        {
            return BlockData.BlockPrefabs[index];
        }
        public static Block GetBlock(string name)
        {
            Block block = BlockData.BlockPrefabs.Find(prefab => prefab.name == name);
            return block;
        }
        #endregion

        #region StageData
        public void InitializeStageData()
        {
            StageDatas = m_StageDatas;
        }

        public static StageData GetStageData(int index)
        {
            return StageDatas[index];
        }
        public static StageData GetStageData(string name)
        {
            StageData stageData = StageDatas.Find(data => data.Name == name);
            return stageData;
        }
        #endregion
    }
}
