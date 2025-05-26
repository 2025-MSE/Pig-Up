using TMPro;
using UnityEngine;

namespace MSE.Core
{
    public class UICharacterInfo : MonoBehaviour
    {
        private TMP_Text m_Text;
        private Transform m_Target;
        private Transform m_ViewTransform;

        void Awake()
        {
            m_Text = GetComponentInChildren<TMP_Text>();
        }

        private void LateUpdate()
        {
            Vector3 targetPos = m_Target.position;
            targetPos.y += 5f;
            transform.position = targetPos;
            transform.rotation = m_ViewTransform.rotation;
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }
        public void SetViewTransform(Transform viewTransform)
        {
            m_ViewTransform = viewTransform;
        }

        public void SetInfo(string text)
        {
            m_Text.text = text;
        }
    }
}
