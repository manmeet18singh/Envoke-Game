using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAnimationController : MonoBehaviour
{
    [SerializeField] private float mForwardAmount = 100;
    [SerializeField] private float mAnimationTime = 3;
    [SerializeField] private PortalSpawner mPortal = null;

    #region State Variables
    private Vector3 mStartingPosition;
    private Vector3 mActualPosition;
    private float mTimeElapsed = 0;
    private bool mSpawning = true;
    private bool mDying = false;
    #endregion

    private void Start()
    {
        mStartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mSpawning)
            AnimateSpawn();
        if (mDying)
            AnimateDeath();

        transform.position = mSpawning || mDying ? mActualPosition : mStartingPosition;
    }

    private void AnimateSpawn()
    {
        mTimeElapsed += Time.deltaTime;

        if (mTimeElapsed >= mAnimationTime)
        {
            mTimeElapsed = 0;
            mSpawning = false;
        }

        if (mTimeElapsed >= mAnimationTime / 2)
        {
            mPortal.IsEnabled = true;
        }

        float animProgess = Mathf.Lerp(mForwardAmount, 0, Mathf.Min(mTimeElapsed / mAnimationTime, 1));
        mActualPosition = mStartingPosition + (transform.forward * animProgess);
    }

    private void AnimateDeath()
    {
        mPortal.IsEnabled = false;
        mTimeElapsed += Time.deltaTime;

        if (mTimeElapsed >= mAnimationTime)
        {
            mTimeElapsed = 0;
            mDying = false;
            Destroy(gameObject);
        }

        float animProgess = Mathf.Lerp(0 , mForwardAmount, mTimeElapsed / mAnimationTime);
        mActualPosition = mStartingPosition - (transform.forward * -1 * animProgess);
    }


    public void Dead()
    {
        mDying = true;
    }
}
