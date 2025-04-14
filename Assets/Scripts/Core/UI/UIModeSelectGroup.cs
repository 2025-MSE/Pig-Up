using UnityEngine;

namespace MSE.Core
{
    public class UIModeSelectGroup : MonoBehaviour
    {
        [SerializeField]
        private UIStoryStageSelectGroup m_StoryStageSelectGroup;

        public void OnStoryModePressed()
        {
            m_StoryStageSelectGroup.gameObject.SetActive(true);
        }
    }
}

