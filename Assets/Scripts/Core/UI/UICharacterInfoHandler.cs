using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSE.Core
{
    public class UICharacterInfoHandler : MonoBehaviour
    {
        [SerializeField] private UICharacterInfo m_InfoPrefab;
        [SerializeField] private Transform m_InfoRoot;

        private Dictionary<string, UICharacterInfo> m_Infos = new Dictionary<string, UICharacterInfo>();

        public static Action<string, Transform, string, Transform> OnCharacterActivated;
        public static Action<string> OnCharacterDeactivated;

        private void OnEnable()
        {
            OnCharacterActivated += CreateInfo;
            OnCharacterDeactivated += RemoveInfo;
        }
        private void OnDisable()
        {
            OnCharacterActivated -= CreateInfo;
            OnCharacterDeactivated -= RemoveInfo;
        }

        private void CreateInfo(string id, Transform target, string text, Transform viewTransform)
        {
            UICharacterInfo newInfo = Instantiate(m_InfoPrefab);
            newInfo.transform.SetParent(m_InfoRoot, true);
            newInfo.SetTarget(target);
            newInfo.SetViewTransform(viewTransform);
            newInfo.SetInfo(text);

            m_Infos.Add(id, newInfo);
        }

        private void RemoveInfo(string id)
        {
            UICharacterInfo info = m_Infos[id];
            if (info != null)
            {
                Destroy(info.gameObject);
                m_Infos.Remove(id);
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
