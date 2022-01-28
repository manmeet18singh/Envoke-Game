using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_ChargeRushAttack : StateAttacking
{

    private readonly EnemySettings mEnemySettings;
    private float mMaxRushingTime;


    public State_ChargeRushAttack(Enemy enemy, EnemySettings settings, float rushTime) : base(enemy, StateAttacking.DefaultAnimationName)
    {
        mEnemySettings = settings;
        mMaxRushingTime = rushTime;
    }

    public bool IsDoneRushing
    {
        get => (mEnemy.transform.position - mDestination).magnitude < 1.5
|| mTimeSpentRushing > mMaxRushingTime;
    }

    private float mTimeSpentRushing = 0;

    private bool mTargetSet = false;
    private Vector3 mDestination;

    public override void OnEnter()
    {
        base.OnEnter();
        //mEnemy.mMeshAgent.isStopped = false;
        mTimeSpentRushing = 0;
        mTargetSet = false;
        mEnemy.mMeshAgent.acceleration = mEnemySettings.mChargeSpeed;
        mEnemy.mMeshAgent.speed = mEnemySettings.mChargeSpeed;
        SetTargetPoint();
    }

    public override void OnExit()
    {
        base.OnExit();
        //mEnemy.mMeshAgent.isStopped = true;
        mEnemy.mEnemyAttackBehavior.Attack();
        mEnemy.mAnimator.ResetTrigger("Hit");
        mEnemy.mMeshAgent.acceleration = mEnemySettings.mAcceleration;
        mEnemy.mMeshAgent.speed = mEnemySettings.mSpeed;
    }

    private void SetTargetPoint()
    {
        Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
        Vector3 targetLocation = playerPosition - ((playerPosition - mEnemy.transform.position).normalized * mEnemy.mEnemySettings.mStoppingDistance);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetLocation, out hit, 3, 1))
        {
            if (hit.position.y > mEnemy.transform.position.y + 1.5f) return;

            NavMeshPath path = new NavMeshPath();
            mEnemy.mMeshAgent.CalculatePath(hit.position, path);
            mEnemy.mMeshAgent.SetPath(path);
            mDestination = hit.position;
            mTargetSet = true;
        }
    }

    public override void Tick()
    {
        base.Tick();
        mTimeSpentRushing += Time.deltaTime;
        if (!mTargetSet)
            SetTargetPoint();
    }
}

