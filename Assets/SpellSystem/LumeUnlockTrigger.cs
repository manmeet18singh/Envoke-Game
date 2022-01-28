using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumeUnlockTrigger : MonoBehaviour
{
    [SerializeField]
    Lume mLumeType = Lume.EDUR;
    [SerializeField] private string[] mSfx = null;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LumeUnlockManager.Instance.LumePicked((int)mLumeType);
            AudioManager.PlayRandomSFX(mSfx);
        }
    }

}
