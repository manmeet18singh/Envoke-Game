using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack : IState
{
    protected readonly Enemy mEnemy;
    protected readonly EnemySettings mEnemySettings;

    public State_Attack(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;

    }

    virtual public void Tick()
    {
        /**
         * While this is nice, if the player runs circles around the target quick enough, they lose target, but
         * continue to attack. In practice, this should just be focusing player unless we plan to have ally
         * NPCs.
         */
        //if (mEnemy.mCurrentTarget != null)
        //{
        //    Vector3 direction = (mEnemy.mCurrentTarget.transform.position - mEnemy.transform.position).normalized;
        //    Quaternion lookRotation = Quaternion.LookRotation(direction);

        //    mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, lookRotation, mEnemySettings.mLookAtSpeed);

        //    //mEnemy.mEnemyAttackBehavior.Attack();
        //    mEnemy.mHasAttacked = true;
        //}

        Vector3 direction = (GameManager.Instance.mPlayer.transform.position - mEnemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, lookRotation, mEnemySettings.mLookAtSpeed);

        //mEnemy.mEnemyAttackBehavior.Attack();
        
    }
    virtual public void OnEnter()
    {
#if UNITY_EDITOR
        //Debug.Log("Beginning Attack State.");
#endif
        mEnemy.mMeshAgent.enabled = true;
        mEnemy.mIsAttacking = true;
        //Stop in my track and fire
        mEnemy.mMeshAgent.SetDestination(mEnemy.transform.position);
        // Animator Stuff
        mEnemy.mAnimator.SetBool("Attacking", true);
    }

    virtual public void OnExit()
    {
#if UNITY_EDITOR
        //Debug.Log("Ending Attack State.");
#endif
        mEnemy.mIsAttacking = false;
        mEnemy.mMeshAgent.enabled = false;
        mEnemy.mAnimator.SetBool("Attacking", false);
    }

}
