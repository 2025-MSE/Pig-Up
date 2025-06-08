using UnityEngine;

namespace MSE.Core
{
    public class UIStageEnterPanel : MonoBehaviour
    {
        [SerializeField] private UILobbyGroup m_LobbyGroup;

        public void OnLobbyPressed()
        {
            m_LobbyGroup.gameObject.SetActive(true);
        }

        public void OnStoryPressed()
        {
            UIManager.Instance.ShowToastMessage("Story is work in progress.");
        }

        public void OnBackPressed()
        {
            gameObject.SetActive(false);
        }
    }
}
