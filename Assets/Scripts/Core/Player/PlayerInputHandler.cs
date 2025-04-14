/**
 * Owner: Dongjin Kuk
 */

using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerInputHandler : NetworkBehaviour
{
    private PlayerInput m_PlayerInput;

    private void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        m_PlayerInput.enabled = true;
    }
}
