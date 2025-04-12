using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace MSE.Core
{
    public class RelayManager
    {
        private static RelayManager s_Instance;
        public static RelayManager Instance
        {
            get
            {
                s_Instance ??= new RelayManager();
                return s_Instance;
            }
        }

        public async Task<string> StartHost(int maxConnections, string connectionType = "dtls")
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return joinCode;
        }

        public async Task<bool> StartClient(string joinCode, string connectionType = "dtls")
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
            return !string.IsNullOrEmpty(joinCode);
        }
    }
}
