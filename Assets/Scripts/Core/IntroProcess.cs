/**
 * Owner: Dongjin Kuk
 */

using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;

namespace MSE.Core
{
    public class IntroProcess : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_SplashGroup;
        [SerializeField]
        private CanvasGroup m_IntroGroup;
        [SerializeField]
        private CanvasGroup m_LoginGroup;

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
            Task task = UnityServices.InitializeAsync();
            yield return new WaitUntil(() => task.IsCompleted);

            yield return StartCoroutine(SplashCoroutine());
            yield return new WaitForSeconds(0.5f);
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

        public void OnStartTouched()
        {
            m_LoginGroup.gameObject.SetActive(true);
        }
    }
}
