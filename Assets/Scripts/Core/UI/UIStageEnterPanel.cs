/// <summary>
/// Author: Dongjin Kuk
/// Description: Stage Enter Panel (UI Panel)
/// </summary>

using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSE.Core
{
    public class UIStageEnterPanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_CanvasObj;
        [SerializeField] private UILobbyGroup m_LobbyGroup;

        public void OnLobbyPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            m_LobbyGroup.gameObject.SetActive(true);
        }

        public void OnStoryPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            SceneManager.LoadScene("DialogueScene", LoadSceneMode.Additive);
            DialogueManaager.OnDialogueEnded += OnDialogueEnded;
            m_CanvasObj.SetActive(false);
        }

        public void OnBackPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            gameObject.SetActive(false);
        }
        
        private void OnDialogueEnded()
        {
            DialogueManaager.OnDialogueEnded -= OnDialogueEnded;
            m_CanvasObj.SetActive(true);
        }
    }
}
