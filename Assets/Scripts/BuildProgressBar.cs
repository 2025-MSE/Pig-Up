using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI progressText;

    public void SetProgress(float value)
    {
        value = Mathf.Clamp01(value);
        slider.value = value;

        if (progressText != null)
        {
            if (value >= 1f)
                progressText.text = "Done!";
            else
                progressText.text = $"House building... {Mathf.RoundToInt(value * 100)}%";
        }
    }
}
