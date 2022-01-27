using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BS1_MoveAround : BossState
{
    public bool mDestinationSet;
    public Vector3 mDestination;

    private float mTimeToMoveAround;

    public bool mHasMoved { get => mBoss.mTimeMovingAround > mTimeToMoveAround; }

    public BS1_MoveAround(FinalBoss _boss) : base(_boss, "State_MoveAround")
    {}

    public override void OnEnter()
    {
        base.OnEnter();
        mBoss.mAnimator.SetTrigger("WalkingT");
        mDestinationSet = false;
        mBoss.mTimeMovingAround = 0;
        mBoss.mNavMeshAgent.isStopped = false;
        mTimeToMoveAround = Random.Range(1, 2);
        // TODO - Set new walk point
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mAnimator.SetTrigger("WalkingT");
        mBoss.mNavMeshAgent.isStopped = true;
    }

    public override void Tick()
    {
        base.Tick();
        mBoss.mTimeMovingAround += Time.deltaTime;

        if (!mDestinationSet || (mDestination - mBoss.transform.position).magnitude < 1)
            SearchWalkPoint();
    }

    protected virtual void SearchWalkPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * mBoss.mWalkPointRange;
        randomDirection += mBoss.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, mBoss.mWalkPointRange, 1))
        {
            if (hit.position.y > mBoss.transform.position.y + 1.5f) return;

            NavMeshPath path = new NavMeshPath();
            mBoss.mNavMeshAgent.CalculatePath(hit.position, path);
            mBoss.mNavMeshAgent.SetPath(path);
            mDestination = hit.position;

            mDestinationSet = true;
        }
    }
}
