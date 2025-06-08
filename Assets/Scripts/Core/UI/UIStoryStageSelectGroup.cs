using System;
using Unity.Services.Authentication;
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
            m_StageButtons = m_ButtonRoot.GetComponentsInChildren<UIStoryStageButton>();
        }

        private async void OnEnable()
        {
            User userData = await API.UpdateUserIdAsync(AuthenticationService.Instance.PlayerId);
            
            foreach (UIStoryStageButton sbutton in m_StageButtons)
            {
                bool activated = Array.Find(userData.stageClearInfos ?? Array.Empty<UserStageClearData>(), (data) => data.stageName.Equals(sbutton.RequiredStage)) != null;
                if (sbutton.RequiredStage.IsNullOrEmpty())
                {
                    activated = true;
                }

                sbutton.Button.interactable = activated;
            }
        }

        public void OnStageButtonPressed(string stageName)
        {
            StageData stageData = DataManager.GetStageData(stageName);
            DataManager.CurrStageData = stageData;

            m_StageEnterPanel.gameObject.SetActive(true);
        }

        public void OnBackButtonPressed()
        {
            gameObject.SetActive(false);
        }
    }
}
