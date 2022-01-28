using UnityEngine;

public class BS2_FrenzyAoE : BossState
{
    private readonly float mMaxAoETime = 5;

    private float mTimeInAoE;

    public bool MaxAoETimeReached { get => mTimeInAoE >= mMaxAoETime; }

    public BS2_FrenzyAoE(FinalBoss _boss) : base(_boss, "State_FrenzyAoE")
    { }

    public override void OnEnter()
    {
        base.OnEnter();
        mBoss.mFrenzyAoE.ResetSpell();
        mTimeInAoE = 0;
        mBoss.mAnimator.SetTrigger("LaserBeamT");
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mAnimator.ResetTrigger("LaserBeamT");
        mBoss.mFrenzyAoE.CancelCast();
        mBoss.mTimeSinceLastAoEAttack = 0;
    }

    public override void Tick()
    {
        base.Tick();
        mTimeInAoE += Time.deltaTime;
    }
}