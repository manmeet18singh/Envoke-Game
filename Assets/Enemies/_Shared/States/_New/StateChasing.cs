using UnityEngine;
using UnityEngine.AI;

public class StateChasing : EnemyState
{
    public static readonly string DefaultAnimationName = "Patrolling";

    #region Properties
    // Determines how far away the target can get before resampling
    public float mFollowAccuracy;
    // Determines how precision the position sampling is when targeting
    public float mFollowPrecision;
    #endregion

    public StateChasing(Enemy _enemy, string _animationName = null, float _followAccuracy = 1, float _followPrecision = 5) : base(_enemy, _animationName ?? DefaultAnimationName) 
    {
        mFollowAccuracy = _followAccuracy;
        mFollowPrecision = _followPrecision;
    }

    override public void OnEnter()
    {
#if UNITY_EDITOR
        //Debug.Log("Beginning Chasing State.");
#endif
        base.OnEnter();
        mEnemy.SetChaseSpeed();
        mEnemy.mMeshAgent.enabled = true;
        mEnemy.mMeshAgent.SetDestination(mEnemy.mTarget.transform.position);
    }

    override public void OnExit()
    {
#if UNITY_EDITOR
        //Debug.Log("Ending Chasing State.");
#endif
        base.OnExit();
        mEnemy.mMeshAgent.enabled = false;
    }

    override public void Tick()
    {
        // Update position if the player has moved far enough away from original target
        if ((mEnemy.mTarget.transform.position - mEnemy.transform.position).magnitude > mFollowAccuracy)
        {
            if (NavMesh.SamplePosition(mEnemy.mTarget.transform.position, out NavMeshHit hit, mFollowPrecision, 1))
            {
                NavMeshPath path = new NavMeshPath();
                mEnemy.mMeshAgent.CalculatePath(hit.position, path);
                mEnemy.mMeshAgent.SetPath(path);
            }
        }
    }
}
