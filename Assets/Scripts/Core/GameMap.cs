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

        private void Awake()
        {
            m_PlayerSpawnPoints = m_PlayerSpawnPointRoot.GetComponentsInChildren<Transform>();
        }
    }
}
