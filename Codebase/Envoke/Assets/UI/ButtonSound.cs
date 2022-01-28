using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void OnClickSound()
    {
        AudioManager.instance.Play("Button");
    }

    public void OnClickSound(string _sound)
    {
        AudioManager.instance.Play(_sound);
    }
}
