using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SkeletonWarriorAttack : StateAttacking
{
    // References
    protected Collider mLeftAxe;
    protected Collider mRightAxe;

    Coroutine mAxeEnabler;

    public State_SkeletonWarriorAttack(Enemy _enemy, Collider _leftAxe, Collider _rightAxe) : base(_enemy, StateAttacking.DefaultAnimationName)
    {
        mLeftAxe = _leftAxe;
        mRightAxe = _rightAxe;
    }

    override public void OnEnter()
    {
        base.OnEnter();
        if (mAxeEnabler != null)
            mEnemy.StopCoroutine(mAxeEnabler);
        mAxeEnabler = mEnemy.StartCoroutine(EnableWeapons());
    }

    public override void OnExit()
    {
        base.OnExit();
        if (mAxeEnabler != null)
        {
            mEnemy.StopCoroutine(mAxeEnabler);
            mAxeEnabler = null;
        }
        DisableWeapons();
    }

    private void DisableWeapons()
    {
        mLeftAxe.enabled = false;
        mRightAxe.enabled = false;
    }

    private IEnumerator EnableWeapons()
    {
        yield return new WaitForSeconds(mEnemy.mEnemySettings.mAttackDelay);
        mRightAxe.enabled = true;
        mLeftAxe.enabled = true;
    }
}
