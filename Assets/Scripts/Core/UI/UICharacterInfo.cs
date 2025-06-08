using TMPro;
using UnityEngine;

namespace MSE.Core
{
    public class UICharacterInfo : MonoBehaviour
    {
        private TMP_Text m_Text;

        void Awake()
        {
            m_Text = GetComponentInChildren<TMP_Text>();
        }

        public void SetInfo(string text)
        {
            m_Text.text = text;
        }
    }
}
