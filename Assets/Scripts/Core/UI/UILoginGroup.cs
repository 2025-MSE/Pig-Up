using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSE.Core
{
    public class UILoginGroup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_UsernameField;
        [SerializeField] private TMP_InputField m_PwdField;

        public void OnLoginPressed()
        {
            StartCoroutine(Login());
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
