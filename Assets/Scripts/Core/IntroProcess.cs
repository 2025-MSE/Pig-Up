/**
 * Owner: Dongjin Kuk
 */

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace MSE.Core
{
    public class IntroProcess : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_SplashGroup;
        [SerializeField]
        private CanvasGroup m_IntroGroup;

        private void Awake()
        {
            m_SplashGroup.alpha = 0f;
            m_IntroGroup.gameObject.SetActive(false);
        }

        private void Start()
        {
            StartCoroutine(RunProcess());
        }

        private IEnumerator RunProcess()
        {
            yield return StartCoroutine(SplashCoroutine());
            m_IntroGroup.gameObject.SetActive(true);
        }

        private IEnumerator SplashCoroutine()
        {
            float alpha = 0f;
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime;
                alpha = Mathf.Clamp01(elapsedTime / 1f);

                m_SplashGroup.alpha = alpha;
                yield return null;
            }

            m_SplashGroup.alpha = 1f;

            yield return new WaitForSeconds(1);

            elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime;
                alpha = Mathf.Clamp01(1f - elapsedTime / 1f);

                m_SplashGroup.alpha = alpha;
                yield return null;
            }

            m_SplashGroup.alpha = 0f;
            m_SplashGroup.gameObject.SetActive(false);
        }
    }
}
