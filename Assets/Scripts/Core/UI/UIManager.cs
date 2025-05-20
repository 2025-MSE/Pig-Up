using UnityEngine;

namespace MSE.Core
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager s_Instance;
        public static UIManager Instance => s_Instance;

        [SerializeField] private Canvas m_Canvas2D;
        [SerializeField] private Canvas m_Canvas3D;

        [SerializeField]
        private UICharacterInfoHandler m_InfoHandler;
        public UICharacterInfoHandler InfoHandler => m_InfoHandler;

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                s_Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
