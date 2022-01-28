using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_MoveToTarget : IState
{
    private readonly Enemy mEnemy;
    private readonly EnemySettings mEnemySettings;


    public State_MoveToTarget(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;
    }


    public void Tick()
    {

        if (!mEnemy.mMeshAgent.pathPending)
        {
            if (mEnemy.mMeshAgent.remainingDistance <= mEnemy.mMeshAgent.stoppingDistance)
            {
                if (!mEnemy.mMeshAgent.hasPath || mEnemy.mMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    mEnemy.mHasWalkPoint = false;
                    //mEnemy.mHasIdled = false;
                    //Uncomment to see the walkpoints
                    //UnityEngine.GameObject.Destroy(mEnemy.sphere);
                }
            }
        }
    }

    public void OnEnter()
    {
        mEnemy.mMeshAgent.enabled = true;
        //Animator stuff here
        mEnemy.mAnimator.SetBool("Patrolling", true);

    }

    public void OnExit()
    {
        mEnemy.mMeshAgent.enabled = false;
        //Animator stuff here
        mEnemy.mAnimator.SetBool("Patrolling", false);
    }
}
