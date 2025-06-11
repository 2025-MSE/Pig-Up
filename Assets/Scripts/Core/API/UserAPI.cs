using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Networking;

namespace MSE.Core
{
    [Serializable]
    public class User
    {
        public string unityUserId;
        public string playername;
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
        public string stageId;
        public string playername;
        public long clearTime;
        public string clearDateTime;
    }

    [SerializeField]
    public class StageClearResultData
    {
        public long id;
        public string unityUserId;
        public string stageId;
        public string playername;
        public long clearTime;
        public string clearDateTime;
    }

    public partial class API
    {
#if UNITY_EDITOR
        private readonly static string BASE_URL = "http://localhost:8080";
#else
        private readonly static string BASE_URL = "https://example.url.com"; // Need to modify when we publish this application.
#endif

        public static async Task<User> UpdateUserIdAsync(string id, string playerName)
        {
            User user = new User();
            user.unityUserId = id;
            user.playername = playerName;
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

        public static async Task SaveStageClearData(string userId, string stageName, string playerName, long clearTime)
        {
            StageClearData stageClearData = new StageClearData();
            stageClearData.unityUserId = userId;
            stageClearData.stageId = stageName;
            stageClearData.playername = playerName;
            stageClearData.clearTime = clearTime;
            string clearDataJson = JsonUtility.ToJson(stageClearData);

            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Post($"{BASE_URL}/api/stage-clear/save", clearDataJson, "application/json");
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

        public static async Task<List<StageClearResultData>> GetStageRanking(string stageName)
        {
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get($"{BASE_URL}/api/stage-clear/ranking/{stageName}");
                await webRequest.SendWebRequest();

                if (webRequest.error != null)
                {
                    throw new Exception(webRequest.error);
                }

                string resJson = webRequest.downloadHandler.text;
                Debug.Log(resJson);
                List<StageClearResultData> ranking = JsonConvert.DeserializeObject<List<StageClearResultData>>(resJson);

                return ranking;

            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<bool> IsStageClearedAsync(string stageName)
        {
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get($"{BASE_URL}/api/stage-clear/cleared/{AuthenticationService.Instance.PlayerId}/{stageName}");
                await webRequest.SendWebRequest();

                if (webRequest.error != null)
                {
                    throw new Exception(webRequest.error);
                }

                string resJson = webRequest.downloadHandler.text;
                bool cleared = bool.Parse(resJson);

                return cleared;
            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
