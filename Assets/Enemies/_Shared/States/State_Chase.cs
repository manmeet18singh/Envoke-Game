using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : IState
{
    private readonly Enemy mEnemy;
    private readonly EnemySettings mEnemySettings;


    public State_Chase(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;
    }

    public void Tick()
    {
        // Chase for some time maybe?
        Collider[] targetsInDetectionRadius = Physics.OverlapSphere(mEnemy.transform.position, mEnemySettings.mDetectionRadius, mEnemySettings.mPlayerLayerMask);

        for (int i = 0; i < targetsInDetectionRadius.Length; i++)
        {
            PlayerHealth target = targetsInDetectionRadius[i].transform.GetComponent<PlayerHealth>();

            if (target != null)
            {
                mEnemy.mMeshAgent.SetDestination(target.transform.position);
            }
        }
    }

    public void OnEnter()
    {
        //mEnemy.mHasAttacked = false;
        mEnemy.mMeshAgent.enabled = true;
        mEnemy.mMeshAgent.SetDestination(mEnemy.mTarget.transform.position);
        //Animator Stuff
        mEnemy.mAnimator.SetBool("Patrolling", true);
    }

    public void OnExit()
    {
        mEnemy.mMeshAgent.enabled = false;
        //Animator stuff
        mEnemy.mAnimator.SetBool("Patrolling", false);
    }
}
