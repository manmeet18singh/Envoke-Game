using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlinkController : MonoBehaviour
{
    [SerializeField]
    BlinkDamage mBlinkDamage = null;
    [SerializeField]
    private ParticleSystem mEffect = null;
    [SerializeField] private string[] mSFX;

    public void Initialize(BlinkData _data)
    {
        mEffect.Play(true);
        AudioManager.PlayRandomSFX(mSFX);
        mBlinkDamage.Initialize(_data);
    }
}