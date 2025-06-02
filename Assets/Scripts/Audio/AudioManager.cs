using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip titleBGM;
    public AudioClip gameBGM;
    public AudioClip storyBGM;
    public AudioClip lobbyBGM;

    [Header("SFX")]
    public AudioClip blockPlaceSFX;
    public AudioClip blockGrabSFX;
    public AudioClip rankPopupSFX;
    public AudioClip clickSFX;
    public AudioClip jumpSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(AudioType type, AudioClip clip)
    {
        if (clip == null) return;

        if (type == AudioType.BGM)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else if (type == AudioType.SFX)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
