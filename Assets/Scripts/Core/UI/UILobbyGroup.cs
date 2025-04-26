using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace MSE.Core
{
    public class UILobbyGroup : MonoBehaviour
    {
        [SerializeField] private UILobbyCell m_LobbyCellPrefab;
        [SerializeField] private Transform m_LobbyRootTrans;

        [SerializeField]
        private UIRoomGroup m_RoomGroup;

        public async void OnCreateLobbyButtonPressed()
        {
            float randLobbySuffix = Random.Range(0, 1000);
            await LobbyManager.Instance.CreateLobby($"Lobby{randLobbySuffix}", 3, 1);

            m_RoomGroup.gameObject.SetActive(true);
            m_RoomGroup.Config();
        }

        public void OnRefreshButtonPressed()
        {
            Refresh();
        }

        public async void OnLobbyCellPressed(UILobbyCell lobbyCell)
        {
            await LobbyManager.Instance.JoinLobby(lobbyCell.LobbyId);

            m_RoomGroup.gameObject.SetActive(true);
            m_RoomGroup.Config();
        }

        public async void Refresh()
        {
            foreach (Transform childTrans in m_LobbyRootTrans)
            {
                Destroy(childTrans.gameObject);
            }

            var queriedLobbies = await LobbyManager.Instance.QueryLobby(1);
            foreach (Lobby lobby in queriedLobbies)
            {
                UILobbyCell newLobbyCell = Instantiate(m_LobbyCellPrefab);
                newLobbyCell.transform.SetParent(m_LobbyRootTrans);
                newLobbyCell.Config(lobby.Id, lobby.Name, lobby.Players.Count, lobby.MaxPlayers);
                newLobbyCell.Button.onClick.AddListener(() => OnLobbyCellPressed(newLobbyCell));
            }
        }
    }
}
