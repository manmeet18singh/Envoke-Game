using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BomberGoblin : EnemyBasic
{
    [Header("Bomber Goblin Properties")]
    [SerializeField] public float mFleeSpeed = 8;
    [Range(0,100)][SerializeField] private int mFleeHealthThreshold = 50;
    [SerializeField] private float mFleeingRange = 20;
    [SerializeField] private Vector2 mFleeingBombDropInterval = new Vector2(0.1f, 1);
    [SerializeField] private float mFleeingBombDropRange = 15;

    [Header("Bomber Goblin References")]
    [SerializeField] public GameObject mBomb;

    GoblinStateFlee mStateFleeing;

    protected override void Awake()
    {
        base.Awake();
        mStateFleeing = new GoblinStateFlee(this, mFleeingRange, mFleeingBombDropInterval, mFleeingBombDropRange);

        Func<bool> wantsToFlee() => () => mHealth.CurrentHealthPercent <= mFleeHealthThreshold;

        mStateMachine.AddTransition(mStateWander, mStateFleeing, wantsToFlee());
        mStateMachine.AddTransition(mStateIdle, mStateFleeing, wantsToFlee());
        mStateMachine.AddTransition(mStateChasing, mStateFleeing, wantsToFlee());
        mStateMachine.AddTransition(mStateAttacking, mStateFleeing, wantsToFlee());
    }
}
