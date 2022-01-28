using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string Name;
    
    
    public bool Loop;
    public AudioClip Clip;
    public AudioMixerGroup Mixer;
    public bool IgnoreAudioListener;
 
    [HideInInspector]
    public AudioSource Source;
    

    [Range(0f, 1f)]
    public float volume;

    [Range(1f, 3f)]
    public float pitch;


}



