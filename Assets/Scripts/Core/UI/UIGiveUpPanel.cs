/// <summary>
/// Author: Dongjin Kuk
/// Description: This class manages the give up panel.
/// </summary>

using System;
using UnityEngine;

namespace MSE.Core
{
    public class UIGiveUpPanel : MonoBehaviour
    {
        public Action OnGiveUp;

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
        }
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnBackPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            gameObject.SetActive(false);
        }

        public void OnGiveUpPressed()
        {
            AudioManager.Instance.PlayAudio(AudioType.SFX, AudioManager.Instance.clickSFX);
            gameObject.SetActive(false);
            OnGiveUp?.Invoke();
        }
    }
}
