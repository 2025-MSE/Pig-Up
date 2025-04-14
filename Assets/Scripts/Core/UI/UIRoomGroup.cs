using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MSE.Core
{
    public class UIRoomGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_RoomNameText;
        [SerializeField] private TMP_Text m_PlayerCountText;
        [SerializeField] private TMP_Text m_PlayersText;

        [SerializeField]
        private Button m_StartButton;

        public void Config(bool isOwner = false)
        {
            Lobby myLobby = LobbyManager.Instance.MyLobby;
            m_RoomNameText.text = myLobby.Name;
            m_PlayerCountText.text = $"{myLobby.Players.Count} / {myLobby.MaxPlayers}";
            m_PlayersText.text = "";
            foreach (var player in myLobby.Players)
            {
                m_PlayersText.text += $"{player.Data["name"].Value} ";
            }

            m_StartButton.gameObject.SetActive(isOwner);
        }

        public async void OnExitPressed()
        {
            await LobbyManager.Instance.LeaveLobby(LobbyManager.Instance.MyLobby.Id);
            gameObject.SetActive(false);
        }

        public async void OnStartPressed()
        {
            await LobbyManager.Instance.GenerateRelayJoinCode();
        }
    }
}
