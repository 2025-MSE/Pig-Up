using UnityEngine;
using UnityEngine.UI;

namespace MSE.Core
{
    public class UIStoryStageButton : MonoBehaviour
    {
        [SerializeField]
        private string m_RequiredStage;
        public string RequiredStage => m_RequiredStage;

        private Button m_Button;
        public Button Button => m_Button;

        private void Awake()
        {
            m_Button = GetComponent<Button>();
        }
    }
}
