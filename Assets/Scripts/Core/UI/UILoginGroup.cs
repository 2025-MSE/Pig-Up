using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MSE.Core
{
    public class UILoginGroup : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_SignupGroup;

        [SerializeField] private TMP_InputField m_UsernameField;
        [SerializeField] private TMP_InputField m_PwdField;

        private bool m_Loginning = false;

        public void OnLoginPressed()
        {
            LoginAsync();
        }

        public void OnSignupPressed()
        {
            gameObject.SetActive(false);
            m_SignupGroup.gameObject.SetActive(true);
        }

        public async void LoginAsync()
        {
            if (m_Loginning) return;

            m_Loginning = true;

            string username = m_UsernameField.text;
            string password = m_PwdField.text;
            
            try
            {
                await AuthManager.Instance.LoginAsync(username, password);
                SceneManager.LoadScene("Lobby");
            } 
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            m_Loginning = false;
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
                LoginAsync();
            }
        }
    }
}
