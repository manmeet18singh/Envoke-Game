using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailSlow : MonoBehaviour
{

    private HailData mSpellData;

    public void Initialize(HailData _data)
    {
        mSpellData = _data;
    }


    private void OnTriggerEnter(Collider other)
    {
        IAffectable affectable = other.GetComponent<IAffectable>();
        if (affectable != null)
        {
            affectable.SlowOverTime(mSpellData.TimeTillImmobile, mSpellData.MinMoveSpeed, mSpellData.CanFreeze, mSpellData.FreezeDuration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IAffectable affectable = other.GetComponent<IAffectable>();
        if (affectable != null)
        {
            affectable.StopSlowOverTime();
        }
    }
}
