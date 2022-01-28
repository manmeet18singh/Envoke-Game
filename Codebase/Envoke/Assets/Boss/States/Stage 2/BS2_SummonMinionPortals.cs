using UnityEngine;
public class BS2_SummonMinionPortals : BossState
{
    public bool mIsDoneCasting;

    public BS2_SummonMinionPortals(FinalBoss _boss) : base(_boss, "State_FrenzySummonPortals")
    { }

    public override void OnEnter()
    {
        base.OnEnter();
        mIsDoneCasting = false;
        mBoss.mFrenzySummonPortals.ResetSpell();
        mBoss.mSummonHandLeft.SetActive(true);

        if (mBoss.mFrenzySummonPortals.TeleportBeforeCasting)
            mBoss.mAnimator.SetTrigger("TeleportSummonPortalsT");
        mBoss.mAnimator.SetTrigger("FrenzySummonPortalT");
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mSummonHandLeft.SetActive(false);

        if (mBoss.mFrenzySummonPortals.TeleportBeforeCasting)
            mBoss.mAnimator.ResetTrigger("TeleportSummonPortalsT");
        mBoss.mAnimator.ResetTrigger("FrenzySummonPortalT");
        mBoss.mTimeSinceLastFrenzySummoned = 0;
        //mBoss.mFrenzySummonPortals.CancelCast();
    }
}