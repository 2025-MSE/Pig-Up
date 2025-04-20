using UnityEngine;

namespace MSE.Core
{
    public class DataManager : MonoBehaviour
    {
        private static bool m_Initialized = false;

        [SerializeField]
        private BlockData m_BlockData;
        public static BlockData BlockData;

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
        }

        #region BlockData
        public void InitializeBlockData()
        {
            BlockData = m_BlockData;
            for (int i = 0; i < m_BlockData.BlockPrefabs.Count; i++)
            {
                m_BlockData.BlockPrefabs[i].Index = i;
            }
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
    }
}
