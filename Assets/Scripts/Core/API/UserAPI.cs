using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MSE.Core
{
    [Serializable]
    public class User
    {
        public string unityUserId;
        public UserStageClearData[] stageClearInfos;
    }

    [Serializable]
    public class UserStageClearData
    {
        public long id;
        public string stageName;
        public long clearTime;
    }

    [Serializable]
    public class StageClearData
    {
        public string unityUserId;
        public string stageName;
        public long clearTime;
    }

    public partial class API
    {
#if UNITY_EDITOR
        private readonly static string BASE_URL = "http://localhost:8080";
#else
        private readonly static string BASE_URL = "https://example.url.com"; // Need to modify when we publish this application.
#endif

        public static async Task<User> UpdateUserIdAsync(string id)
        {
            User user = new User();
            user.unityUserId = id;
            string userJson = JsonUtility.ToJson(user);

            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Post($"{BASE_URL}/api/users", userJson, "application/json");
                await webRequest.SendWebRequest();

                if (webRequest.error != null)
                {
                    throw new Exception(webRequest.error);
                }

                string resultJson = webRequest.downloadHandler.text;
                User userData = JsonUtility.FromJson<User>(resultJson);

                return userData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task SaveStageClearData(string userId, string stageName, long clearTime)
        {
            StageClearData stageClearData = new StageClearData();
            stageClearData.unityUserId = userId;
            stageClearData.stageName = stageName;
            stageClearData.clearTime = clearTime;
            string clearDataJson = JsonUtility.ToJson(stageClearData);

            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Post($"{BASE_URL}/api/users/stage-clear", clearDataJson, "application/json");
                await webRequest.SendWebRequest();

                if (webRequest.error != null)
                {
                    throw new Exception(webRequest.error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
