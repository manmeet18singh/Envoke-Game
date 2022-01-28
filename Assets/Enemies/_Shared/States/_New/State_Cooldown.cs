using UnityEngine;

public class State_Cooldown : EnemyState
{
    public static readonly string DefaultAnimationName = "";

    #region Properties
    private int mMinIdleTime;
    private int mMaxIdleTime;
    #endregion

    #region Dynamic State Variables
    // A random time between mMinIdleTime and mMaxIdleTime
    private float mTimeToSitIdle;

    // Public accessor used for state transitions
    public bool HasBeenIdleTooLong { get => mTimeSpentIdle > mTimeToSitIdle; }

    // The actual time spent in this state
    private float mTimeSpentIdle = 0;
    #endregion

    public State_Cooldown(Enemy _enemy, int _minIdleTime = 1, int _maxIdleTime = 3, string _animationName = null) : base(_enemy, _animationName ?? DefaultAnimationName)
    {
        mMinIdleTime = _minIdleTime;
        mMaxIdleTime = _maxIdleTime;
    }

    override public void OnEnter()
    {
#if UNITY_EDITOR
        //Debug.Log("Beginning Idle State.");
#endif
        base.OnEnter();
        mEnemy.mMeshAgent.enabled = true;
        mEnemy.mMeshAgent.SetDestination(mEnemy.transform.position);
        mTimeSpentIdle = 0;
        mTimeToSitIdle = ((float)Random.Range(mMinIdleTime * 1000, mMaxIdleTime * 1000)) / 1000;
    }

    override public void Tick()
    {
        mTimeSpentIdle += Time.deltaTime;
    }
}
