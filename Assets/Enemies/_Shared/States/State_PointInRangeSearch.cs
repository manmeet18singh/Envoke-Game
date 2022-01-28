using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PointInRangeSearch : IState
{
    private readonly Enemy mEnemy;
    private readonly EnemySettings mEnemySettings;


    public State_PointInRangeSearch(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;
    }

    private Vector3 mWalkPoint;
    
    public void Tick()
    {
        if (!mEnemy.mHasWalkPoint)
        {
            SearchWalkPoint(mEnemySettings.mWalkPointRange);
        }
        if (mEnemy.mHasWalkPoint)
        {
            mEnemy.mWalkPoint = mWalkPoint;
        }
    }

    private void SearchWalkPoint(float walkPointRange)
    {
        //Calculate random point in range
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        // Set random walk 
        mWalkPoint = new Vector3(mEnemy.transform.position.x + randomX, mEnemy.transform.position.y, mEnemy.transform.position.z + randomZ);

        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(mWalkPoint, out hit, 2f, mEnemy.mMeshAgent.areaMask))
        {
            mEnemy.mHasWalkPoint = true;
            mWalkPoint = hit.position;
            mEnemy.mMeshAgent.SetDestination(mWalkPoint);

            //UNCOMMENT TO SEE THE POINT
            //mEnemy.sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //mEnemy.sphere.transform.position = mWalkPoint;
        }
    }

    public void OnEnter()
    {
        mEnemy.mMeshAgent.enabled = true;
    }

    public void OnExit()
    {
        mEnemy.mMeshAgent.enabled = true;
    }
}
