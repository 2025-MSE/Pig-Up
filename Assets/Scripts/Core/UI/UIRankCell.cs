using TMPro;
using UnityEngine;

namespace MSE.Core
{
    public class UIRankCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_RankText;
        [SerializeField] private TMP_Text m_PlayerNameText;
        [SerializeField] private TMP_Text m_TimeText;

        public void SetInfo(int rank, string playerName, int time)
        {
            m_RankText.text = rank.ToString();
            m_PlayerNameText.text = playerName;

            int min = time / 60;
            int sec = time % 60;
            m_TimeText.text = $"{min.ToString().PadLeft(2, '0')}:{sec.ToString().PadLeft(2, '0')}";
        }
    }
}
