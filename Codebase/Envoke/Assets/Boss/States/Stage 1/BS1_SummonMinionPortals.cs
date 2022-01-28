using UnityEngine;

public class BS1_SummonMinionPortals : BossState
{
    public bool mIsDoneCasting;
    public BS1_SummonMinionPortals(FinalBoss _boss) : base(_boss, "State_SummonPortals")
    {}

    public override void OnEnter()
    {
        base.OnEnter();
        mIsDoneCasting = false;
        mBoss.mSummonPortals.ResetSpell();
        mBoss.mSummonHandLeft.SetActive(true);
        mBoss.mSummonHandRight.SetActive(true);

        if (mBoss.mSummonPortals.TeleportBeforeCasting)
            mBoss.mAnimator.SetTrigger("TeleportSummonPortalsT");
        else
            mBoss.mAnimator.SetTrigger("SummonPortalsT");
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mSummonHandLeft.SetActive(false);
        mBoss.mSummonHandRight.SetActive(false);

        if (mBoss.mSummonPortals.TeleportBeforeCasting)
            mBoss.mAnimator.ResetTrigger("TeleportSummonPortalsT");
        mBoss.mAnimator.ResetTrigger("SummonPortalsT");
        mBoss.mTimeSinceLastSummonedPortals = 0;
        //mBoss.mSummonPortals.CancelCast();
    }
}
