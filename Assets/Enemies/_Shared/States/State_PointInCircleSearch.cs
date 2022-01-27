using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PointInCircleSearch : IState
{
    private readonly Enemy mEnemy;
    private readonly EnemySettings mEnemySettings;

    private Vector3 mWalkPoint;

    public State_PointInCircleSearch(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;
    }

    public void Tick()
    {
        if (!mEnemy.mHasWalkPoint)
        {
            SearchWalkPoint();
        }
        if (mEnemy.mHasWalkPoint)
        {
            mEnemy.mWalkPoint = mWalkPoint;
        }
    }

    private void SearchWalkPoint()
    {
        Vector2 point = new Vector2(mEnemy.transform.position.x, mEnemy.transform.position.z) + Random.insideUnitCircle.normalized * mEnemySettings.mDetectionRadius;

        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(new Vector3(point.x, mEnemy.transform.position.y, point.y), out hit, .5f, mEnemy.mMeshAgent.areaMask))
        {
            mEnemy.mHasWalkPoint = true;
            mWalkPoint = hit.position;
            mWalkPoint = new Vector3(point.x, mEnemy.transform.position.y, point.y);
            mEnemy.mMeshAgent.SetDestination(mWalkPoint);

            //UNCOMMENT TO SEE THE WALKPOINT
            //mEnemy.sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //mEnemy.sphere.transform.position = mWalkPoint;
        }
    }

    public void OnEnter() 
    {
        mEnemy.mMeshAgent.enabled = true;
        //Animator stuff here
    }

    public void OnExit() 
    {
        mEnemy.mMeshAgent.enabled = true;
    }
}
