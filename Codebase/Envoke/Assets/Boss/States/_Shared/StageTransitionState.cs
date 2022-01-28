using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransitionState : BossState
{
    public StageTransitionState(FinalBoss _boss) : base(_boss, "Stage Transition")
    {}

    public bool mIsDoneTransitioning = false;

    public override void OnEnter()
    {
        base.OnEnter();
        mBoss.mNavMeshAgent.SetDestination(mBoss.transform.position);
        mBoss.mAnimator.SetTrigger("StageChangeT");
        mBoss.mAnimator.SetBool("InStage1", false);
        mBoss.mAnimator.SetBool("InStage2", true);
    }


    public void SummonSword()
    {
        mBoss.mSword.SetActive(true);
    }
}
