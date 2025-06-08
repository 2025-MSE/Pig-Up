/**
 * Owner: Dongjin Kuk
 */

using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

namespace MSE.Core
{
    public class Player : NetworkBehaviour
    {
        private NetworkVariable<FixedString64Bytes> m_PlayerName = new NetworkVariable<FixedString64Bytes>(writePerm: NetworkVariableWritePermission.Owner);

        [SerializeField]
        private Camera m_Camera;

        private Animator m_Animator;
        public Animator Animator => m_Animator;

        [SerializeField]
        private UICharacterInfo m_Info;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
            m_Camera.gameObject.SetActive(false);
        }

        public override void OnNetworkSpawn()
        {
            m_Camera.gameObject.SetActive(IsOwner);

            if (IsOwner)
            {
                string playerName = AuthenticationService.Instance.PlayerName;
                playerName = string.IsNullOrEmpty(playerName) ? AuthenticationService.Instance.PlayerId : playerName;
                m_PlayerName.Value = playerName;
            }

            m_PlayerName.OnValueChanged += OnPlayerNameChanged;
            m_Info.SetInfo(m_PlayerName.Value.ToString());

            Debug.Log($"PlayerName: {m_PlayerName.Value.ToString()}");
        }

        private void OnPlayerNameChanged(FixedString64Bytes prevName, FixedString64Bytes currName)
        {
            m_Info.SetInfo(currName.ToString());
        }
    }
}
