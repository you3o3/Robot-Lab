using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopoutScreen : MonoBehaviour
{
    AudioPlayer audioPlayer;

    private void Awake()
    {
        audioPlayer = GetComponent<AudioPlayer>();
        if (audioPlayer == null) Debug.Log("audio player is null on popout screen");
    }

    private void OnEnable()
    {
        BGMusicManager.Instance.SetVolume(0);
        audioPlayer.Play();
    }
}
