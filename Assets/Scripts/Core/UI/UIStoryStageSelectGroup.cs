using UnityEngine;

namespace MSE.Core
{
    public class UIStoryStageSelectGroup : MonoBehaviour
    {
        [SerializeField]
        private UILobbyGroup m_LobbyGroup;

        public void OnStageButtonPressed(int stage)
        {
            m_LobbyGroup.gameObject.SetActive(true);
        }
    }
}
