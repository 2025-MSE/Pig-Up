/// <summary>
/// Author: Hyeongyenog Lee
/// Description: It defines the dialgoue system. (Only for a pig and a wolf)
/// </summary>

using MSE.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DialogueData
{
    public int stage;
    public List<DialogueLine> dialog;
}

[System.Serializable]
public class DialogueLine
{
    public string type;
    public string text;
    public string animation;
}

[System.Serializable]
public class CharacterInfo
{
    public string displayName;
    public Color backgroundColor;
}



public class DialogueManaager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image nameBgImage;
    public GameObject pig;
    public GameObject wolf;
    private float typingSpeed = 0.05f;

    private DialogueData dialogueData;
    private int currentIndex = 0;

    public string nextSceneName = "";

    private Dictionary<string, CharacterInfo> characterInfoMap = new Dictionary<string, CharacterInfo>
    {
    { "pig", new CharacterInfo { displayName = "Pig", backgroundColor = new Color(1f, 0.8f, 0.8f) } },
    { "wolf", new CharacterInfo { displayName = "Wolf", backgroundColor = new Color(0.5f, 0.5f, 0.5f) } },
    };

    public static Action OnDialogueEnded;

    private void Start()
    {
        LoadDialogueForStage(DataManager.CurrStageData.Name);
    }

    private void OnEnable()
    {
        AudioManager.Instance.PlayAudio(AudioType.BGM, AudioManager.Instance.storyBGM);
    }

    private void OnDisable()
    {
        AudioManager.Instance.PlayAudio(AudioType.BGM, AudioManager.Instance.lobbyBGM);
    }

    public void LoadDialogueForStage(string stage)
    {
        string path = $"Dialogue/{stage}";
        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        if (jsonFile == null)
        {
            Debug.LogError($"Dialogue JSON not found at Resources/{path}.json");
            return;
        }

        dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        currentIndex = 0;
        StartCoroutine(ShowNextLine());
    }
    public void LoadDialogueFromJson(TextAsset jsonFile)
    {
        dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        currentIndex = 0;
        StartCoroutine(ShowNextLine());
    }
    IEnumerator ShowNextLine()
    {
        if (currentIndex >= dialogueData.dialog.Count)
        {
            dialogueText.text = "";
            pig.SetActive(false);
            wolf.SetActive(false);
            SceneManager.UnloadSceneAsync("DialogueScene");
            OnDialogueEnded?.Invoke();
            yield break;
        }

        DialogueLine line = dialogueData.dialog[currentIndex];
        currentIndex++;

        if (characterInfoMap.TryGetValue(line.type, out CharacterInfo info))
        {
            nameText.text = info.displayName;
            nameBgImage.color = info.backgroundColor;
        }
        else
        {
            nameText.text = line.type;
            nameBgImage.color = Color.white;
        }

        if (line.type == "pig")
        {
            pig.SetActive(true);
            wolf.SetActive(false);
        }
        else if (line.type == "wolf")
        {
            wolf.SetActive(true);
            pig.SetActive(false);
        }
        else
        {
            pig.SetActive(false);
            wolf.SetActive(false);
        }

        if (!string.IsNullOrEmpty(line.animation))
        {
            Animator targetAnimator = GetAnimatorByType(line.type);
            if (targetAnimator != null)
            {
                GameObject go = targetAnimator.gameObject;
                if (!go.activeInHierarchy)
                {
                    go.SetActive(true);
                }

                targetAnimator.Play(line.animation);
            }
        }

        if (!string.IsNullOrEmpty(line.text))
        {
            yield return StartCoroutine(TypeText(line.text));
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowNextLine());
    }
    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    private Animator GetAnimatorByType(string type)
    {
        if (type == "pig" && pig != null)
            return pig.GetComponent<Animator>();
        else if (type == "wolf" && wolf != null)
            return wolf.GetComponent<Animator>();
        return null;
    }

}
