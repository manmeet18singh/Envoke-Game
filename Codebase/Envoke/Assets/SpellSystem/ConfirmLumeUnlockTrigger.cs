using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmLumeUnlockTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpellEvents.Instance.ConfirmLumeUnlock(LumeUnlockManager.Instance.LumeChose);
            
        }
    }
    
}
