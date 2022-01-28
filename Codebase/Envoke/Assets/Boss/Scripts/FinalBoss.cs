using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BossHealth), typeof(NavMeshAgent))]
public class FinalBoss : MonoBehaviour
{
    #region References
    [Header("Spells")]
    [SerializeField] public CastFlameWreath mFlameWreath;
    [Range(0,500)] [SerializeField] private float mFlameWreathWeight = 100;
    [SerializeField] public CastIceTorrent mIceTorrent;
    [Range(0,500)] [SerializeField] private float mIceTorrentWeight = 100;
    [SerializeField] public CastSummonPortals mSummonPortals;
    [Range(0,500)] [SerializeField] private float mSummonPortalsWeight = 200;

    [Range(0, 500)] [SerializeField] private float mDashWeight = 200;
    [SerializeField] public float mDashStoppingDistance = 2;
    [SerializeField] public CastSummonPortals mFrenzySummonPortals;
    [Range(0, 500)] [SerializeField] private float mFrenzySummonWeight = 50;
    [SerializeField] public CastBossLaserSpell mFrenzyAoE;

    [Range(0, 500)] [SerializeField] private float mFrenzyAoEWeight = 190;

    [Header("References")]
    [SerializeField] public BossHealth mHealth;
    [SerializeField] public NavMeshAgent mNavMeshAgent;
    [SerializeField] public Animator mAnimator;
    [SerializeField] public GameObject mSword;
    [SerializeField] public GameObject mFireHandRight;
    [SerializeField] public GameObject mFireHandLeft;
    [SerializeField] public GameObject mSummonHandRight;
    [SerializeField] public GameObject mSummonHandLeft;
    [SerializeField] private BossSwordDamage mSwordDamager = null;

    [HideInInspector] private List<GameObject> mPortalSpawnPoints;
    public List<GameObject> PortalSpawnPoints { set => SetPortalSpawnPoints(value); }

    private void SetPortalSpawnPoints(List<GameObject> _portalSpwnPoints)
    {
        mPortalSpawnPoints = _portalSpwnPoints;
        mSummonPortals.mPortalSpawns = GetLocationsFromObjects(mPortalSpawnPoints);
        mFrenzySummonPortals.mPortalSpawns = mSummonPortals.mPortalSpawns;
    }

    [HideInInspector] private Vector3 mSpawnPoint;
    public Vector3 SpawnPoint { set => mSpawnPoint = value; }
    #endregion

    #region Properties
    private float mMinPossibleWeight = 0.1f;

    [Header("Stage 1 Properties")]
    [SerializeField] public float mWalkPointRange = 30;
    [SerializeField] public float mStage1Speed = 5;

    [Header("Stage 2 Properties")]
    [SerializeField] public float mRushSpeed = 20;
    #endregion

    #region State Variables
    private float mMaxWeight = 0;
    private float mMinWeight = 0;

    private StateMachine mStage1StateMachine;
    private StateMachine mStage2StateMachine;

    private StateMachine mCurrentStateMachine;

    private BS1_CastFlameWreath mStateCastFlameWreath;
    private BS1_CastIceTorrent mStateCastIceTorrent;
    private BS1_SummonMinionPortals mStateSummonPortals;
    private BS1_MoveAround mStateMoveAround;

    private BS2_RushPlayer mStateRushPlayer;
    private BS2_SummonMinionPortals mStateFrenzySummonPortals;
    private BS2_SlashAttacks mStateFrenzySlashAttacks;
    private BS2_FrenzyAoE mStateFrenzyAoEAttack;

    private StageTransitionState mStageTransitionState;

    // Stage 1 time state variables
    [HideInInspector] public float mTimeSinceLastCastFlameWreath = 0;
    [HideInInspector] public float mTimeSinceLastCastIceTorrent = 0;
    [HideInInspector] public float mTimeSinceLastSummonedPortals = 0;
    [HideInInspector] public float mTimeSinceLastCastLaser = 0;
    [HideInInspector] public float mTimeMovingAround = 0;

    // Stage 3 time state variables
    [HideInInspector] public float mTimeSinceLastSlashed = 0;
    [HideInInspector] public float mTimeSinceLastDash = 0;
    [HideInInspector] public float mTimeSinceLastAoEAttack = 0;
    [HideInInspector] public float mTimeSinceLastFrenzySummoned = 0;
    #endregion

    /// <summary>
    /// The current plan is this:
    ///     - During stages 1 and 2, the boss will alternate between casting his spells and returning to 
    ///     the "Move Around" state. After casting a spell, he will have to transition to moving around before 
    ///     he can consider casting another spell. The longer he stays in moving around, and the longer its been
    ///     since he's casted a particular spell, the more likely he is to transition into that spell's state.
    ///     
    ///     - The "Want to" predicates take into account how long its been since certain states have happened,
    ///     and create propabilities accordingly. These predicates should only be used while in the "Move Around"
    ///     state as it's likely that no transitions will return true for some time until the times since last
    ///     casting a spell go up.
    /// </summary>
    private void Awake()
    {

        // Set up spell references
        mFlameWreath.mBoss = this;
        mIceTorrent.mBoss = this;
        mSummonPortals.mBoss = this;
        mFrenzySummonPortals.mBoss = this;
        mFrenzyAoE.mBoss = this;

        mStageTransitionState = new StageTransitionState(this);

        // Set up Stage 1 state machine
        mStage1StateMachine = new StateMachine();
        mStateCastFlameWreath = new BS1_CastFlameWreath(this);
        mStateCastIceTorrent = new BS1_CastIceTorrent(this);
        mStateSummonPortals = new BS1_SummonMinionPortals(this);
        mStateMoveAround = new BS1_MoveAround(this);

        Func<bool> isDoneSummoningPortals() => () => mSummonPortals.HasSummonedAllPortals;
        Func<bool> wantsToCastFlameWreath() => () => mStateMoveAround.mHasMoved && WantsToCastFlameWreath();
        Func<bool> wantsToSummonPortals() => () => mStateMoveAround.mHasMoved && WantsToSummonPortals();
        Func<bool> wantsToCastIceTorrent() => () => mStateMoveAround.mHasMoved && WantsToCastIceTorrent();
        Func<bool> isDoneCastingFlameWreath() => () => mFlameWreath.IsDoneCasting;
        Func<bool> isDoneCastingIceTorrent() => () => mIceTorrent.IsDoneCasting;

        mStage1StateMachine.AddTransition(mStateSummonPortals, mStateMoveAround, isDoneSummoningPortals());
        mStage1StateMachine.AddTransition(mStateCastFlameWreath, mStateMoveAround, isDoneCastingFlameWreath());
        mStage1StateMachine.AddTransition(mStateCastIceTorrent, mStateMoveAround, isDoneCastingIceTorrent());
        mStage1StateMachine.AddTransition(mStateMoveAround, mStateSummonPortals, wantsToSummonPortals());
        mStage1StateMachine.AddTransition(mStateMoveAround, mStateCastFlameWreath, wantsToCastFlameWreath());
        mStage1StateMachine.AddTransition(mStateMoveAround, mStateCastIceTorrent, wantsToCastIceTorrent());

        //mStage1StateMachine.AddTransition(mStageTransitionState, );
        // Set up Stage 2 state machine
        mStage2StateMachine = new StateMachine();
        mStateFrenzySummonPortals = new BS2_SummonMinionPortals(this);
        mStateRushPlayer = new BS2_RushPlayer(this);
        mStateFrenzySlashAttacks = new BS2_SlashAttacks(this);
        mStateFrenzyAoEAttack = new BS2_FrenzyAoE(this);

        Func<bool> slashToSummon() => () => mStateFrenzySlashAttacks.HasFinishedSlashing && WantsToFrenzySummon();
        Func<bool> slashToRush() => () => mStateFrenzySlashAttacks.HasFinishedSlashing && WantsToRushPlayer();
        Func<bool> isDoneRushing() => () => mStateRushPlayer.IsDoneRushing;
        Func<bool> slashToAoE() => () => mStateFrenzySlashAttacks.HasFinishedSlashing && WantsToFrenzyAoE();
        Func<bool> slashToFlameWreath () => () => mStateFrenzySlashAttacks.HasFinishedSlashing && WantsToCastFlameWreath();
        Func<bool> slashToIceTorrent() => () => mStateFrenzySlashAttacks.HasFinishedSlashing && WantsToCastIceTorrent();
        Func<bool> summonToSlash() => () => mStateFrenzySummonPortals.mIsDoneCasting && 
            (transform.position - GameManager.Instance.mPlayer.transform.position).magnitude < 2;
        Func<bool> summonToRush() => () => mStateFrenzySummonPortals.mIsDoneCasting && WantsToRushPlayer();
        Func<bool> summonToAoE() => () => mStateFrenzySummonPortals.mIsDoneCasting && WantsToFrenzyAoE();
        Func<bool> summonToFlameWreath() => () => mStateFrenzySummonPortals.mIsDoneCasting && WantsToCastFlameWreath();
        Func<bool> summonToIceTorrent() => () => mStateFrenzySummonPortals.mIsDoneCasting && WantsToCastIceTorrent();
        Func<bool> flameWreathToRush() => () => mFlameWreath.IsDoneCasting && WantsToRushPlayer();
        Func<bool> flameWreathToSlash() => () => mFlameWreath.IsDoneCasting && 
            (transform.position - GameManager.Instance.mPlayer.transform.position).magnitude < 2;
        Func<bool> flameWreathToSummon() => () => mFlameWreath.IsDoneCasting && WantsToFrenzySummon();
        Func<bool> flameWreathToIceTorrent() => () => mFlameWreath.IsDoneCasting && WantsToCastIceTorrent();
        Func<bool> flameWreathToAoE() => () => mFlameWreath.IsDoneCasting && WantsToFrenzyAoE();
        Func<bool> iceTorrentToRush() => () => mIceTorrent.IsDoneCasting && WantsToRushPlayer();
        Func<bool> iceTorrentToSlash() => () => mIceTorrent.IsDoneCasting && 
            (transform.position - GameManager.Instance.mPlayer.transform.position).magnitude < 2;
        Func<bool> iceTorrentToSummon() => () => mIceTorrent.IsDoneCasting && WantsToFrenzySummon();
        Func<bool> iceTorrentToFlameWreath() => () => mIceTorrent.IsDoneCasting && WantsToCastFlameWreath();
        Func<bool> iceTorrentToAoE() => () => mIceTorrent.IsDoneCasting && WantsToFrenzyAoE();
        Func<bool> frenzyAoEAttackToSlash() => () => mFrenzyAoE.IsDoneCasting &&
            (transform.position - GameManager.Instance.mPlayer.transform.position).magnitude < 2;
        Func<bool> frenzyAoEAttackToRush() => () => mFrenzyAoE.IsDoneCasting && WantsToRushPlayer();
        Func<bool> frenzyAoEAttackToFlameWreath() => () => mFrenzyAoE.IsDoneCasting && WantsToCastFlameWreath();
        Func<bool> frenzyAoEAttackToIceTorrent() => () => mFrenzyAoE.IsDoneCasting && WantsToCastIceTorrent();
        Func<bool> frenzyAoEAttackToSummon() => () => mFrenzyAoE.IsDoneCasting && WantsToFrenzySummon();
        Func<bool> frenzyAoETimeUp() => () => mFrenzyAoE.IsDoneCasting;


        // Boss Stage Transition State
        Func<bool> transitionDone()=> () => mStageTransitionState.mIsDoneTransitioning;
        mStage2StateMachine.AddTransition(mStageTransitionState, mStateFrenzySummonPortals, transitionDone());

        // Slash
        mStage2StateMachine.AddTransition(mStateFrenzySlashAttacks, mStateRushPlayer, slashToRush());
        mStage2StateMachine.AddTransition(mStateFrenzySlashAttacks, mStateFrenzyAoEAttack, slashToAoE());
        mStage2StateMachine.AddTransition(mStateFrenzySlashAttacks, mStateFrenzySummonPortals, slashToSummon());
        mStage2StateMachine.AddTransition(mStateFrenzySlashAttacks, mStateCastFlameWreath, slashToFlameWreath());
        mStage2StateMachine.AddTransition(mStateFrenzySlashAttacks, mStateCastIceTorrent, slashToIceTorrent());
        // Rush
        mStage2StateMachine.AddTransition(mStateRushPlayer, mStateFrenzySlashAttacks, isDoneRushing());
        // AoE
        mStage2StateMachine.AddTransition(mStateFrenzyAoEAttack, mStateFrenzySlashAttacks, frenzyAoEAttackToSlash());
        mStage2StateMachine.AddTransition(mStateFrenzyAoEAttack, mStateRushPlayer, frenzyAoEAttackToRush());
        mStage2StateMachine.AddTransition(mStateFrenzyAoEAttack, mStateCastFlameWreath, frenzyAoEAttackToFlameWreath());
        mStage2StateMachine.AddTransition(mStateFrenzyAoEAttack, mStateCastIceTorrent, frenzyAoEAttackToIceTorrent());
        mStage2StateMachine.AddTransition(mStateFrenzyAoEAttack, mStateFrenzySummonPortals, frenzyAoEAttackToSummon());
        mStage2StateMachine.AddTransition(mStateFrenzyAoEAttack, mStateRushPlayer, frenzyAoETimeUp());
        // Summon
        mStage2StateMachine.AddTransition(mStateFrenzySummonPortals, mStateFrenzySlashAttacks, summonToSlash());
        mStage2StateMachine.AddTransition(mStateFrenzySummonPortals, mStateFrenzyAoEAttack, summonToAoE());
        mStage2StateMachine.AddTransition(mStateFrenzySummonPortals, mStateRushPlayer, summonToRush());
        mStage2StateMachine.AddTransition(mStateFrenzySummonPortals, mStateCastFlameWreath, summonToFlameWreath());
        mStage2StateMachine.AddTransition(mStateFrenzySummonPortals, mStateCastIceTorrent, summonToIceTorrent());
        // Flame Wreath
        mStage2StateMachine.AddTransition(mStateCastFlameWreath, mStateRushPlayer, flameWreathToRush());
        mStage2StateMachine.AddTransition(mStateCastFlameWreath, mStateFrenzySlashAttacks, flameWreathToSlash());
        mStage2StateMachine.AddTransition(mStateCastFlameWreath, mStateFrenzySummonPortals, flameWreathToSummon());
        mStage2StateMachine.AddTransition(mStateCastFlameWreath, mStateCastIceTorrent, flameWreathToIceTorrent());
        mStage2StateMachine.AddTransition(mStateCastFlameWreath, mStateFrenzyAoEAttack, flameWreathToAoE());
        // Ice Torrent
        mStage2StateMachine.AddTransition(mStateCastIceTorrent, mStateRushPlayer, iceTorrentToRush());
        mStage2StateMachine.AddTransition(mStateCastIceTorrent, mStateFrenzySlashAttacks, iceTorrentToSlash());
        mStage2StateMachine.AddTransition(mStateCastIceTorrent, mStateFrenzySummonPortals, iceTorrentToSummon());
        mStage2StateMachine.AddTransition(mStateCastIceTorrent, mStateCastFlameWreath, iceTorrentToFlameWreath());
        mStage2StateMachine.AddTransition(mStateCastIceTorrent, mStateFrenzyAoEAttack, iceTorrentToAoE());

        // Blink?

        // Attach listeners to BossHealth events
        //mHealth.mBossPhaseChange.OnEventRaised += StageChanged;
    }

    private void Start()
    {
        // Set current state machine to the first stage
        mCurrentStateMachine = mStage1StateMachine;
        mCurrentStateMachine.SetState(mStateSummonPortals);
    }

    private void Update()
    {
        mCurrentStateMachine.Tick();
        UpdateTimeVariables();
    }

    public void SetRushSpeed()
    {
        mNavMeshAgent.speed = mRushSpeed;
    }

    public void CastTeleportAndSummon()
    {
        transform.position = mSpawnPoint;

        if (mHealth.CurrentStage == 1)
        {
            mAnimator.SetTrigger("SummonPortalsT");
            //mSummonPortals.SetSpellVariation(UnityEngine.Random.Range(0, mSummonPortals.NumberOfVariations));
            //mSummonPortals.CastSpell();
        }
        else
        {
            mAnimator.SetTrigger("FrenzySummonPortalT");
            //mFrenzySummonPortals.SetSpellVariation(UnityEngine.Random.Range(0, mFrenzySummonPortals.NumberOfVariations));
            //mFrenzySummonPortals.CastSpell();
        }
    }

    #region Helper Functions
    public static List<Transform> GetLocationsFromObjects(List<GameObject> _summonLocations)
    {
        List<Transform> locations = new List<Transform>();
        foreach (GameObject locationObject in _summonLocations)
        {
            locations.Add(locationObject.transform);
        }

        return locations;
    }

    private void UpdateTimeVariables()
    {
        switch (mHealth.CurrentStage)
        {
            case 1:
                mTimeSinceLastCastFlameWreath += Time.deltaTime;
                mTimeSinceLastCastIceTorrent += Time.deltaTime;
                mTimeSinceLastSummonedPortals += Time.deltaTime;
                break;
            case 2:
                mTimeSinceLastCastFlameWreath += Time.deltaTime;
                mTimeSinceLastCastIceTorrent += Time.deltaTime;
                mTimeSinceLastSlashed += Time.deltaTime;
                mTimeSinceLastDash += Time.deltaTime;
                mTimeSinceLastAoEAttack += Time.deltaTime;
                mTimeSinceLastFrenzySummoned += Time.deltaTime;
                break;
            default:
#if UNITY_EDITOR
                Debug.LogWarning("Unknow stage change: " + mHealth.CurrentStage);
#endif
                break;
        }
    }

    public void StageChanged(int _stage)
    {  
        mMinWeight = mMaxWeight = 0;
        switch (_stage)
        {
            case 1:
                mSummonPortals.DestroyAllPortals();
                mStageTransitionState.mBoss = this;
                mStateFrenzySlashAttacks.mBoss = this;
                mCurrentStateMachine = mStage2StateMachine;
                mCurrentStateMachine.SetState(mStageTransitionState);
                break;
            default:
#if UNITY_EDITOR
                Debug.LogWarning("Unknown stage change: " + _stage);
#endif
                break;
        }
    }

    public void DoneCastingFrenzyPortals()
    {
        mStateFrenzySummonPortals.mIsDoneCasting = true;
    }

    public void DoneCastingPortals()
    {
        mStateSummonPortals.mIsDoneCasting = true;
    }

    public void FinishedSlashing()
    {
        mStateFrenzySlashAttacks.HasFinishedSlashing = true;
    }

    public void FinishedStageTransition()
    {
        mStageTransitionState.mIsDoneTransitioning = true;
    }
    public void SummonSword()
    {
        mStageTransitionState.SummonSword();
    }

    public void EnableSword(bool _enabled)
    {
        mSwordDamager.mColliderEnabled = _enabled;
    }
    #endregion

    #region Weight Calculators
    private void UpdateWeightEnds(float _weight)
    {
        //Debug.Log("Updating weight " + _weight + " (" + mMinWeight + " , " + mMaxWeight + ")");
        if (mMinWeight == 0 || mMinWeight > _weight)
            mMinWeight = _weight;
        if (mMaxWeight <= _weight)
            mMaxWeight = _weight;
    }

    private bool WantsToSummonPortals()
    {
        //Debug.Log("Time since portals were summoned: " + mTimeSinceLastSummonedPortals);
        return mTimeSinceLastSummonedPortals >= 10;

        //float weight = GetWeightValue(mSummonPortalsWeight) + GetLastCastWeight(mTimeSinceLastSummonedPortals) - GetMovementWeight();
        //UpdateWeightEnds(weight);
        //return weight >= UnityEngine.Random.Range(mMinWeight, mMaxWeight);
    }

    private bool WantsToCastIceTorrent()
    {
        float weight = Mathf.Min(GetWeightValue(mIceTorrentWeight) + GetLastCastWeight(mTimeSinceLastCastIceTorrent) - (mHealth.CurrentStage == 1 ? GetMovementWeight() : 0), mMinPossibleWeight);
        UpdateWeightEnds(weight);
        //Debug.Log("Ice Torrent weight: " + weight + " " + mTimeSinceLastCastFlameWreath);
        return weight >= UnityEngine.Random.Range(mMinWeight, mMaxWeight);
    }

    private bool WantsToCastFlameWreath()
    {
        float weight = Mathf.Min(GetWeightValue(mFlameWreathWeight) + GetLastCastWeight(mTimeSinceLastCastFlameWreath) - (mHealth.CurrentStage == 1 ? GetMovementWeight() : 0), mMinPossibleWeight);

        UpdateWeightEnds(weight);
        //Debug.Log("Flame Wreath weight: " + weight + " " + mTimeSinceLastCastFlameWreath);
        //Debug.Log("Min " + mMinWeight + " and Max " + mMaxWeight);
        return weight >= UnityEngine.Random.Range(mMinWeight, mMaxWeight);
    }


    private bool WantsToFrenzyAoE()
    {
        float weight = Mathf.Min(GetWeightValue(mFrenzyAoEWeight) + GetLastCastWeight(mTimeSinceLastAoEAttack), mMinPossibleWeight);
        //Debug.Log("AoE weight: " + GetWeightValue(mFrenzyAoEWeight) + GetLastCastWeight(mTimeSinceLastAoEAttack) + " " + mTimeSinceLastAoEAttack);
        UpdateWeightEnds(weight);
        return weight >= UnityEngine.Random.Range(mMinWeight, mMaxWeight);
    }

    private bool WantsToRushPlayer()
    {
        float weight = Mathf.Min(GetWeightValue(mDashWeight) + GetLastCastWeight(mTimeSinceLastDash), mMinPossibleWeight);
        //Debug.Log("Dash weight: " + GetWeightValue(mDashWeight) + GetLastCastWeight(mTimeSinceLastDash) + " " + mTimeSinceLastDash);
        UpdateWeightEnds(weight);
        return weight >= UnityEngine.Random.Range(mMinWeight, mMaxWeight);
    }

    private bool WantsToFrenzySummon()
    {
        if (mFrenzySummonPortals.SomePortalsDestroyed() && mTimeSinceLastSummonedPortals > 10)
            return true;
        float weight = Mathf.Min(GetWeightValue(mFrenzySummonWeight) + GetLastCastWeight(mTimeSinceLastFrenzySummoned), mMinPossibleWeight);
        UpdateWeightEnds(weight);
        //Debug.Log("Frenzy Summon weight: " + GetWeightValue(mFrenzySummonWeight) + GetLastCastWeight(mTimeSinceLastFrenzySummoned) + " " + mTimeSinceLastFrenzySummoned);
        return weight >= UnityEngine.Random.Range(mMinWeight, mMaxWeight);
    }

    private float GetMovementWeight()
    {
        return Mathf.Max(0, (1 / mTimeMovingAround + 0.01f));
    }

    private float GetLastCastWeight(float _compare)
    {
        switch (mHealth.CurrentStage)
        {
            case 1:
                return (_compare * _compare) / (mTimeSinceLastCastFlameWreath * mTimeSinceLastCastFlameWreath
                    + mTimeSinceLastCastIceTorrent * mTimeSinceLastCastIceTorrent
                    + mTimeSinceLastSummonedPortals / 2);
            case 2:
                return (_compare * _compare) / (mTimeSinceLastCastFlameWreath * mTimeSinceLastCastFlameWreath
                    + mTimeSinceLastCastIceTorrent * mTimeSinceLastCastIceTorrent
                    + mTimeSinceLastAoEAttack * mTimeSinceLastAoEAttack
                    + mTimeSinceLastDash * mTimeSinceLastDash
                    + mTimeSinceLastFrenzySummoned * mTimeSinceLastFrenzySummoned);
            default:
#if UNITY_EDITOR
                Debug.LogWarning("Unknown boss stage! (" + mHealth.CurrentStage + ")");
#endif
                return 0;
        }
    }

    private float GetWeightValue(float _compare)
    {
        switch (mHealth.CurrentStage)
        {
            case 1:
                return _compare / (mFlameWreathWeight + mIceTorrentWeight + mSummonPortalsWeight);
            case 2:
                return _compare / (mFlameWreathWeight + mIceTorrentWeight + mFrenzyAoEWeight + mFrenzySummonWeight + mDashWeight);
            default:
#if UNITY_EDITOR
                Debug.LogWarning("Unknown boss stage! (" + mHealth.CurrentStage + ")");
#endif
                return 0;
        }
    }
    #endregion
}
