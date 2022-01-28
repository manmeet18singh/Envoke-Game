using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerFences : MonoBehaviour
{
    [SerializeField]
    Animator mAnim;

    void Start()
    {
        if(SavePointSystem.NumLumesUnlocked >= LumeInventory.NumLumeTypes)
        {
            LowerFence();
            return;
        }

        SpellEvents.Instance.mConfirmedLumeUnlockCallback += LowerFence;
    }


    private void OnDestroy()
    {
        SpellEvents.Instance.mConfirmedLumeUnlockCallback -= LowerFence;
    }

    void LowerFence()
    {
        mAnim.enabled = true;
    }


}
