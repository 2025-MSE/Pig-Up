using Newtonsoft.Json;
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

        public IEnumerator SignUpCoroutine(string username, string password, string email)
        {
            var dataBody = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "email", email }
            };
            string dataJson = JsonConvert.SerializeObject(dataBody);

            Task signUpTask = AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            yield return new WaitUntil(() => signUpTask.IsCompleted);

            if (signUpTask.Exception != null)
            {
                Debug.LogException(signUpTask.Exception);
                yield break;
            }

            Task updatePlayerNameTask = AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            yield return new WaitUntil(() => updatePlayerNameTask.IsCompleted);

            using (UnityWebRequest request = new UnityWebRequest($"{ServerRef.BASE_URL}/api/users/signup", "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(dataJson);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();
            }
        }

        public IEnumerator LoginCoroutine(string username, string password)
        {
            var dataBody = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };
            string dataJson = JsonConvert.SerializeObject(dataBody);

            Task loginTask = AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                Debug.LogException(loginTask.Exception);
                yield break;
            }

            Task updatePlayerNameTask = AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            yield return new WaitUntil(() => updatePlayerNameTask.IsCompleted);

            using (UnityWebRequest request = new UnityWebRequest($"{ServerRef.BASE_URL}/apis/users/login", "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(dataJson);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
