/**
 * Owner: Dongjin Kuk
 */

using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;
using UnityEngine;

namespace MSE.Core
{
    public class ConnectionManager : MonoBehaviour
    {
        private string m_ProfileName;
        private string m_SessionName;
        private int m_MaxPlayers = 3;
        private ConnectionState m_State = ConnectionState.Disconnected;
        private ISession m_Session;
        private NetworkManager m_NetworkManager;

        private enum ConnectionState
        {
            Disconnected,
            Connecting,
            Connected
        }

        private async void Awake()
        {
            m_NetworkManager = GetComponent<NetworkManager>();
            m_NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
            m_NetworkManager.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
            await UnityServices.InitializeAsync();
        }

        private void OnSessionOwnerPromoted(ulong sessionOwnerPromoted)
        {
            if (m_NetworkManager.LocalClient.IsSessionOwner)
            {
                Debug.Log($"Client - {m_NetworkManager.LocalClientId} is the session owner!");
            }
        }

        private void OnClientConnectedCallback(ulong clientId)
        {
            if (m_NetworkManager.LocalClientId == clientId)
            {
                Debug.Log($"Client-{clientId} is connected and can spawn {nameof(NetworkObject)}s.");
            }
        }

        private void OnGUI()
        {
            if (m_State == ConnectionState.Connected) return;

            GUI.enabled = m_State != ConnectionState.Connecting;

            using (new GUILayout.HorizontalScope(GUILayout.Width(250)))
            {
                GUILayout.Label("Profile Name", GUILayout.Width(100));
                m_ProfileName = GUILayout.TextField(m_ProfileName);
            }

            using (new GUILayout.HorizontalScope(GUILayout.Width(250)))
            {
                GUILayout.Label("Session Name", GUILayout.Width(100));
                m_SessionName = GUILayout.TextField(m_SessionName);
            }

            GUI.enabled = GUI.enabled && !string.IsNullOrEmpty(m_ProfileName) && !string.IsNullOrEmpty(m_SessionName);

            if (GUILayout.Button("Create or Join Session"))
            {
                CreateOrJoinSessionAsync();
            }
        }

        private void OnDestroy()
        {
            m_Session?.LeaveAsync();
        }

        private async Task CreateOrJoinSessionAsync()
        {
            m_State = ConnectionState.Connecting;

            //try
            //{
            //    AuthenticationService.Instance.SwitchProfile(m_ProfileName);
            //    await AuthenticationService.Instance.SignInAnonymouslyAsync();

            //    var options = new SessionOptions()
            //    {
            //        Name = m_SessionName,
            //        MaxPlayers = m_MaxPlayers
            //    }.WithRelayNetwork();
            //} catch
        }
    }
}
