using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    public AudioSource backgroundSource;
    public AudioSource btnSource;

    public GameObject backSlider;
    public GameObject effectSlider;

    private void Awake()
    {
        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    public void SetBackMusicVol(float vol)
    {
        backgroundSource.volume = vol;
    }

    public void SetBtnSoundVol(float vol)
    {
        btnSource.volume = vol;
    }

    public void OnBtnSound()
    {
        btnSource.Play();
    }
}
