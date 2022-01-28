using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CastBossLaserSpell : BossSpell
{
    [Header("References")]
    [SerializeField] GameObject mLaserObject = null;

    [Header("Variations")]
    [SerializeField] private LaserSpellSO mSpellData;
    [SerializeField] private List<LaserSpellSO> mVariations = null;
    public int NumberOfVariations { get => mVariations.Count; }
    [SerializeField] protected string[] mSfx = null;

    #region State Variables
    private List<LaserController> mLasers = new List<LaserController>();
    private bool mIsCastingSpell = false;
    private bool mIsDespawning = false;

    private delegate void CastLaserBeam();
    private CastLaserBeam mLaserBeamFunction;

    private GameObject mRotator;
    private float mCurrentAngle = 0f;
    #endregion

    private void Awake()
    {
        //Debug.Log("STARTING");
        mLasers = new List<LaserController>();
    }

    public override void CancelCast()
    {
        DespawnLasers();
    }

    private void DespawnLasers()
    {
        mIsDespawning = true;
        bool doneDespawning = true;
        foreach (LaserController laser in mLasers)
        {
            if (laser != null)
            {
                laser.DestroyLaser();
                doneDespawning = false;
            }
        }

        if (doneDespawning)
        {
            mLasers.Clear();
            mIsCastingSpell = false;
            mIsDoneCasting = true;
            mIsDespawning = false;
        }
    }

    public override void CastSpell()
    {
        mIsCastingSpell = true;
        mIsDoneCasting = false;
        mIsDespawning = false;
        switch (mSpellData.LaserPathType)
        {
            case LaserSpellSO.LaserType.Circular:
                AudioManager.PlayRandomSFX(mSfx);
                mLaserBeamFunction = CastLaserInCircle;
                mCurrentAngle = 0;
                break;
            case LaserSpellSO.LaserType.FollowPlayer:
                //mLaserBeamFunction = CastLaserFollowingPlayer;
                break;
            case LaserSpellSO.LaserType.RandomRapidFire:
                //mLaserBeamFunction = CastLaserRapidFire;
                break;
        }
        InstantiateBeams();
    }

    private void Update()
    {
        if (mIsDespawning)
        {
            DespawnLasers();
            return;
        }

        if (!mIsCastingSpell)
            return;

        mLaserBeamFunction();
    }

    private void InstantiateBeams()
    {
        //if (mRotator != null)
        //GameObject.Destroy(mRotator);

        // mRotator = Instantiate(mLaserRotator, mBoss.transform.position, Quaternion.identity);

        foreach (LaserSpellSO.StartStop startStopPos in mSpellData.LazerPositions)
        {
            Vector3 pos = mBoss.transform.position + startStopPos.StartPositionOffset;
            mLasers.Add(Instantiate(mLaserObject, pos, Quaternion.identity).GetComponent<LaserController>());
            //mLasers[mLasers.Count - 1].transform.parent = mRotator.transform;
            mLasers[mLasers.Count - 1].mDamage = mSpellData.Damage;
            mLasers[mLasers.Count - 1].mDestroyReductionInterval = mSpellData.BeamDecaythRate;
            mLasers[mLasers.Count - 1].mStartPos = pos;
            mLasers[mLasers.Count - 1].mEndPos = pos;
        }

        //Debug.Log("Instantiated");
    }

    private bool GrowBeams()
    {
        // Animate beams shooting out
        bool beamsStillGrowing = false;
        for (int i = 0; i < mLasers.Count; ++i)
        {
            LaserController controller = mLasers[i];
            if (controller.mBeamDoneGrowing)
                continue;

            LaserSpellSO.StartStop positionData = mSpellData.LazerPositions[i];
            controller.mStartPos = mBoss.transform.position + positionData.StartPositionOffset;
            Vector3 endPos = controller.mStartPos + positionData.EndPositionOffset;
            //Debug.Log("endPos " + endPos);
            controller.mMaxEndPos = endPos;

            if ((controller.mEndPos - endPos).magnitude > 1)
            {
                Vector3 growthDirection = (endPos - controller.mStartPos).normalized;
                //Debug.Log("Growing by " + (growthDirection * mSpellData.BeamGrowthRate * Time.deltaTime));
                controller.Grow(growthDirection * mSpellData.BeamGrowthRate * Time.deltaTime);
                beamsStillGrowing = true;
            }
            else
                controller.mBeamDoneGrowing = true;
        }

        return beamsStillGrowing;
    }

    private void CastLaserInCircle()
    {
        //Debug.Log("Casting in a circle " + mLasers.Count);
        // If set, wait for full beam growth before 
        if (GrowBeams() && mSpellData.GrowBeamsBeforeMoving)
            return;
        //Debug.Log("Done growing!");
        // DEBUG
        if (true)
        {
            DespawnLasers();
            return;
        }

        //if (mCurrentAngle >= 360)
        //{
        //    Debug.Log("Done casting spell!");
        //    mIsCastingSpell = false;
        //    mIsDoneCasting = true;
        //    return;
        //}

        //mRotator.transform.Rotate(Vector3.up, mSpellData.AngleInterval * Time.deltaTime);

        //Vector2 beamMovement = new Vector2(Mathf.Cos(mCurrentAngle), Mathf.Sin(mCurrentAngle));
        //Debug.Log("beamMovement: " + beamMovement + " ( " + mCurrentAngle + ")");
        //for (int i = 0; i < mLasers.Count; ++i)
        //{
        //    LaserController controller = mLasers[i];
        //    LaserSpellSO.StartStop positionData = mSpellData.LazerPositions[i];
        //    controller.mStartPos = mBoss.transform.position + positionData.StartPositionOffset;

        //    Vector3 beamDirection = (controller.mEndPos - controller.mStartPos);
        //    beamDirection.x += beamMovement.x;
        //    beamDirection.y += beamMovement.y;
        //    float beamMagnitude = beamDirection.magnitude;
        //    beamDirection = beamDirection.normalized;

        //    controller.mEndPos.x = controller.mStartPos.x + (beamDirection.x * beamMagnitude);
        //    controller.mEndPos.z = controller.mStartPos.z + (beamDirection.z * beamMagnitude);
        //}

        //mCurrentAngle += mSpellData.AngleInterval * Time.deltaTime;
    }

    public override void SetSpellVariation(int _index)
    {
        if (_index >= mVariations.Count)
            return;

        mSpellData = mVariations[_index];
    }
}
