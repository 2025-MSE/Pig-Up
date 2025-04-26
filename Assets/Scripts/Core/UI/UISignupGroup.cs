using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MSE.Core
{
    public class UISignupGroup : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_LoginGroup;

        [SerializeField] private TMP_InputField m_UsernameField;
        [SerializeField] private TMP_InputField m_PwdField;
        [SerializeField] private TMP_InputField m_PlayerNameField;

        private bool m_Signupping = false;

        void OnEnable()
        {
            m_UsernameField.text = string.Empty;
            m_PwdField.text = string.Empty;
            m_PlayerNameField.text = string.Empty;

            m_Signupping = false;
        }

        public void OnSignupPressed()
        {
            StartCoroutine(Signup());
        }

        public IEnumerator Signup()
        {
            if (m_Signupping) yield break;

            m_Signupping = true;

            string username = m_UsernameField.text;
            string password = m_PwdField.text;
            string playername = m_PlayerNameField.text;

            bool succeeded = false;
            yield return StartCoroutine(AuthManager.Instance.SignUpCoroutine(username, password, playername, (success) => {
                succeeded = success;
            }));

            if (succeeded)
            {
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                Debug.Log("Signup error");
            }

            m_Signupping = false;
        }
        
        public void OnTab(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameObject selObj = EventSystem.current.currentSelectedGameObject;
                
                if (selObj == null)
                {
                    m_UsernameField.Select();
                }
                else
                {
                    Selectable nextSelectable = selObj.GetComponent<Selectable>().FindSelectableOnDown();
                    nextSelectable?.Select();
                }
            }
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                StartCoroutine(Signup());
            }
        }
    }
}
