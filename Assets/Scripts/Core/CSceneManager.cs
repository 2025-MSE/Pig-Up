using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSE.Core
{
    public class CSceneManager
    {
        public static string TargetSceneName;

        public static void LoadScene(string sceneName)
        {
            TargetSceneName = sceneName;
            SceneManager.LoadScene("LoadingScene");
            
        }
    }
}
