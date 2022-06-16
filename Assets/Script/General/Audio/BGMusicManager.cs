using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicManager : SingletonWMonoBehaviour<BGMusicManager>
{
    public static BGMusicManager Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, true);
    }

    private DynamicPlayer dynPlayer;

    private void Start()
    {
        dynPlayer = transform.GetChild(0).gameObject.GetComponent<DynamicPlayer>();
    }

    // volume ranges from 0 to 1
    public void SwitchPart(int part, float volume = 1)
    {
        dynPlayer.SwitchParts(part);
        dynPlayer.SetSourceVolume(part, volume);
    }

    public void SetVolume(float volume)
    {
        dynPlayer.SetSourceVolume(dynPlayer.currentPart, volume);
    }

    public void SetAllVolume(float volume)
    {
        dynPlayer.totalVolume = volume;
        SetVolume(1);
    }

    public float GetAllVolume()
    {
        return dynPlayer.totalVolume;
    }

    public void Play()
    {
        dynPlayer.PlayMusic();
    }

    public void Stop()
    {
        dynPlayer.StopMusic();
    }

    public int GetCurrentPart()
    {
        return dynPlayer.currentPart;
    }
}
