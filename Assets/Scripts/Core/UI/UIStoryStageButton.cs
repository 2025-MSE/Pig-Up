/// <summary>
/// Author: Dongjin Kuk
/// Description: Story Stage Button. It has requiredStage variable, and will be enable/disabled by checking it's clear status.
/// </summary>

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
        public Button Button
        {
            get
            {
                m_Button ??= GetComponent<Button>();
                return m_Button;
            }
        }

        private void Awake()
        {
            m_Button = GetComponent<Button>();
        }
    }
}
