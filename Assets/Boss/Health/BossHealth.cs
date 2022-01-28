using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    [Header("Boss Properties")]
    [SerializeField]
    BossHealthData mHealthData = null;
    //[SerializeField]
    //public IntEventChannelSO mBossPhaseChange = null;
    BossHealthUI mHealthUI;
    [SerializeField] IntEventChannelSO mBossDamaged = null;
    [SerializeField] FinalBoss mBoss;

    int mStage = 0;
    public int CurrentStage { get => mStage+1; }
    
    protected override void Awake()
    {
        mMaxHealth = mHealthData.mHealthThresholds[mStage];
        mCurrentHealth = mMaxHealth;
        mHealthUI = GameManager.Instance.mBossUI.GetComponent<BossHealthUI>();
    }

    public override void TakeDamage(int _damage, AttackFlags _damageFlags)
    {

        mCurrentHealth -= _damage;
        mBossDamaged.RaiseEvent(mCurrentHealth);
        //Debug.Log("Current Health: " + mCurrentHealth + ", Max Health: " + mMaxHealth);
        mHealthUI.UpdateHealth(mCurrentHealth);
        if (mCurrentHealth <= 0)
        {
            //Debug.Log("stage is " + (mStage + 1) + " out of " + (mHealthData.mHealthThresholds.Length - 1));
            if (mStage < mHealthData.mHealthThresholds.Length - 1)
            {
                mCurrentHealth = mHealthData.mHealthThresholds[++mStage];
                mMaxHealth = mCurrentHealth;
                mHealthUI.StageChange(mStage);
                mBoss.StageChanged(mStage);
                //Need boss phase change event
            }
            else
            {
                Dying();
            }
        }
    }

    public override void Freeze(float _duration)
    {}

    public override void Stun(float _duration)
    {}

    public override void Dying()
    {
        GetComponent<FinalBoss>().mAnimator.SetTrigger("Dead");
    }

    public override void Death()
    {
        mHealthUI.gameObject.SetActive(false);
        base.Death();
        GameManager.Instance.GameWon();
    }
}
