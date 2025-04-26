using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Networking;

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

        public IEnumerator SignUpCoroutine(string username, string password, string playername, Action<bool> callback)
        {
            Task signUpTask = AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            yield return new WaitUntil(() => signUpTask.IsCompleted);

            if (signUpTask.Exception != null)
            {
                Debug.LogException(signUpTask.Exception);
                callback?.Invoke(false);
                yield break;
            }

            Task updatePlayerNameTask = AuthenticationService.Instance.UpdatePlayerNameAsync(playername);
            yield return new WaitUntil(() => updatePlayerNameTask.IsCompleted);

            callback?.Invoke(true);
        }

        public IEnumerator LoginCoroutine(string username, string password, Action<bool> callback)
        {
            Task loginTask = AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                Debug.LogException(loginTask.Exception);
                callback?.Invoke(false);
                yield break;
            }

            Task updatePlayerNameTask = AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            yield return new WaitUntil(() => updatePlayerNameTask.IsCompleted);

            callback?.Invoke(true);
        }
    }
}
