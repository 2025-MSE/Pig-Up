using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSE.Core
{
    public class UILobbyCell : MonoBehaviour
    {
        private string m_LobbyId;
        public string LobbyId => m_LobbyId;

        [SerializeField]
        private TMP_Text m_Text;
        [SerializeField]
        private Button m_Button;
        public Button Button => m_Button;

        public void Config(string lobbyId, string lobbyName, int currPlayers, int maxPlayers)
        {
            m_LobbyId = lobbyId;
            m_Text.text = $"{lobbyName} {currPlayers}/{maxPlayers}";
        }
    }
}

