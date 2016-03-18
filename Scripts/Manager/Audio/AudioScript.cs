using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioScript : MonoBehaviour {

    public static AudioScript m_Audio;

    AudioSource m_AudioSrc;

    void Awake()
    {
        m_Audio = this;
    }

    void Start() {
        m_AudioSrc = GetComponent<AudioSource>();
	}

    public void PlaySoundFx(AudioClip soundfx, float volume)
    {
        m_AudioSrc.PlayOneShot(soundfx, volume);

    }

    IEnumerator FadeOutCurrentAudio()
    {
        while (m_AudioSrc.volume >= 0.1f)
        {
            m_AudioSrc.volume = Mathf.Lerp(m_AudioSrc.volume, 0.1f, Time.deltaTime * 2);

            yield return null;
        }

        if(m_AudioSrc.volume <= 0.1f)
        {
            StartCoroutine(FadeInAudio());
        }
        
    }

    IEnumerator FadeInAudio()
    {
        while (m_AudioSrc.volume <= 0.25f)
        {
            m_AudioSrc.volume = Mathf.Lerp(m_AudioSrc.volume, 0.25f, Time.deltaTime * 2);

            yield return null;
        }
    }

}
