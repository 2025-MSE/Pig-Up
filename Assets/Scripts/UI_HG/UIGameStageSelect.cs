using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameStageSelect : MonoBehaviour
{
    [SerializeField] private Button[] stageButtons;
    [SerializeField] private TMP_Text messageText;

    private bool[] stageUnlocked;

    private void Start()
    {
        // For testing: simulate user who has cleared 1 stage
        SetStageUnlockState(0);
    }


    public void SetStageUnlockState(int clearedStage)
    {
        stageUnlocked = new bool[stageButtons.Length];

        for (int i = 0; i < stageButtons.Length; i++)
        {
            bool unlocked = i <= clearedStage;
            stageUnlocked[i] = unlocked;

            stageButtons[i].interactable = true;

            Image buttonImage = stageButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                if (unlocked)
                {
                    buttonImage.color = Color.white;
                }
                else
                {
                    buttonImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
            }
        }
    }

    public void OnStageButtonPressed(int stageIndex)
    {
        if (stageUnlocked == null || !stageUnlocked[stageIndex])
        {
            ShowMessage($"[LOCK] Stage {stageIndex + 1} is locked.");
            return;
        }

        Debug.Log($"[SELCET] Stage {stageIndex + 1} is selected.");

        // Request stage scene loading
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), duration);
    }

    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }
}
