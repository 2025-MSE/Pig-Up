using System.Collections.Generic;
using UnityEngine;

namespace MSE.Core
{
    public class UICharacterInfoHandler : MonoBehaviour
    {
        [SerializeField] private UICharacterInfo m_InfoPrefab;
        [SerializeField] private Transform m_InfoRoot;

        private void OnDisable()
        {
            foreach (Transform trans in m_InfoRoot)
            {
                Destroy(trans.gameObject);
            }
        }

        public void CreateInfos(List<Transform> transforms)
        {
            for (int i = 0; i < transforms.Count; i++)
            {
                UICharacterInfo newInfo = Instantiate(m_InfoPrefab);
                newInfo.transform.SetParent(m_InfoRoot, true);

                newInfo.SetTarget(transforms[i]);
            }
        }
    }
}
