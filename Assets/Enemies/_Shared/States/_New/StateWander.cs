using UnityEngine;
using UnityEngine.AI;

public class StateWander : EnemyState
{
    public static readonly string DefaultAnimationName = "Patrolling";

    #region Properties
    protected int mMinWanderTimeSeconds;
    protected int mMaxWanderTimeSeconds;
    #endregion

    #region Dynamic State Variables
    protected Vector3 mStartingPosition;
    protected Vector3 mDestination;

    // A random time between mMinWanderTimeSeconds and mMaxWanderTimeSeconds
    private float mTimeToWander;

    protected bool mDestinationSet;

    private float mTimeSpentWandering;

    // Public predicate used for state transitions
    public bool HasBeenWanderingTooLong { get => mTimeSpentWandering > mTimeToWander; }
    #endregion

    public StateWander(Enemy _enemy, int _minWanderTimeSeconds = 3, int _maxWanderTimeSeconds = 7, string _animationName = null) : base(_enemy, _animationName ?? DefaultAnimationName)
    {
        mMinWanderTimeSeconds = _minWanderTimeSeconds;
        mMaxWanderTimeSeconds = _maxWanderTimeSeconds;
    }

    override public void OnEnter()
    {
#if UNITY_EDITOR
        //Debug.Log("Beginning Wander State.");
#endif
        base.OnEnter();
        mEnemy.SetWalkSpeed();
        mEnemy.mMeshAgent.enabled = true;

        mStartingPosition = mEnemy.transform.position;
        mTimeToWander = ((float) Random.Range(mMinWanderTimeSeconds*1000, mMaxWanderTimeSeconds*1000)) / 1000;
        mTimeSpentWandering = 0;
        mDestinationSet = false;
    }

    override public void OnExit()
    {
#if UNITY_EDITOR
        //Debug.Log("Ending Wander State.");
#endif
        base.OnExit();
    }

    override public void Tick()
    {
        mTimeSpentWandering += Time.deltaTime;
        // Set walk point if has none
        if (!mDestinationSet)
            SearchWalkPoint();
        // Start walking towards valid a point
        if (mDestinationSet)
        {
            // Check if reached destination
            Vector3 distanceToWalkPoint = mEnemy.transform.position - mDestination;
            distanceToWalkPoint.y = 0;
            mDestinationSet = distanceToWalkPoint.magnitude > 1f;
        }
    }

    protected virtual void SearchWalkPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * mEnemy.mEnemySettings.mWalkPointRange;
        randomDirection += mStartingPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, mEnemy.mEnemySettings.mWalkPointRange, 1))
        {
            if (hit.position.y > mEnemy.transform.position.y + .5f) return;

            NavMeshPath path = new NavMeshPath();
            mEnemy.mMeshAgent.CalculatePath(hit.position, path);
            mEnemy.mMeshAgent.SetPath(path);
            mDestination = hit.position;
#if UNITY_EDITOR
            mEnemy.mWalkPoint = mDestination;
#endif

            mDestinationSet = true;
        }
    }
}
