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
    [SerializeField] private Button exitButton;

    public void ShowResult(string stageName, float clearTime, bool isClear)
    {
        AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.rankPopupSFX);

        gameObject.SetActive(true);

        stageNameText.text = $"Stage {stageName}";

        int min = Mathf.FloorToInt(clearTime / 60);
        int sec = Mathf.FloorToInt(clearTime % 60);

        timeText.text = $"{min.ToString().PadLeft(2, '0')}:{sec.ToString().PadLeft(2, '0')}";

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


    public async void OnExitPressed()
    {
        AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);

        var myLobby = LobbyManager.Instance.MyLobby;
        RelayManager.Instance.Shutdown();
        await LobbyManager.Instance.LeaveLobby(myLobby.Id);

        SceneManager.LoadScene("Lobby");
    }
}
