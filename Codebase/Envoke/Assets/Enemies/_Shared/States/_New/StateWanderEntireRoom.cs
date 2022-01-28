using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWanderEntireMap : StateWander
{
    public StateWanderEntireMap(Enemy _enemy, int _minWanderTimeSeconds = 3, int _maxWanderTimeSeconds = 7, string _animationName = null) 
        : base(_enemy, _minWanderTimeSeconds, _maxWanderTimeSeconds, _animationName ?? DefaultAnimationName)
    {}

    protected override void SearchWalkPoint()
    {

        //Calculate random point in range
        float randomX = Random.Range(-mEnemy.mEnemySettings.mWalkPointRange, mEnemy.mEnemySettings.mWalkPointRange);
        float randomZ = Random.Range(-mEnemy.mEnemySettings.mWalkPointRange, mEnemy.mEnemySettings.mWalkPointRange);

        // Set random walk 
        Vector3 walkPoint = new Vector3(mEnemy.transform.position.x + randomX, mEnemy.transform.position.y, mEnemy.transform.position.z + randomZ);

        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(walkPoint, out hit, 2f, mEnemy.mMeshAgent.areaMask) &&
            hit.position.y <= mEnemy.transform.position.y + .5f)
        {

            mEnemy.mHasWalkPoint = true;
            mEnemy.mMeshAgent.SetDestination(walkPoint);
            mDestination = hit.position;
            mDestinationSet = true;

            //UNCOMMENT TO SEE THE POINT
            //mEnemy.sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //mEnemy.sphere.transform.position = mWalkPoint;
        }
    }
}
