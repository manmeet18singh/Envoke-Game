using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS2_CastLaser : BossState
{
    LaserController mLaser = null;

    public BS2_CastLaser(FinalBoss _boss, LaserController _laser) : base(_boss, "State_BossLaser")
    {
        mLaser = _laser;
    }

    public override void OnEnter()
    {
        mLaser.mStartPos = mBoss.transform.position;
        mLaser.mEndPos = mBoss.transform.position;
        mLaser.gameObject.SetActive(true);
        mBoss.mNavMeshAgent.enabled = false;
    }

    public override void Tick()
    {
        mBoss.transform.LookAt(new Vector3(GameManager.Instance.transform.position.x, mBoss.transform.position.y, GameManager.Instance.mPlayer.transform.position.z));

        if(Physics.Raycast(mBoss.transform.position, mBoss.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            mLaser.mEndPos = hit.point;
        }

        mLaser.mStartPos = mBoss.transform.position;
    }

    public override void OnExit()
    {
        mLaser.gameObject.SetActive(false);
        mBoss.mNavMeshAgent.enabled = true;
        //mLaser.IsDoneCasting = false;
    }

}
