/// <summary>
/// Author: Dongjin Kuk
/// Description: Lobby Group (UI Panel)
/// </summary>

using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace MSE.Core
{
    public class UILobbyGroup : MonoBehaviour
    {
        [SerializeField] private UILobbyCell m_LobbyCellPrefab;
        [SerializeField] private Transform m_LobbyRootTrans;

        [SerializeField] private GameObject m_IconObj;
        [SerializeField] private GameObject m_RootObj;

        [SerializeField] private UIRoomGroup m_RoomGroup;

        private bool m_Creating = false;
        private bool m_Refreshing = false;

        private void OnEnable()
        {
            m_Creating = false;
            m_Refreshing = false;
            Refresh();
        }

        public async void OnCreateLobbyButtonPressed()
        {
            if (m_Creating) return;

            m_Creating = true;

            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);

            float randLobbySuffix = Random.Range(0, 1000);
            await LobbyManager.Instance.CreateLobby($"Lobby{randLobbySuffix}", 3, DataManager.CurrStageData.Name);

            m_RoomGroup.gameObject.SetActive(true);
            m_RoomGroup.Config();

            m_Creating = false;
        }

        public void OnRefreshButtonPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            Refresh();
        }

        public void OnBackButtonPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            gameObject.SetActive(false);
        }

        public async void OnLobbyCellPressed(UILobbyCell lobbyCell)
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);

            await LobbyManager.Instance.JoinLobby(lobbyCell.LobbyId);

            m_RoomGroup.gameObject.SetActive(true);
            m_RoomGroup.Config();
        }

        public async void Refresh()
        {
            if (m_Refreshing) return;

            m_Refreshing = true;
            m_IconObj.SetActive(true);
            m_RootObj.SetActive(false);

            foreach (Transform childTrans in m_LobbyRootTrans)
            {
                Destroy(childTrans.gameObject);
            }

            var queriedLobbies = await LobbyManager.Instance.QueryLobby(DataManager.CurrStageData.Name);
            foreach (Lobby lobby in queriedLobbies)
            {
                UILobbyCell newLobbyCell = Instantiate(m_LobbyCellPrefab);
                newLobbyCell.transform.SetParent(m_LobbyRootTrans);
                newLobbyCell.Config(lobby.Id, lobby.Name, lobby.Players.Count, lobby.MaxPlayers);
                newLobbyCell.Button.onClick.AddListener(() => OnLobbyCellPressed(newLobbyCell));
            }

            m_Refreshing = false;
            m_IconObj.SetActive(false);
            m_RootObj.SetActive(true);
        }
    }
}
