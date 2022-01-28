using System;
using UnityEngine;

/// <summary>
/// A basic implementation of an enemy with 4 primary states (not including death).
/// Basic enemies will wander around and periodically stand idle. 
/// 
/// When their target comes into detection range and in LoS (line of sight), they will 
/// begin to chase. They will only stop chasing and return to wandering if the target 
/// escapes vision for a given number of seconds.
/// 
/// Once the target is within combat range, they will switch to attacking. When the 
/// target leaves combat range, they return to chasing. 
/// </summary>
public class EnemyBasic : Enemy
{
    [Header("Basic Enemy Properties")]
    [SerializeField] protected bool mIsMelee = false;
    [SerializeField] protected int mMinWanderTime = 3;
    [SerializeField] protected int mMaxWanderTime = 10;
    [SerializeField] protected int mMinIdleTime = 2;
    [SerializeField] protected int mMaxIdleTime = 5;

    protected StateWander mStateWander;
    protected StateChasing mStateChasing;
    protected StateAttacking mStateAttacking;
    protected StateIdle mStateIdle;

    public bool mSpawnedInBossRoom = false;

    protected override void Awake()
    {
        base.Awake();

        InstantiateBasicStates();

        // State changes
        Func<bool> bossMinion() => () => mSpawnedInBossRoom;
        Func<bool> wanderingTooLong() => () => mStateWander.HasBeenWanderingTooLong;
        Func<bool> idleTooLong() => () => mStateIdle.HasBeenIdleTooLong;
        Func<bool> forgotAboutPlayer() => () => !mSpawnedInBossRoom && !mTargetNearby && mTimeSinceTargetDetected > mEnemySettings.mTimeToForgetPlayer;
        Func<bool> playerNearby() => () => mTargetNearby;
        Func<bool> playerIsAttackable() => () => TargetInAttackRange() && mTargetNearby;
        Func<bool> playerNotAttackable() => () => !mTargetNearby || (mIsMelee && !TargetInAttackRange());
        Func<bool> wasHitByPlayer() => () => {
            if (!mIsAttacking && mHealth.CurrentHealth > 0 && mWasHitByPlayer)
            {
                mWasHitByPlayer = false;
                return true;
            }
            else
                return false;
        };

        mStateMachine.AddTransition(mStateIdle, mStateChasing, bossMinion());
        mStateMachine.AddTransition(mStateIdle, mStateWander, idleTooLong());
        mStateMachine.AddTransition(mStateIdle, mStateChasing, playerNearby());
        mStateMachine.AddTransition(mStateIdle, mStateAttacking, playerIsAttackable());

        mStateMachine.AddTransition(mStateWander, mStateChasing, bossMinion());
        mStateMachine.AddTransition(mStateWander, mStateIdle, wanderingTooLong());
        mStateMachine.AddTransition(mStateWander, mStateChasing, playerNearby());

        mStateMachine.AddTransition(mStateChasing, mStateWander, forgotAboutPlayer());
        mStateMachine.AddTransition(mStateChasing, mStateAttacking, playerIsAttackable());

        mStateMachine.AddTransition(mStateAttacking, mStateChasing, playerNotAttackable());

        mStateMachine.AddTransition(mStateIdle, mStateChasing, wasHitByPlayer());
        mStateMachine.AddTransition(mStateWander, mStateChasing, wasHitByPlayer());

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
        if (mStateAttacking == null)
            mStateAttacking = new StateAttacking(this);
        if (mStateIdle == null)
            mStateIdle = new StateIdle(this, mMinIdleTime, mMaxIdleTime);

        mIdleState = mStateIdle;
    }
}
