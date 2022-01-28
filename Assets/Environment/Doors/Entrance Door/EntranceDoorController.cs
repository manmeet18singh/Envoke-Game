using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoorController : MonoBehaviour
{
    [SerializeField] Animator mAnimator = null;

    private bool mIsListening = false;

    // Start is called before the first frame update
    void Start()
    {
        if (SavePointSystem.NumLumesUnlocked == 3)
            OpenDoor();
        else
        {
            SpellEvents.Instance.mConfirmedLumeUnlockCallback += OpenDoor;
            mIsListening = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (mIsListening)
            SpellEvents.Instance.mConfirmedLumeUnlockCallback -= OpenDoor;
    }

    void OpenDoor()
    {
        //Debug.Log("Opening door!");
        mAnimator.enabled = true;
    }
}
