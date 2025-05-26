/**
 * Owner: Dongjin Kuk
 */

using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

namespace MSE.Core
{
    public struct PlayerTag
    {
        public string Id;
        public string Name;
        public Transform Transform;
    }

    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private Camera m_Camera;

        private Animator m_Animator;
        public Animator Animator => m_Animator;

        private static List<PlayerTag> m_PlayerTags = new List<PlayerTag>();

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
            m_Camera.gameObject.SetActive(false);
        }

        public override void OnNetworkSpawn()
        {
            m_Camera.gameObject.SetActive(IsOwner);

            // Spawn it's player name info.
            if (!IsOwner)
            {
                SpawnNameInfoRpc(AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.PlayerName);
            }
            else
            {
                foreach (var tag in m_PlayerTags)
                {
                    UICharacterInfoHandler.OnCharacterActivated?.Invoke(tag.Id, tag.Transform, tag.Name, transform);
                }
            }
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                RemoveNameInfoRpc(AuthenticationService.Instance.PlayerId);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SpawnNameInfoRpc(string id, string name)
        {
            if (NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject() == null)
            {
                m_PlayerTags.Add(new PlayerTag { Id = id, Name = string.IsNullOrEmpty(name) ? id : name, Transform = transform });
                return;
            }

            UICharacterInfoHandler.OnCharacterActivated?.Invoke(id, transform, string.IsNullOrEmpty(name) ? id : name, NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().transform);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void RemoveNameInfoRpc(string id)
        {
            UICharacterInfoHandler.OnCharacterDeactivated?.Invoke(id);
        }
    }
}
