using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSE.Core
{
    public class UILoginGroup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_UsernameField;
        [SerializeField] private TMP_InputField m_IdField;
        [SerializeField] private TMP_InputField m_PwdField;

        public void OnSignUpPressed()
        {
            StartCoroutine(SignUp());
        }

        public void OnLoginPressed()
        {
            StartCoroutine(Login());
        }

        public IEnumerator SignUp()
        {
            string username = m_UsernameField.text;
            string email = m_IdField.text;
            string password = m_PwdField.text;
            yield return StartCoroutine(AuthManager.Instance.SignUpCoroutine(username, password, email));

            SceneManager.LoadScene("Lobby");
        }

        public IEnumerator Login()
        {
            string username = m_UsernameField.text;
            string password = m_PwdField.text;
            yield return StartCoroutine(AuthManager.Instance.LoginCoroutine(username, password));

            SceneManager.LoadScene("Lobby");
        }
    }
}
