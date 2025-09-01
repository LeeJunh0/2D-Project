using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource effectSource;

    public void Init()
    {
        bgmSource.Play();
    }

    public void SetBGMVolum(float value)
    {
        bgmSource.volume = value;
    }

    public void SetEffectVolume(float value)
    {
        effectSource.volume = value;
    }

    public void SetMute(bool isMute)
    {
        bgmSource.mute = isMute;
        effectSource.mute = isMute;
    }

    public void EffectPlay(string clipName)
    {
        effectSource.PlayOneShot(MainManager.Addressable.Load<AudioClip>(clipName));
    }
}
