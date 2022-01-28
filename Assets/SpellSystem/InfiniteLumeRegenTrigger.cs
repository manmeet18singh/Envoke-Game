using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteLumeRegenTrigger : MonoBehaviour
{
    private bool[] mUnlockedLumes = null;
    private int[] mCurrentLumes = null;
    private int[] mMaxLumes = null;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mUnlockedLumes = LumeInventory.GetUnlockedLumes();
            mCurrentLumes = LumeInventory.GetCurrentLumes();
            mMaxLumes = LumeInventory.GetMaxLumes();
            SpellEvents.Instance.mSpellCastCallback += RefillLumes;
            RefillLumes();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for(int i = 0; i < LumeInventory.NumLumeTypes; ++i)
            {
                if (mUnlockedLumes[i])
                    LumeInventory.SetLumeAmounts(i, mCurrentLumes[i], mMaxLumes[i]);
            }

            SpellEvents.Instance.mSpellCastCallback -= RefillLumes;
        }
    }




    //TODO: Remove lume regen bug
    
    private void OnDestroy()
    {
        if(mUnlockedLumes != null)
        {
            for (int i = 0; i < LumeInventory.NumLumeTypes; ++i)
            {
                if (mUnlockedLumes[i])
                    LumeInventory.SetLumeAmounts(i, mCurrentLumes[i], mMaxLumes[i]);
            }

        }

        SpellEvents.Instance.mSpellCastCallback -= RefillLumes;

    }

    void RefillLumes()
    {
        for(int i = 0; i <= (int)Lume.SOLEIS; ++i)
        {
            LumeInventory.TryAddLume(i, LumeInventory.GetMaxLumeAmount(i));
        }
    }
}
