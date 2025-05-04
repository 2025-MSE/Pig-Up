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

        public Action OnLobbyUpdated;

        public async Task<Lobby> CreateLobby(string lobbyName, int maxPlayers, int stage)
        {
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;
            options.IsLocked = false;
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "stage", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: stage.ToString(),
                        index: DataObject.IndexOptions.S1)
                },
                {
                    "started", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: "false")
                }
            };

            Lobby newLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            m_Lobbies.Add(newLobby);

            var callbacks = new LobbyEventCallbacks();
            callbacks.LobbyChanged += OnLobbyChanged;
            await LobbyService.Instance.SubscribeToLobbyEventsAsync(newLobby.Id, callbacks);

            m_MyLobby = newLobby;

            UpdatePlayerOptions upOptions = new UpdatePlayerOptions();
            upOptions.Data = new Dictionary<string, PlayerDataObject>()
            {
                {
                    "name", new PlayerDataObject(
                        visibility: PlayerDataObject.VisibilityOptions.Public,
                        value: AuthenticationService.Instance.PlayerName)
                },
            };

            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.UpdatePlayerAsync(newLobby.Id, playerId, upOptions);

            return newLobby;
        }

        public async Task RemoveLobby(string lobbyId)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
                m_MyLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        public async Task JoinLobby(string lobbyId)
        {
            try
            {
                JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
                {
                    Player = new Unity.Services.Lobbies.Models.Player(
                        id: AuthenticationService.Instance.PlayerId,
                        data: new Dictionary<string, PlayerDataObject>
                        {
                            {
                                "name", new PlayerDataObject(
                                    visibility: PlayerDataObject.VisibilityOptions.Public,
                                    value: AuthenticationService.Instance.PlayerName
                                )
                            }
                        }
                    )
                };
                Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
                m_MyLobby = joinedLobby;

                var callbacks = new LobbyEventCallbacks();
                callbacks.LobbyChanged += OnLobbyChanged;
                await LobbyService.Instance.SubscribeToLobbyEventsAsync(joinedLobby.Id, callbacks);
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
                    "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode)
                }
            };

            await LobbyService.Instance.UpdateLobbyAsync(m_MyLobby.Id, ulOptions);
        }

        public async Task LeaveLobby(string lobbyId)
        {
            try
            {
                NetworkManager.Singleton.Shutdown();

                string playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);

                m_MyLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogException(e);
            }
        }

        public async Task<List<Lobby>> QueryLobby(int stage)
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
                        value: stage.ToString())
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
            if (changes.LobbyDeleted)
            {
                return;
            }
            else
            {
                changes.ApplyToLobby(m_MyLobby);
                OnLobbyUpdated?.Invoke();
            }

            if (m_MyLobby.Data.ContainsKey("joinCode"))
            {
                bool isHost = m_MyLobby.HostId == AuthenticationService.Instance.PlayerId;

                if (isHost)
                {
                    await SceneManager.LoadSceneAsync("Game");
                    NetworkManager.Singleton.StartHost();
                } else
                {
                    await RelayManager.Instance.StartClient(m_MyLobby.Data["joinCode"].Value);
                    NetworkManager.Singleton.StartClient();
                }
            }
        }
    }
}
