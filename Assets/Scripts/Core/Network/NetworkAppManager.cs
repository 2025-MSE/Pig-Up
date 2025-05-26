using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSE.Core
{
    public class NetworkAppManager : MonoBehaviour
    {
        private static NetworkAppManager s_Instance;
        public static NetworkAppManager Instance => s_Instance;

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                s_Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void OnApplicationQuit()
        {
            var myLobby = LobbyManager.Instance.MyLobby;
            if (myLobby != null)
            {
                LobbyManager.Instance.LeaveLobby(myLobby.Id);
            }

            LobbyManager.Instance.Started = false;
        }

        private void OnClientDisconnected(ulong id)
        {
            Debug.Log($"Client {id} disconnected!");
            UIManager.Instance.ShowToastMessage($"Client {id} disconnected!");
        }
    }
}
