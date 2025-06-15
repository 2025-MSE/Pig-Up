/// <summary>
/// Author: Dongjin Kuk
/// Description: Settings Panel (UI Panel). Can control bgm/sfx volume.
/// </summary>

using UnityEngine;
using UnityEngine.UI;

namespace MSE.Core
{
    public class UISettingsPanel : MonoBehaviour
    {
        [SerializeField] private Transform m_GraphicsToggleRoot;
        private Toggle[] m_GraphicsToggles;

        [SerializeField] private Slider m_BGMVolumeSlider;
        [SerializeField] private Slider m_SFXVolumeSlider;

        private void Awake()
        {
            m_GraphicsToggles = m_GraphicsToggleRoot.GetComponentsInChildren<Toggle>(true);
            for (int i = 0; i < m_GraphicsToggles.Length; i++)
            {
                m_GraphicsToggles[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnGraphicsToggleChanged(i);
                });
            }
        }

        private void OnEnable()
        {
            int level = QualitySettings.GetQualityLevel();
            foreach (Toggle toggle in m_GraphicsToggles)
            {
                toggle.isOn = false;
            }
            m_GraphicsToggles[level].isOn = true;

            m_BGMVolumeSlider.value = AudioManager.Instance.BGMVolume;
            m_SFXVolumeSlider.value = AudioManager.Instance.SFXVolume;
        }

        public void OnGraphicsToggleChanged(int level)
        {
            QualitySettings.SetQualityLevel(level);
            PlayerPrefs.SetInt("quality", level);
        }

        public void OnBGMVolumeSliderChanged()
        {
            float volume = m_BGMVolumeSlider.value;
            AudioManager.Instance.SetVolume(AudioType.BGM, volume);
            PlayerPrefs.SetFloat("bgm", volume);
        }

        public void OnSFXVolumeSliderChanged()
        {
            float volume = m_SFXVolumeSlider.value;
            AudioManager.Instance.SetVolume(AudioType.SFX, volume);
            PlayerPrefs.SetFloat("sfx", volume);
        }

        public void OnCloseButtonPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            gameObject.SetActive(false);
        }
    }
}
