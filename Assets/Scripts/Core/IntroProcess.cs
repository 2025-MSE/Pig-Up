/// <summary>
/// Author: Dongjin Kuk
/// Description: It defines the intro process.
/// </summary>

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
            float bgmVolume = PlayerPrefs.GetFloat("bgm", 0.3f);
            float sfxVolume = PlayerPrefs.GetFloat("sfx", 0.5f);
            int qLevel = PlayerPrefs.GetInt("quality", 0);

            AudioManager.Instance.SetVolume(AudioType.BGM, bgmVolume);
            AudioManager.Instance.SetVolume(AudioType.SFX, sfxVolume);
            QualitySettings.SetQualityLevel(qLevel);

            StartCoroutine(RunProcess());
        }

        private IEnumerator RunProcess()
        {
            Task task = UnityServices.InitializeAsync();
            yield return new WaitUntil(() => task.IsCompleted);

            yield return StartCoroutine(SplashCoroutine());
            yield return new WaitForSeconds(0.5f);
            m_IntroGroup.gameObject.SetActive(true);
            AudioManager.Instance.PlayAudio(AudioType.BGM, AudioManager.Instance.titleBGM);
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
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            m_LoginGroup.gameObject.SetActive(true);
        }
    }
}
