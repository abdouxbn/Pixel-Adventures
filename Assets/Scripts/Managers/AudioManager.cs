using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioManagerInstance;

    [SerializeField] private Sound[] soundCollection;
    [SerializeField] private AudioMixerGroup backgroundMusicMixer;
    [SerializeField] private AudioMixerGroup soundEffectsMixer;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (AudioManagerInstance == null) AudioManagerInstance = this;
        else Destroy(this.gameObject);

        foreach (Sound soundClip in soundCollection)
        {
            soundClip.clipAudioSource = gameObject.AddComponent<AudioSource>();

            soundClip.clipAudioSource.clip = soundClip.audioClip;
            soundClip.clipAudioSource.volume = soundClip.clipVolume;
            soundClip.clipAudioSource.pitch = soundClip.clipPitch;
            soundClip.clipAudioSource.loop = soundClip.loopClip;
            
            if (soundClip.clipName == "MainMenuTheme" || soundClip.clipName == "GameplayTheme")
            {
                soundClip.clipAudioSource.outputAudioMixerGroup = backgroundMusicMixer;
            }
            else
            {
                soundClip.clipAudioSource.outputAudioMixerGroup = soundEffectsMixer;  
            }
        }
    }

    private void Start()
    {
        PlayClip("MainMenuTheme");
    }

    public void PlayClip(string clipName)
    {
        Sound soundClip = Array.Find(soundCollection, soundClip => soundClip.clipName == clipName);
        soundClip.clipAudioSource.Play();
    }
}
