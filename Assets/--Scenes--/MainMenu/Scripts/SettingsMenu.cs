using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SettingsMenu : MonoBehaviour
{

    public AudioMixer AudioMix;
   

    public void SetVolume(float volume)
    {
        AudioMix.SetFloat(gameObject.name, Mathf.Log10(volume) * 20);

    }

    
}

