using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS1_CastFlameWreath : BossState
{
    public BS1_CastFlameWreath(FinalBoss _boss) : base(_boss, "State_CastFlameWreath")
    {}

    public override void OnEnter()
    {
        base.OnEnter();
        mBoss.mFlameWreath.ResetSpell();
        mBoss.mAnimator.SetTrigger("FlameWreathT");
        mBoss.mFireHandLeft.SetActive(true);
        mBoss.mFireHandRight.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mAnimator.ResetTrigger("FlameWreathT");
        mBoss.mFireHandLeft.SetActive(false);
        mBoss.mFireHandRight.SetActive(false);
        mBoss.mFlameWreath.CancelCast();
        mBoss.mTimeSinceLastCastFlameWreath = 0;
    }
}
