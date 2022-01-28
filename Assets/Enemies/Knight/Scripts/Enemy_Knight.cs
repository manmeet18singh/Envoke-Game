using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Knight : Enemy
{
    [Header("Basic Enemy Properties")]
    [SerializeField] protected bool mIsMelee = false;
    [SerializeField] protected int mMinWanderTime = 3;
    [SerializeField] protected int mMaxWanderTime = 10;
    [SerializeField] protected int mMinIdleTime = 2;
    [SerializeField] protected int mMaxIdleTime = 5;

    protected StateWander mStateWander;
    protected StateChasing mStateChasing;
    protected StateIdle mStateIdle;
    protected State_ChargeRushAttack mStateCharge;
    protected State_Cooldown mStateCooldown;
    
    protected override void Awake()
    {
        base.Awake();

        InstantiateBasicStates();

        // State changes
        Func<bool> wanderingTooLong() => () => mStateWander.HasBeenWanderingTooLong;
        Func<bool> idleTooLong() => () => mStateIdle.HasBeenIdleTooLong;
        Func<bool> forgotAboutPlayer() => () => !mTargetNearby && mTimeSinceTargetDetected > mEnemySettings.mTimeToForgetPlayer;
        Func<bool> playerNearby() => () => mTargetNearby;
        Func<bool> playerIsAttackable() => () => TargetInAttackRange() && mTargetNearby;
        Func<bool> wasHitByPlayer() => () => {
            if (!mIsAttacking && mHealth.CurrentHealth > 0 && mWasHitByPlayer)
            {
                mWasHitByPlayer = false;
                return true;
            }
            else
                return false;
        };
        Func<bool> isDoneRushing() => () => mStateCharge.IsDoneRushing;
        Func<bool> CooldownTooLong() => () => mStateCooldown.HasBeenIdleTooLong;

        mStateMachine.AddTransition(mStateIdle, mStateWander, idleTooLong());
        mStateMachine.AddTransition(mStateIdle, mStateChasing, playerNearby());
        mStateMachine.AddTransition(mStateIdle, mStateCharge, playerIsAttackable());

        mStateMachine.AddTransition(mStateWander, mStateIdle, wanderingTooLong());
        mStateMachine.AddTransition(mStateWander, mStateChasing, playerNearby());

        mStateMachine.AddTransition(mStateChasing, mStateWander, forgotAboutPlayer());
        mStateMachine.AddTransition(mStateChasing, mStateCharge, playerIsAttackable());

        mStateMachine.AddAnyTransition(mStateChasing, wasHitByPlayer());
        
        mStateMachine.AddTransition(mStateCharge, mStateCooldown, isDoneRushing());
        mStateMachine.AddTransition(mStateCooldown, mStateWander, CooldownTooLong());

        if (UnityEngine.Random.Range(0, 101) < 50)
            mStateMachine.SetState(mStateWander);
        else
            mStateMachine.SetState(mIdleState);
    }

    /// <summary>
    /// Simply sets up  any states that haven't been explicity set up by child classes. 
    /// By overriding Awake() and setting up state classes before calling base.Awake(),
    /// you can customize a typical "Wander->Chase->Attack" enemy while still utilizing
    /// this base class.
    /// </summary>
    private void InstantiateBasicStates()
    {
        if (mStateWander == null)
            mStateWander = new StateWander(this, mMinWanderTime, mMaxWanderTime);
        if (mStateChasing == null)
            mStateChasing = new StateChasing(this);
        if (mStateIdle == null)
            mStateIdle = new StateIdle(this, mMinIdleTime, mMaxIdleTime);
        if (mStateCharge == null)
            mStateCharge = new State_ChargeRushAttack(this, this.mEnemySettings, 3f);
        if (mStateCooldown == null)
            mStateCooldown = new State_Cooldown(this, 5, 5, "Cooldown");

        mIdleState = mStateIdle;
    }
}
