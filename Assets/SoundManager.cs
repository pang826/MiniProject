using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance {  get { return instance; } }
    [Header("BGM")]
    public AudioSource titleSceneBgm;
    public AudioSource gameSceneBgm;
    public AudioSource dieBgm;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioSource audioSource)
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayTitleBgm()
    {
        titleSceneBgm.Play();
    }

    public void StopTitleBgm()
    {
        if(titleSceneBgm.isPlaying)
        {
            titleSceneBgm.Stop();
        }
    }

    public void PlayGameBgm()
    {
        gameSceneBgm.Play();
    }

    public void StopGameBgm()
    {
        if(gameSceneBgm.isPlaying)
        {
            gameSceneBgm.Stop();
        }
    }

    public void PlayDieBgm()
    {
        dieBgm.Play();
    }

    public void StopDieBgm()
    {
        if (dieBgm.isPlaying)
        {
            dieBgm.Stop();
        }
    }
}
