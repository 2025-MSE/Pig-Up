using TMPro;
using UnityEngine;

namespace MSE.Core
{
    public class UICharacterInfo : MonoBehaviour
    {
        private TMP_Text m_Text;
        private Transform m_Target;

        void Awake()
        {
            m_Text = GetComponent<TMP_Text>();
        }

        private void LateUpdate()
        {
            Vector3 targetPos = m_Target.position;
            Quaternion targetRot = m_Target.rotation;
            targetPos.y += 5f;
            transform.position = targetPos;
            transform.rotation = targetRot;
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

        public void SetInfo(string text)
        {
            m_Text.text = text;
        }
    }
}
