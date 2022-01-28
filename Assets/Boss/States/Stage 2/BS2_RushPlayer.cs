using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BS2_RushPlayer : BossState
{
    public bool IsDoneRushing { get =>
            (mBoss.transform.position - mDestination).magnitude < 1.5
            || mTimeSpentRushing > mMaxRushingTime; }

    private float mTimeSpentRushing = 0;
    private float mMaxRushingTime = 3;

    private bool mTargetSet = false;
    private Vector3 mDestination;

    public BS2_RushPlayer(FinalBoss _boss) : base (_boss, "State_RushPlayer")
    { }

    public override void OnEnter()
    {
        base.OnEnter();
        mBoss.mAnimator.SetTrigger("DashingT");
        mBoss.mNavMeshAgent.isStopped = false;
        mTimeSpentRushing = 0;
        mTargetSet = false;
        mBoss.SetRushSpeed();
        SetTargetPoint();
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mAnimator.ResetTrigger("DashingT");
        mBoss.mNavMeshAgent.isStopped = true;
        mBoss.SetRushSpeed();
        mBoss.mTimeSinceLastDash = 0;
    }

    private void SetTargetPoint()
    {
        Vector3 playerPosition = GameManager.Instance.mPlayer.transform.position;
        Vector3 targetLocation = playerPosition - ((playerPosition - mBoss.transform.position).normalized * mBoss.mDashStoppingDistance);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetLocation, out hit, 3, 1))
        {
            if (hit.position.y > mBoss.transform.position.y + 1.5f) return;
               

            //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = hit.position;


            NavMeshPath path = new NavMeshPath();
            mBoss.mNavMeshAgent.CalculatePath(hit.position, path);
            mBoss.mNavMeshAgent.SetPath(path);
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