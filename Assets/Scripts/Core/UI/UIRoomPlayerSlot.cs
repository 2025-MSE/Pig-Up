using TMPro;
using UnityEngine;

namespace MSE.Core
{
    public class UIRoomPlayerSlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_NameText;

        public void Config(string playerName)
        {
            m_NameText.text = playerName;
        }
    }
}
