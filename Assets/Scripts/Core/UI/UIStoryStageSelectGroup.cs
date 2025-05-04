using UnityEngine;

namespace MSE.Core
{
    public class UIStoryStageSelectGroup : MonoBehaviour
    {
        [SerializeField]
        private UILobbyGroup m_LobbyGroup;

        public void OnStageButtonPressed(string stageName)
        {
            StageData stageData = DataManager.GetStageData(stageName);
            DataManager.CurrStageData = stageData;

            m_LobbyGroup.gameObject.SetActive(true);
        }
    }
}
