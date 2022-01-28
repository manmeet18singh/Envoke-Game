using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinStateFlee : IState
{
    // References
    protected BomberGoblin mGoblin;

    // State variables
    protected Vector3 mStartingPosition;
    protected Vector3 mDestination;
    protected float mWanderRange;
    protected bool mDestinationSet = false;

    // Bomb Dropping Variables
    private Vector2 mBombDropInterval;
    private float mBombDropRange;

    // State Variables
    private float mTimeSinceLastBombDrop = 0;
    private float mTimeToDropBomb = 0;

    public GoblinStateFlee(BomberGoblin _goblin, float _fleeRange, Vector2 _bombDropInterval, float _bombDropRange)
    {
        mGoblin = _goblin;
        mWanderRange = _fleeRange;
        mBombDropInterval = _bombDropInterval;
        mBombDropRange = _bombDropRange;
        mTimeToDropBomb = Random.Range(mBombDropInterval.x, mBombDropInterval.y);
    }

    public void OnEnter()
    {
#if UNITY_EDITOR
        //Debug.Log("Beginning Flee State.");
#endif
        mGoblin.mMeshAgent.enabled = true;
        mGoblin.mMeshAgent.speed = mGoblin.mFleeSpeed;
        mStartingPosition = mGoblin.transform.position;
        Vector3 playerPos = GameManager.Instance.transform.position;
        // Set position in front facing player
        Vector3 initialPos = mStartingPosition + (mStartingPosition - playerPos).normalized * 2;
        mGoblin.mMeshAgent.SetDestination(initialPos);

        //mGoblin.transform.LookAt(new Vector3(GameManager.Instance.mPlayer.transform.position.x, 
        //    mGoblin.transform.position.y, 
        //    GameManager.Instance.mPlayer.transform.position.z));

        // Set initial destination directly away from player a few feet
        SearchFleePoint();

        mGoblin.SetChaseSpeed();
        // TODO - Player surprised animation, then start running animation
        mGoblin.mAnimator.SetBool("Fleeing", true);
        mGoblin.mAnimator.SetTrigger("Flee");
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        //Debug.Log("Ending Flee State.");
#endif
        mGoblin.SetWalkSpeed();
        mDestinationSet = false;
        // TODO - exit running animation
        mGoblin.mAnimator.SetBool("Fleeing", false);
        mGoblin.mAnimator.ResetTrigger("Flee");
    }
    public void Tick()
    {
        mGoblin.mMeshAgent.speed = mGoblin.mFleeSpeed;
        RaycastHit hitInfo;
        Debug.DrawRay(mGoblin.transform.position, mGoblin.transform.forward * 5, Color.red);
        if (Physics.Raycast(mGoblin.transform.position, mGoblin.transform.forward, out hitInfo, 5, LayerMask.GetMask("Obstacles")))
        {
            Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
            Vector3 newLoc = Vector3.Reflect((hitInfo.point - playerPosition).normalized, hitInfo.normal) * mWanderRange + mGoblin.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newLoc, out hit, mWanderRange, 1))
            {
                NavMeshPath path = new NavMeshPath();
                mGoblin.mMeshAgent.CalculatePath(hit.position, path);
                mGoblin.mMeshAgent.SetPath(path);
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
            Vector3 distanceToWalkPoint = mGoblin.transform.position - mDestination;
            distanceToWalkPoint.y = 0;
            mDestinationSet = distanceToWalkPoint.magnitude > 1f;
        }

        // Bomb Dropping
        mTimeSinceLastBombDrop += Time.deltaTime;

        if (mTimeSinceLastBombDrop > mTimeToDropBomb)
        {
            mTimeSinceLastBombDrop = 0;

            Vector3 bombLocation = mGoblin.transform.position + (Random.insideUnitSphere * mBombDropRange);
            bombLocation.y += 0.5f;
            Vector3 initialPosition = new Vector3(mGoblin.transform.position.x, mGoblin.transform.position.y + 0.5f, mGoblin.transform.position.z);
            GameObject bomb = GameObject.Instantiate(mGoblin.mBomb, initialPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
            //bomb.GetComponentInChildren<BombExploder>().Goblin = mGoblin;
            GameManager.Instance.LerpObjectToPosition(bomb, bomb.transform.position, bombLocation, 1f);

            mTimeToDropBomb = Random.Range(mBombDropInterval.x, mBombDropInterval.y);
        }
    }

    private void SearchFleePoint()
    {
        Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
        Vector3 fleeDirection = (mGoblin.transform.position - playerPosition).normalized;

        Vector3 fleePoint = mGoblin.transform.position + (fleeDirection * 5);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePoint, out hit, mWanderRange, 1))
        {
            NavMeshPath path = new NavMeshPath();
            mGoblin.mMeshAgent.CalculatePath(hit.position, path);
            mGoblin.mMeshAgent.SetPath(path);
            mDestination = hit.position;
            mDestinationSet = true;
        }
    }

    protected virtual void SearchWalkPoint()
    {
        Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
        Vector3 fleeDirection = (mGoblin.transform.position - playerPosition).normalized;

        Vector3 randomDirection = Random.insideUnitSphere * mWanderRange;
        randomDirection += mStartingPosition;
        randomDirection.x += fleeDirection.x * Random.Range(0, mWanderRange);
        randomDirection.z += fleeDirection.z * Random.Range(0, mWanderRange);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, mWanderRange, 1)) 
        {
            NavMeshPath path = new NavMeshPath();
            mGoblin.mMeshAgent.CalculatePath(hit.position, path);
            mGoblin.mMeshAgent.SetPath(path);
            mDestination = hit.position;
            mDestinationSet = true;
        }
    }
}
