using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace MSE.Core
{
    public class AuthManager
    {
        private static AuthManager s_Instance;
        public static AuthManager Instance
        {
            get
            {
                s_Instance ??= new AuthManager();
                return s_Instance;
            }
        }

        public async Task SignupAsync(string username, string password, string playername)
        {
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playername);
                await API.UpdateUserIdAsync(AuthenticationService.Instance.PlayerId);
            }
            catch (Exception ex)
            {
                AuthenticationService.Instance.SignOut();
                throw ex;
            }
        }

        public async Task LoginAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                await API.UpdateUserIdAsync(AuthenticationService.Instance.PlayerId);
            }
            catch (Exception ex)
            {
                AuthenticationService.Instance.SignOut();
                throw ex;
            }
        }
    }
}
