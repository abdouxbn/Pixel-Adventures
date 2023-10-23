using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string clipName;
    public AudioClip audioClip;
    public bool loopClip;
    [Range(0f, 1f)]
    public float clipVolume;
    [Range(0.1f, 3f)]
    public float clipPitch; 

    [HideInInspector] public AudioSource clipAudioSource;

}
