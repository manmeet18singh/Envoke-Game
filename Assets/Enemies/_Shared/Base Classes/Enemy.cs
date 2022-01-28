using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    #region References
    [SerializeField] public Animator mAnimator;
    [SerializeField] protected Collider mCollider;
    [HideInInspector] public NavMeshAgent mMeshAgent;
    #endregion

    #region Properties
    [SerializeField] public EnemySettings mEnemySettings;
    [SerializeField] public EnemyAttackBehavior mEnemyAttackBehavior;
    [SerializeField] public EnemyHealth mHealth;
    [SerializeField] public readonly bool mManuallyPlaced = false;
    private PickupData mGoldData;
    [HideInInspector] public Transform mTarget;
    #endregion

    #region State Variables
    [HideInInspector] protected StateMachine mStateMachine = null;
    [HideInInspector] protected IState mIdleState;

    // State variables regarding vision on target
    [HideInInspector] public float mTimeSinceTargetDetected = 0;
    [HideInInspector] public bool mTargetNearby = false;
    [HideInInspector] public bool mWasHitByPlayer = false;
    [HideInInspector] public bool mIsAttacking = false;
    [HideInInspector] private float mTimeLastHitByPlayer = 0;
    [HideInInspector] public float TimeLastHitByPlayer { get => mTimeLastHitByPlayer; }
    #endregion

    #region Other Variables
    private Coroutine mSlowCoroutine;
    // Caches for enemy settings used in trig functions
    [HideInInspector] private float mPlayerDetectionRangeSquared;
    [HideInInspector] private float mPlayerAttackRangeSquared;
    [HideInInspector] public bool mHasWalkPoint = false;

    [HideInInspector] public bool mHit = false;
    #endregion


    protected virtual void Awake()
    {
        mPlayerDetectionRangeSquared = Mathf.Pow(mEnemySettings.mDetectionRadius, 2);
        mPlayerAttackRangeSquared = Mathf.Pow(mEnemySettings.mAttackRadius, 2);
        mMeshAgent = GetComponent<NavMeshAgent>();

        if (mStateMachine == null)
            mStateMachine = new StateMachine();

        if (CurrencyManager.Instance != null)
            mGoldData = CurrencyManager.Instance.mGoldData;

        mHealth.mEnemyDying += EnemyDying;
        SetNavMeshAgentDefaults();

        if (mManuallyPlaced)
            GameManager.Instance.UpdateEnemiesRemaining(1);
    }

    protected virtual void Start()
    {
        if (mTarget == null)
            mTarget = GameManager.Instance.mPlayer.transform;
    }

    /// <summary>
    /// This function applies default nav mesh settings as 
    /// defined in mEnemySettings.
    /// </summary>
    protected virtual void SetNavMeshAgentDefaults()
    {
        mMeshAgent.acceleration = mEnemySettings.mAcceleration;
        mMeshAgent.angularSpeed = mEnemySettings.mAngularSpeed;
        mMeshAgent.areaMask = mEnemySettings.mAreaMask;
        mMeshAgent.avoidancePriority = mEnemySettings.mAvoidancePriority;
        mMeshAgent.baseOffset = mEnemySettings.mBaseOffset;
        mMeshAgent.height = mEnemySettings.mHeight;
        mMeshAgent.obstacleAvoidanceType = mEnemySettings.mObstacleAvoidanceType;
        mMeshAgent.radius = mEnemySettings.mRadius;
        mMeshAgent.speed = mEnemySettings.mSpeed;
        mMeshAgent.stoppingDistance = mEnemySettings.mStoppingDistance;
    }

    virtual protected void FixedUpdate()
    {
        UpdateTargetPosition();
        mStateMachine.Tick();
    }

    /// <summary>
    /// Event function that is called whenever a player deals damage
    /// to the enemy. It is used to trigger certain state changes in
    /// the enemy's AI.
    /// </summary>
    public virtual void HitByPlayer()
    {
        mTimeSinceTargetDetected = 0;
        mWasHitByPlayer = true;
        mTimeLastHitByPlayer = Time.time;

        // Play a hit effect approximately every other time hit to reduce sound overload
        if (UnityEngine.Random.Range(0, 100) > 50)
            AudioManager.instance.Play(mEnemySettings.GetRandomSFX(mEnemySettings.mWoundedSFX));
    }

    /// <summary>
    /// Event function that is called whenever EnemyHealth takes 
    /// enough damage to reach zero. This function is called BEFORE
    /// Death(), and is meant to take care of animation and any
    /// logic that must run before the enemy is cleaned up in memory.
    /// </summary>
    public virtual void EnemyDying()
    {
        if (mCollider)
            mCollider.enabled = false;
        mStateMachine.SetState(new State_Death(this));
        mHealth.mEnemyDying -= EnemyDying;

        int chanceToDropPickup = mEnemySettings.mEnemyDifficulty * 20;
        AudioManager.instance.Play(mEnemySettings.GetRandomSFX(mEnemySettings.mDeathSFX));
        if (UnityEngine.Random.Range(0, 100) < chanceToDropPickup)
        {
            Vector3 pickupLocation = transform.position;
            pickupLocation.y = GameManager.Instance.mPlayer.transform.position.y;
            pickupLocation.x += UnityEngine.Random.Range(-2, 2);
            pickupLocation.z += UnityEngine.Random.Range(-2, 2);
            float randTime = ((float)UnityEngine.Random.Range(500, 1000)) / 1000;
            GameManager.Instance.SpawnPickup(GameManager.Instance.GetRandomPickup(), pickupLocation, transform.position, randTime);
        }

        if(CurrencyManager.Instance.DropGold())
        {
            Vector3 goldLocation = transform.position;
            goldLocation.y = GameManager.Instance.mPlayer.transform.position.y;
            goldLocation.x += UnityEngine.Random.Range(-2, 2);
            goldLocation.z += UnityEngine.Random.Range(-2, 2);
            GameManager.Instance.SpawnPickup(mGoldData, goldLocation, transform.position, ((float)UnityEngine.Random.Range(500, 1000)) / 1000);
        }

    }
    
    #region Target Tracking
    /// <summary>
    /// Function called every frame that does various checks to determine
    /// where the target is w/respect to the enemy. This is critical for
    /// most AI functionality.
    /// </summary>
    protected void UpdateTargetPosition()
    {
        // If nothing is obstructing the view of the target, and the target is within sight range,
        //  and the target is within the enemy's FoV => player is detected!
        if (TargetInDetectionRange()
            && !TargetOccluded()
            && TargetWithinFieldOfView())
        {
            mTargetNearby = true;
            mTimeSinceTargetDetected = 0;
        }
        else
        {
            mTargetNearby = false;
            mTimeSinceTargetDetected += Time.deltaTime;
        }
    }

    public bool TargetInAttackRange()
    {
        return (transform.position - mTarget.position).sqrMagnitude < mPlayerAttackRangeSquared;
    }

    public bool TargetInDetectionRange()
    {
        return (transform.position - mTarget.position).sqrMagnitude < mPlayerDetectionRangeSquared;
    }

    public bool TargetOccluded()
    {
        Vector3 offsetR = transform.position + transform.right.normalized;
        Vector3 offsetL = transform.position + transform.right.normalized * -1;
        return Physics.Linecast(transform.position, mTarget.position, mEnemySettings.mObstacleLayerMask)
            || Physics.Linecast(offsetR, mTarget.position, mEnemySettings.mObstacleLayerMask)
            || Physics.Linecast(offsetL, mTarget.position, mEnemySettings.mObstacleLayerMask);
    }

    public bool TargetWithinFieldOfView()
    {
        Vector3 playerPos = GameManager.Instance.mPlayer.transform.position;
        playerPos.y = 0;
        Vector3 goblinPos = transform.position;
        goblinPos.y = 0;

        return Vector3.Angle((playerPos - goblinPos).normalized, transform.forward) < (mEnemySettings.mDetectAngle / 2);
    }
    #endregion

    #region Affectable Functions
    public virtual void Freeze(float duration)
    {
        mStateMachine.SetState(new State_Freeze(mStateMachine, mIdleState, mAnimator, duration));
    }
    
    public void SlowOverTime(int _timeTillZero, float _minMoveSpeed, bool _freeze, float _duration)
    {
        float speedPerTick = mEnemySettings.mSpeed / _timeTillZero;
        mSlowCoroutine = StartCoroutine(SlowOverTime(speedPerTick, _freeze, _duration));
    }

    public void StopSlowOverTime()
    {
        if(mSlowCoroutine != null)
            StopCoroutine(mSlowCoroutine);
        mMeshAgent.speed = mEnemySettings.mSpeed;
    }

    IEnumerator SlowOverTime(float speedPerTick, bool _freeze, float _duration)
    {
        float currentSpeed = mEnemySettings.mSpeed;

        while(currentSpeed > 0)
        {
            yield return new WaitForSeconds(1f);
            currentSpeed -= speedPerTick;
            mMeshAgent.speed = currentSpeed;
        }

        if (_freeze)
            Freeze(_duration);
    }
    #endregion

    public void SetChaseSpeed()
    {
        mMeshAgent.speed = mEnemySettings.mRunSpeed;
    }

    public void SetWalkSpeed()
    {
        mMeshAgent.speed = mEnemySettings.mSpeed;
    }

    [HideInInspector] public Vector3 mWalkPoint;
    #region Debug
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, mEnemySettings.mAttackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mEnemySettings.mDetectionRadius);

        Vector3 viewAngleA = DirectionFromAngle(-mEnemySettings.mDetectAngle / 2, false);
        Vector3 viewAngleB = DirectionFromAngle(mEnemySettings.mDetectAngle / 2, false);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * mEnemySettings.mDetectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * mEnemySettings.mDetectionRadius);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(mWalkPoint, 3f);

        Gizmos.color = Color.magenta;
        if (mTargetNearby)
        {
            Gizmos.DrawLine(transform.position, mTarget.transform.position);
        }
    }
#endif
    private Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    #endregion
}
