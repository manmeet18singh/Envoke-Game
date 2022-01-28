using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttacking : EnemyState
{
    public static readonly string DefaultAnimationName = "Attacking";

    public StateAttacking(Enemy _enemy, string _animationName = null) : base(_enemy, _animationName ?? DefaultAnimationName)
    {}

    public override void Tick()
    {
        Vector3 direction = (GameManager.Instance.mPlayer.transform.position - mEnemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, lookRotation, mEnemy.mEnemySettings.mLookAtSpeed);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        mEnemy.mMeshAgent.enabled = true;
        mEnemy.mIsAttacking = true;
        mEnemy.mMeshAgent.SetDestination(mEnemy.transform.position);
    }

    public override void OnExit()
    {
        base.OnExit();
        mEnemy.mIsAttacking = false;
        mEnemy.mMeshAgent.enabled = false;
    }
}
