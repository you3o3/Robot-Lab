using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static string[] choices { get; } =
    {
        "UIButtonSelectSound",
        "TextButtonSelectSound",
        "NontextButtonSelectSound",
        "LaserShootingSound",
        "WinSound",
        "LoseSound"
    };

    [SerializeField] [StringInList(typeof(AudioPlayer), "get_choices")] private string audioToPlay;

    public void Play()
    {
        AudioManager.Instance.Play(audioToPlay);
    }
}
