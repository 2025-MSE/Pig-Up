using UnityEngine;

namespace MSE.Core
{
    public class UIModeSelectGroup : MonoBehaviour
    {
        [SerializeField]
        private UIStoryStageSelectGroup m_StoryStageSelectGroup;

        private void Start()
        {
            AudioManager.Instance.PlayAudio(AudioType.BGM, AudioManager.Instance.lobbyBGM);
        }

        public void OnStoryModePressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            m_StoryStageSelectGroup.gameObject.SetActive(true);
        }

        public void OnInfinityModePressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            UIManager.Instance.ShowToastMessage("Infinity mode is work in progress.");
        }
    }
}

