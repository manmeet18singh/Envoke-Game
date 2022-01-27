using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSliderSettings : MonoBehaviour
{

     public AudioMixer AudioMix;
    public Slider mSlider;
 
    
    private void Awake()
    {

        if(mSlider != null && PlayerPrefs.HasKey(mSlider.name))
        {
            float f = PlayerPrefs.GetFloat(gameObject.name, 1f);
            AudioMix.SetFloat(gameObject.name, Mathf.Log10(f) * 20);
            mSlider.value = f;
        }
    }
  
    public void SetVolume(float volume)
    {
     
        AudioMix.SetFloat(gameObject.name, Mathf.Log10(volume) * 20);

            PlayerPrefs.SetFloat(gameObject.name, volume);
    }

    
}

