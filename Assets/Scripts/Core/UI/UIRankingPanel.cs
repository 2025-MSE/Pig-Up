using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace MSE.Core
{
    public class UIRankingPanel : MonoBehaviour
    {
        [SerializeField] private Transform m_CellRoot;
        private UIRankCell[] m_RankCells;

        private void Awake()
        {
            m_RankCells = m_CellRoot.GetComponentsInChildren<UIRankCell>(true);
        }

        private void OnEnable()
        {
            StartCoroutine(RefreshLeaderboardCoroutine());
            foreach (UIRankCell rankCell in m_RankCells)
            {
                rankCell.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            StopCoroutine(RefreshLeaderboardCoroutine());
        }

        private IEnumerator RefreshLeaderboardCoroutine(float refreshTime = 30f)
        {
            while (true)
            {
                Task<List<StageClearResultData>> task = API.GetStageRanking(DataManager.CurrStageData.Name);
                yield return new WaitUntil(() => task.IsCompleted);

                List<StageClearResultData> ranking = task.Result;
                for (int i = 0; i < ranking.Count; i++)
                {
                    m_RankCells[i].gameObject.SetActive(true);
                    m_RankCells[i].SetInfo(i + 1, ranking[i].playername, (int)ranking[i].clearTime);
                }
                for (int i = ranking.Count; i < m_RankCells.Length; i++)
                {
                    m_RankCells[i].gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(refreshTime);
            }
        }
    }
}
