using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSpeaker : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.StopTitleBgm();
        SoundManager.Instance.PlayGameBgm();
        SoundManager.Instance.StopDieBgm();
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlayTitleBgm();
        SoundManager.Instance.StopDieBgm();
    }
}
