using UnityEngine;

namespace MSE.Core
{
    public class GameMap : MonoBehaviour
    {
        [SerializeField]
        private Transform m_PlayerSpawnPointRoot;
        private Transform[] m_PlayerSpawnPoints;
        public Transform[] PlayerSpawnPoints => m_PlayerSpawnPoints;

        [SerializeField]
        private Transform m_BuildingSpawnPoint;
        public Transform BuildingSpawnPoint => m_BuildingSpawnPoint;

        [SerializeField]
        private Transform m_PBlockSpawnPointRoot;
        private Transform[] m_PBlockSpawnPoints;
        public Transform[] PBlockSpawnPoints => m_PBlockSpawnPoints;

        private void Awake()
        {
            m_PlayerSpawnPoints = m_PlayerSpawnPointRoot.GetComponentsInChildren<Transform>();
            m_PBlockSpawnPoints = m_PBlockSpawnPointRoot.GetComponentsInChildren<Transform>();
        }
    }
}
