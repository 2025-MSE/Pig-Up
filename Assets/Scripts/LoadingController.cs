using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
        StartCoroutine(LoadingRoutine());
    }

    IEnumerator LoadingRoutine()
    {
        float loadProgress = 0f;

        string randomLoadingMessage = loadingMessages[Random.Range(0, loadingMessages.Length)];
        yield return StartCoroutine(TypeText(randomLoadingMessage));

        while (loadProgress < 1f)
        {
            loadProgress += Time.deltaTime * 0.5f;
            loadingBar.value = loadProgress;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        loadingText.text = "";
        yield return StartCoroutine(TypeText(completeMessage));
    }

    IEnumerator TypeText(string message)
    {
        loadingText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            loadingText.text += message[i];
            yield return new WaitForSeconds(0.08f);
        }
    }
}
