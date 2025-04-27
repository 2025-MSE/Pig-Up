using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MSE.Core
{
    public class UIRoomGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_RoomNameText;

        [SerializeField]
        private Transform m_SlotRoot;
        private List<UIRoomPlayerSlot> m_Slots = new List<UIRoomPlayerSlot>();

        [SerializeField]
        private Button m_StartButton;

        private void Awake()
        {
            m_Slots = m_SlotRoot.GetComponentsInChildren<UIRoomPlayerSlot>().ToList();
        }

        public void Config()
        {
            Refresh();

            LobbyManager.Instance.OnLobbyUpdated += OnUpdated;
        }

        public void Refresh()
        {
            Lobby myLobby = LobbyManager.Instance.MyLobby;
            m_RoomNameText.text = myLobby.Name;
            for (int i = 0; i < myLobby.Players.Count; i++)
            {
                var player = myLobby.Players[i];
                m_Slots[i].gameObject.SetActive(true);
                m_Slots[i].Config(player.Data["name"].Value);
            }
            for (int i = myLobby.Players.Count; i < m_Slots.Count; i++)
            {
                m_Slots[i].gameObject.SetActive(false);
            }

            bool isOwner = myLobby.HostId == AuthenticationService.Instance.PlayerId;
            m_StartButton.gameObject.SetActive(isOwner);
        }

        public async void OnExitPressed()
        {
            LobbyManager.Instance.OnLobbyUpdated -= OnUpdated;

            await LobbyManager.Instance.LeaveLobby(LobbyManager.Instance.MyLobby.Id);
            gameObject.SetActive(false);
        }

        public async void OnStartPressed()
        {
            await LobbyManager.Instance.GenerateRelayJoinCode();
        }

        private void OnUpdated()
        {
            Refresh();
        }
    }
}
