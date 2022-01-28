#region Class EnemyScriptableObject
////////////////////////////////////////////////////////////////////////////////
/// Purpose: This class holds the BASE STATS for an enemy. These can be modified by at object 
/// creation time to buff up enemies, set up NavMeshAgent and to reset their stats if they died 
/// or were modified at runtime. 
///
/// Author: Manmeet Singh
////////////////////////////////////////////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "Enemies/Create Enemy")]
public class EnemySettings : ScriptableObject
{
    public Enemy mPrefab;
    //public AttackScriptableObject mAttackConfiguration

    [Header("Custom Enemy Variables")]
    public float mWalkPointRange = 10f;
    public LayerMask mGroundLayerMask;
    public LayerMask mObstacleLayerMask;
    public LayerMask mPlayerLayerMask;
    [Range(.01f, 1f)] public float mLookAtSpeed = 0.15f;
    public int mEnemyDifficulty = 1;

    [Header("AI Options")]
    public float mTimeToForgetPlayer = 4;

    [Header("Attack Configurations:")]
    //public EnemyAttackBehavior mEnemyAttackBehavior;
    public float mAttackRadius = 5f;
    public float mDetectionRadius = 20f;
    [Range(0, 360)] public float mDetectAngle = 50f;
    public float mAttackDelay = 5f;
    [Range(10, 100)] public float mChargeSpeed = 30f;

    [Header("Movement Speeds:")]
    public float mSpeed = 3f;
    public float mRunSpeed = 3f;

    //Enemy NavMeshAgent Configs
    [Header("NavMesh Agent Configs:")]
    //public NavMeshAgent mAgentType;
    public float mRadius = 0.5f;
    public float mHeight = 2f;
    public float mBaseOffset = 0;
    public float mAcceleration = 8;
    public int mAvoidancePriority = 50;
    public float mStoppingDistance = 0.5f;
    public float mAngularSpeed = 120;
    // -1 means everything
    public int mAreaMask = -1;
    public ObstacleAvoidanceType mObstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

    [Header("SFX")]
    public string[] mDeathSFX;
    public string[] mWoundedSFX;


    //Enemy States
    //[Header("AI State Behaviors:")]
    //public IState mDefaultState = new State_Idle
    //public IState mSearchState;
    //public IState mAttackState;
    //public IState mIdleState;

    public string GetRandomSFX(string[] _sfxList)
    {
        if (_sfxList.Length == 0)
            return "";
        else
            return _sfxList[Random.Range(0, _sfxList.Length)];
    }


    public virtual void SetupNavMeshAgent(Enemy enemy)
    {
        //enemy.mMeshAgent.agentTypeID = mAgentTypeID;
        enemy.mMeshAgent.acceleration = mAcceleration;
        enemy.mMeshAgent.angularSpeed = mAngularSpeed;
        enemy.mMeshAgent.areaMask = mAreaMask;
        enemy.mMeshAgent.avoidancePriority = mAvoidancePriority;
        enemy.mMeshAgent.baseOffset = mBaseOffset;
        enemy.mMeshAgent.height = mHeight;
        enemy.mMeshAgent.obstacleAvoidanceType = mObstacleAvoidanceType;
        enemy.mMeshAgent.radius = mRadius;
        enemy.mMeshAgent.speed = mSpeed;
        enemy.mMeshAgent.stoppingDistance = mStoppingDistance;
    }
}
