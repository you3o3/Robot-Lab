using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [Header("Background Volume")]
    [SerializeField] private Slider backgroundSlider;
    [SerializeField] private TextMeshProUGUI backgroundVolumeText;

    [Header("Sound Effect Volume")]
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private TextMeshProUGUI soundEffectVolumeText;
    
    public float bgVolume = 1, seVolume = 1;

    private void Start()
    {
        bgVolume = BGMusicManager.Instance.GetAllVolume();
        seVolume = AudioManager.Instance.totalVolume;

        backgroundSlider.value = bgVolume;
        soundEffectSlider.value = seVolume;
    }

    private void Update()
    {
        SetTextPercentage(backgroundVolumeText, bgVolume);
        SetTextPercentage(soundEffectVolumeText, seVolume);
    }

    public void SetBackgroundVolume(float volume)
    {
        bgVolume = volume;
        BGMusicManager.Instance.SetAllVolume(volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        seVolume = volume;
        AudioManager.Instance.totalVolume = volume;
    }

    private void SetTextPercentage(TextMeshProUGUI text, float percentage)
    {
        text.text = (percentage * 100).ToString("0") + "%";
    }
}
