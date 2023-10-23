using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeControl : MonoBehaviour
{
    [SerializeField] private string audioMixerName;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    [SerializeField] private float volumeMultiplier;


    public void SetGameVolume()
    {
        slider.onValueChanged.AddListener(SliderValue);
        slider.minValue = 0.00000001f;
        slider.value = PlayerPrefs.GetFloat(audioMixerName, slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(audioMixerName, slider.value);
    }

    private void SliderValue(float value)
    {
        audioMixer.SetFloat(audioMixerName, Mathf.Log10(value) * volumeMultiplier);
    }
}
