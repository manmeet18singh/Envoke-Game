using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectVFX : MonoBehaviour
{
    #region State Variables
    private float mDuration;
    private float mTimeElapsed = 0;
    private bool mHasBeenInitiliazed = false;
    private GameObject mAttachment;
    #endregion

    public void InitializeVFX(float _duration, GameObject _attachment)
    {
        mDuration = _duration;
        mHasBeenInitiliazed = true;
        mAttachment = _attachment;
    }

    private void Update()
    {
        if (mHasBeenInitiliazed)
        {
            if (mAttachment == null || mTimeElapsed >= mDuration)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = mAttachment.transform.position;
                mTimeElapsed += Time.deltaTime;
            }
        }
    }
}
