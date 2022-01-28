using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonSwordsman : EnemyBasic
{
    [SerializeField]
    protected Collider mLeftAxe;
    [SerializeField]
    protected Collider mRightAxe;

    protected override void Awake()
    {
        mStateAttacking = new State_SkeletonWarriorAttack(this, mLeftAxe, mRightAxe);

        base.Awake();
    }
}
