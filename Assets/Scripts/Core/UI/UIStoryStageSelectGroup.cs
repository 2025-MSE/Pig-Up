/// <summary>
/// Author: Dongjin Kuk
/// Description: Story Stage Select Group (UI Panel). It checks the player's stage clear status and enable/disable the stage button.
/// </summary>

using UnityEngine;
using WebSocketSharp;

namespace MSE.Core
{
    public class UIStoryStageSelectGroup : MonoBehaviour
    {
        [SerializeField]
        private UIStageEnterPanel m_StageEnterPanel;

        [SerializeField]
        private Transform m_ButtonRoot;
        private UIStoryStageButton[] m_StageButtons;

        private void Awake()
        {
            m_StageButtons = m_ButtonRoot.GetComponentsInChildren<UIStoryStageButton>(true);
        }

        private async void OnEnable()
        {
            foreach (UIStoryStageButton sbutton in m_StageButtons)
            {
                if (sbutton.RequiredStage.IsNullOrEmpty())
                {
                    sbutton.Button.interactable = true;
                    continue;
                }

                bool activated = await API.IsStageClearedAsync(sbutton.RequiredStage);
                sbutton.Button.interactable = activated;
            }
        }

        public void OnStageButtonPressed(string stageName)
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);

            StageData stageData = DataManager.GetStageData(stageName);
            DataManager.CurrStageData = stageData;

            m_StageEnterPanel.gameObject.SetActive(true);
        }

        public void OnBackButtonPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            gameObject.SetActive(false);
        }
    }
}
