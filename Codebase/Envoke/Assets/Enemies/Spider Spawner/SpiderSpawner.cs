using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpiderSpawner : Enemy
{
    //Modified Version of the EnemyBasic script that will only make the Spider Flee.

    [Header("Basic Enemy Properties")]
    [SerializeField] protected bool mIsMelee = false;
    [SerializeField] protected int mMinWanderTime = 0;
    [SerializeField] protected int mMaxWanderTime = 10;
    [SerializeField] protected int mMinIdleTime = 0;
    [SerializeField] protected int mMaxIdleTime = 5;
    [SerializeField] protected int mMaxWanderRange = 10;


    protected StateWander mStateWander;
    protected StateIdle mStateIdle;
    protected SpiderSpawnerStateFlee mStateFleeing;

    protected override void Awake()
    {
        base.Awake();

        InstantiateBasicStates();

        // State changes
        Func<bool> wanderingTooLong() => () => mStateWander.HasBeenWanderingTooLong;
        Func<bool> idleTooLong() => () => mStateIdle.HasBeenIdleTooLong;
        Func<bool> playerNearby() => () => mTargetNearby;
        Func<bool> forgotAboutPlayer() => () => !mTargetNearby && mTimeSinceTargetDetected > mEnemySettings.mTimeToForgetPlayer;
        Func<bool> wasHitByPlayer() => () =>
        {
            if (mHealth.CurrentHealth > 0 && mWasHitByPlayer)
            {
                mWasHitByPlayer = false;
                return true;
            }
            else
                return false;
        };


        //Transitions
        mStateMachine.AddTransition(mStateWander, mStateIdle, wanderingTooLong());
        mStateMachine.AddTransition(mStateWander, mStateFleeing, playerNearby());

        mStateMachine.AddTransition(mStateIdle, mStateWander, idleTooLong());
        mStateMachine.AddTransition(mStateIdle, mStateFleeing, playerNearby());

        mStateMachine.AddTransition(mStateFleeing, mStateWander, forgotAboutPlayer());

        mStateMachine.AddAnyTransition(mStateFleeing, wasHitByPlayer());

        mStateMachine.SetState(mStateWander);
    }

    private void InstantiateBasicStates()
    {
        if (mStateWander == null)
            mStateWander = new StateWander(this, mMinWanderTime, mMaxWanderTime);
        if (mStateIdle == null)
            mStateIdle = new StateIdle(this, mMinIdleTime, mMaxIdleTime);
        if (mStateFleeing == null)
            mStateFleeing = new SpiderSpawnerStateFlee(this, mMaxWanderRange);
        mIdleState = mStateIdle;
    }
}
