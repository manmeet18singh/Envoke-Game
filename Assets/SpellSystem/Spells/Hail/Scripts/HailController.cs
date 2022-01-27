using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HailController : MonoBehaviour
{
    [SerializeField]
    HailDamage mHailDamage = null;
    [SerializeField]
    HailSlow mHailSlow = null;
    [SerializeField]
    VisualEffect mEffect = null;

    public void Initialize(HailData _data)
    {
        mEffect.enabled = true;
        mHailSlow.Initialize(_data);
        mHailDamage.Initialize(_data);
    }
}
