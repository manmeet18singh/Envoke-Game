using UnityEngine;
using System;

public class ElementalMage : EnemyBasic
{
    #region State variables
    [HideInInspector] private StateCastBlink mStateCastingBlink;
    private ResitantHealth mResitantHealth;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        mResitantHealth = (ResitantHealth)mHealth;

        mStateCastingBlink = new StateCastBlink(this, null);

        Func<bool> doneCastingBlink() => () => mStateCastingBlink.BlinkOnCooldown;
        Func<bool> shouldCastBlink() => () => !mStateCastingBlink.BlinkOnCooldown &&
            (mResitantHealth.CurrentHealth > 0 && mResitantHealth.CurrentHealthPercent < 66) &&
            Time.time - TimeLastHitByPlayer < 1;

        mStateMachine.AddTransition(mStateCastingBlink, mStateChasing, doneCastingBlink());
        mStateMachine.AddTransition(mStateIdle, mStateCastingBlink, shouldCastBlink());
        mStateMachine.AddTransition(mStateAttacking, mStateCastingBlink, shouldCastBlink());
        mStateMachine.AddTransition(mStateWander, mStateCastingBlink, shouldCastBlink());
    }
}
