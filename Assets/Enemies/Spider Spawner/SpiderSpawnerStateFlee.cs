using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This is mainly a copy-paste of the GoblinFlee that Matt made and I am unsure if it works right now
public class SpiderSpawnerStateFlee : IState
{
    // References
    protected SpiderSpawner mSpiderSpawner;

    // State variables
    protected Vector3 mStartingPosition;
    protected Vector3 mDestination;
    protected float mWanderRange;
    protected bool mDestinationSet = false;

    public SpiderSpawnerStateFlee(SpiderSpawner _spiderSpawner, float _wanderRange)
    {
        mSpiderSpawner = _spiderSpawner;
        mWanderRange = _wanderRange;
    }

    public void OnEnter()
    {
#if UNITY_EDITOR
        //Debug.Log("Beginning Flee State.");
#endif
        mStartingPosition = mSpiderSpawner.transform.position;
        Vector3 playerPos = GameManager.Instance.transform.position;
        // Set position in front facing player
        Vector3 initialPos = mStartingPosition + (mStartingPosition - playerPos).normalized * 2;
        mSpiderSpawner.mMeshAgent.SetDestination(initialPos);

        //mSpiderSpawner.transform.LookAt(new Vector3(GameManager.Instance.mPlayer.transform.position.x, 
        //    mSpiderSpawner.transform.position.y, 
        //    GameManager.Instance.mPlayer.transform.position.z));

        // Set initial destination directly away from player a few feet
        SearchFleePoint();

        mSpiderSpawner.SetChaseSpeed();
        // TODO - Player surprised animation, then start running animation
        //mSpiderSpawner.mAnimator.SetBool("Fleeing", true);
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        //Debug.Log("Ending Flee State.");
#endif
        mSpiderSpawner.SetWalkSpeed();
        mDestinationSet = false;
        // TODO - exit running animation
        //mSpiderSpawner.mAnimator.SetBool("Fleeing", false);
    }
    public void Tick()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(mSpiderSpawner.transform.position, mSpiderSpawner.transform.forward * 5, Color.red);
        if (Physics.Raycast(mSpiderSpawner.transform.position, mSpiderSpawner.transform.forward, out hitInfo, 5, LayerMask.GetMask("Obstacles")))
        {
            Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
            Vector3 newLoc = Vector3.Reflect((hitInfo.point - playerPosition).normalized, hitInfo.normal) * mWanderRange + mSpiderSpawner.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newLoc, out hit, mWanderRange, 1))
            {
                NavMeshPath path = new NavMeshPath();
                mSpiderSpawner.mMeshAgent.CalculatePath(hit.position, path);
                mSpiderSpawner.mMeshAgent.SetPath(path);
                mDestination = hit.position;
                mDestinationSet = true;
            }
        }

        // Set walk point if has none
        if (!mDestinationSet)
            SearchWalkPoint();
        // Start walking towards valid a point
        if (mDestinationSet)
        {
            // Check if reached destination
            Vector3 distanceToWalkPoint = mSpiderSpawner.transform.position - mDestination;
            distanceToWalkPoint.y = 0;
            mDestinationSet = distanceToWalkPoint.magnitude > 1f;
        }
    }

    private void SearchFleePoint()
    {
        Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
        Vector3 fleeDirection = (mSpiderSpawner.transform.position - playerPosition).normalized;

        Vector3 fleePoint = mSpiderSpawner.transform.position + (fleeDirection * 5);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePoint, out hit, mWanderRange, 1))
        {
            NavMeshPath path = new NavMeshPath();
            mSpiderSpawner.mMeshAgent.CalculatePath(hit.position, path);
            mSpiderSpawner.mMeshAgent.SetPath(path);
            mDestination = hit.position;
            mDestinationSet = true;
        }
    }

    protected virtual void SearchWalkPoint()
    {
        Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
        Vector3 fleeDirection = (mSpiderSpawner.transform.position - playerPosition).normalized;

        Vector3 randomDirection = Random.insideUnitSphere * mWanderRange;
        randomDirection += mStartingPosition;
        randomDirection.x += fleeDirection.x * Random.Range(0, mWanderRange);
        randomDirection.z += fleeDirection.z * Random.Range(0, mWanderRange);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, mWanderRange, 1))
        {
            NavMeshPath path = new NavMeshPath();
            mSpiderSpawner.mMeshAgent.CalculatePath(hit.position, path);
            mSpiderSpawner.mMeshAgent.SetPath(path);
            mDestination = hit.position;
            mDestinationSet = true;
        }
    }
}
