using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
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
            SignupAsync();
        }

        public async void SignupAsync()
        {
            if (m_Signupping) return;

            m_Signupping = true;

            string username = m_UsernameField.text;
            string password = m_PwdField.text;
            string playername = m_PlayerNameField.text;

            try
            {
                await AuthManager.Instance.SignupAsync(username, password, playername);
                SceneManager.LoadScene("Lobby");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
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
                SignupAsync();
            }
        }
    }
}
