/// <summary>
/// Author: Dongjin Kuk
/// Description: This class manages the player's PlayerInput component.
///     We have to enable the PlayerInput only when the Player is owner of itself.
/// </summary>

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
