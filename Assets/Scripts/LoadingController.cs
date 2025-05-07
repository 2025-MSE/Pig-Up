using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using MSE.Core;

public class LoadingController : MonoBehaviour
{
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;

    private string[] loadingMessages = new string[]
    {
        "Stacking bricks...",
        "Looking for the hammer...",
        "Preparing to raise the roof..."
    };

    private string completeMessage = "Load Complete!";

    private void Start()
    {
        string randomLoadingMessage = loadingMessages[Random.Range(0, loadingMessages.Length)];
        loadingText.text = randomLoadingMessage;
        StartCoroutine(LoadingRoutine());
    }

    IEnumerator LoadingRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        AsyncOperation aop = SceneManager.LoadSceneAsync(CSceneManager.TargetSceneName);
        aop.allowSceneActivation = false;

        while (aop.progress < 0.9f)
        {
            loadingBar.value = aop.progress;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        aop.allowSceneActivation = true;
    }
}
