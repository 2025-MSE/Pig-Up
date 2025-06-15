/// <summary>
/// Author: Dongjin Kuk
/// Description: By overriding the NetworkAnimator, we can synchronize client's animator without adding another sync system.
/// </summary>

using Unity.Netcode.Components;

public class ClientNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative() => false;
}
