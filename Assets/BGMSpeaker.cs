using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSpeaker : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.StopTitleBgm();
        SoundManager.Instance.PlayGameBgm();
    }
}
