using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MSE.Core;

public class UIStageResult : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Star UI")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starFilled;
    [SerializeField] private Sprite starEmpty;

    [Header("Buttons")]
    [SerializeField] private Button nextStageButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button exitButton;

    public void ShowResult(int stageIndex, float clearTime, bool isClear)
    {
        gameObject.SetActive(true);

        stageNameText.text = $"Stage {stageIndex + 1}";
        timeText.text = $"{Mathf.FloorToInt(clearTime / 60)}:{Mathf.FloorToInt(clearTime % 60)}";

        resultText.text = isClear ? "CLEAR!" : "FAIL...";
        resultText.color = isClear ? Color.yellow : Color.red;

        int starCount = CalculateStars(clearTime); // Temporary local calculation
        SetStars(starCount);
    }

    private int CalculateStars(float clearTime)
    {
        //Temporary logic for star count based on clearTime
        if (clearTime <= 60f) return 3;
        if (clearTime <= 120f) return 2;
        if (clearTime <= 180f) return 1;
        return 0;
    }

    private void SetStars(int count)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = i < count ? starFilled : starEmpty;
        }
    }

    public void OnNextStagePressed()
    {
        Debug.Log("Next stage button pressed.");
    }


    public void OnRetryPressed()
    {
        Debug.Log("Retry button pressed.");
    }

    public async void OnExitPressed()
    {
        var myLobby = LobbyManager.Instance.MyLobby;
        RelayManager.Instance.Shutdown();
        await LobbyManager.Instance.LeaveLobby(myLobby.Id);

        SceneManager.LoadScene("Lobby");
    }
}
