using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReEnableLumeUnlocker : MonoBehaviour
{
    [SerializeField] private GameObject mUnlocker = null;
    //private int numUnlocks = 0;
    private bool mIsDone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsDone)
            return;

        bool allUnlocked = true;
        foreach (bool isUnlocked in LumeInventory.GetUnlockedLumes())
        {
            allUnlocked = isUnlocked && allUnlocked;
        }
        mUnlocker.SetActive(true);
        if (allUnlocked)
        {
            mUnlocker.SetActive(false);
            Destroy(mUnlocker);
            mIsDone = true;
        }
    }
}
