using UnityEngine;
using TMPro;
using System.Collections;

public class ToastMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public CanvasGroup canvasGroup;

    public void ShowMessage(string message, float duration = 2f)
    {
        StopAllCoroutines();
        messageText.text = message;
        StartCoroutine(FadeInOut(duration));
    }

    private IEnumerator FadeInOut(float duration)
    {
        // Fade In
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
        float t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / 0.3f;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Wait
        yield return new WaitForSeconds(duration);

        // Fade Out
        t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / 0.3f);
            yield return null;
        }
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
