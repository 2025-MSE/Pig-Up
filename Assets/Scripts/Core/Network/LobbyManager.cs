/**
 * Owner: Dongjin Kuk
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSE.Core
{
    public class LobbyManager
    {
        private static LobbyManager s_Instance = null;
        public static LobbyManager Instance
        {
            get
            {
                s_Instance ??= new LobbyManager();
                return s_Instance;
            }
        }

        private List<Lobby> m_Lobbies = new List<Lobby>();

        private Lobby m_MyLobby = null;
        public Lobby MyLobby => m_MyLobby;

        private ILobbyEvents m_LobbyEvents = null;

        public Action OnLobbyUpdated;

        private bool m_Started = false;

        public async Task<Lobby> CreateLobby(string lobbyName, int maxPlayers, string stage)
        {
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;
            options.IsLocked = false;
            options.Data = CreateLobbyDataObject(stage);

            Lobby newLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            m_Lobbies.Add(newLobby);

            var callbacks = new LobbyEventCallbacks();
            callbacks.LobbyChanged += OnLobbyChanged;
            m_LobbyEvents = await LobbyService.Instance.SubscribeToLobbyEventsAsync(newLobby.Id, callbacks);

            m_MyLobby = newLobby;

            UpdatePlayerOptions upOptions = new UpdatePlayerOptions();
            upOptions.Data = CreatePlayerObject(AuthenticationService.Instance.PlayerName);

            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.UpdatePlayerAsync(newLobby.Id, playerId, upOptions);

            return newLobby;
        }

        public async Task JoinLobby(string lobbyId)
        {
            try
            {
                JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
                {
                    Player = new Unity.Services.Lobbies.Models.Player(
                        id: AuthenticationService.Instance.PlayerId,
                        data: CreatePlayerObject(AuthenticationService.Instance.PlayerName)
                    )
                };
                Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
                m_MyLobby = joinedLobby;

                var callbacks = new LobbyEventCallbacks();
                callbacks.LobbyChanged += OnLobbyChanged;
                m_LobbyEvents = await LobbyService.Instance.SubscribeToLobbyEventsAsync(joinedLobby.Id, callbacks);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        public async Task GenerateRelayJoinCode()
        {
            var joinCode = await RelayManager.Instance.StartHost(m_MyLobby.MaxPlayers);
            UpdateLobbyOptions ulOptions = new UpdateLobbyOptions();
            ulOptions.Data = new Dictionary<string, DataObject>
            {
                {
                    "started", new DataObject(DataObject.VisibilityOptions.Public, "false")
                },
                {
                    "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode)
                }
            };

            await LobbyService.Instance.UpdateLobbyAsync(m_MyLobby.Id, ulOptions);
        }

        public async Task LeaveLobby(string lobbyId)
        {
            try
            {
                string playerId = AuthenticationService.Instance.PlayerId;
                await m_LobbyEvents.UnsubscribeAsync();
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);

                m_MyLobby = null;
                m_Started = false;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        public async Task<List<Lobby>> QueryLobby(string stage)
        {
            try
            {
                QueryLobbiesOptions options = new QueryLobbiesOptions();
                options.Count = 25;

                options.Filters = new List<QueryFilter>()
                {
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.AvailableSlots,
                        op: QueryFilter.OpOptions.GT,
                        value: "0"),
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.S1,
                        op: QueryFilter.OpOptions.EQ,
                        value: stage)
                };

                options.Order = new List<QueryOrder>()
                {
                    new QueryOrder(
                        asc: false,
                        field: QueryOrder.FieldOptions.Created)
                };

                QueryResponse querriedLobbies = await LobbyService.Instance.QueryLobbiesAsync(options);
                return querriedLobbies.Results;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        public IEnumerator HeartbeatCoroutine(string lobbyId, float waitTimeSeconds)
        {
            WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitTimeSeconds);

            while (true)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return delay;
            }
        }

        public async void OnLobbyChanged(ILobbyChanges changes)
        {
            string prevHostId = m_MyLobby.HostId;

            if (changes.LobbyDeleted)
            {
                return;
            }
            else
            {
                changes.ApplyToLobby(m_MyLobby);
                
                if (prevHostId != m_MyLobby.HostId)
                {
                }

                OnLobbyUpdated?.Invoke();
            }

            if (m_MyLobby.Data.ContainsKey("joinCode") && !m_Started)
            {
                bool isHost = m_MyLobby.HostId == AuthenticationService.Instance.PlayerId;
                m_Started = true;

                if (isHost)
                {
                    NetworkManager.Singleton.StartHost();
                    NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
                } else
                {
                    await RelayManager.Instance.StartClient(m_MyLobby.Data["joinCode"].Value);
                    NetworkManager.Singleton.StartClient();
                }
            }
        }

        private Dictionary<string, DataObject> CreateLobbyDataObject(string stageName)
        {
            Dictionary<string, DataObject> datas = new Dictionary<string, DataObject>
            {
                { "stage", new DataObject(DataObject.VisibilityOptions.Public, stageName, DataObject.IndexOptions.S1) }
            };

            return datas;
        }

        private Dictionary<string, PlayerDataObject> CreatePlayerObject(string playerName)
        {
            Dictionary<string, PlayerDataObject> datas = new Dictionary<string, PlayerDataObject>
            {
                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
            };

            return datas;
        }
    }
}
