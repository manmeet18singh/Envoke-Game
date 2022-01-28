using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : Health
{
    [Header("UI References")]
    [SerializeField] protected GameObject mHealthBar = null;
    [SerializeField] protected Image mHealthBarImage = null;

    [Header("Event Callbacks")]
    [SerializeField] protected GameEvent mEnemyDead;
    public static event Action mEnemyDeath;
    public event Action mEnemyDying = null;

    [Header("Debug Feedback References")]
    [SerializeField] protected GameObject mMaterialObject = null;
    private Material mEnemyMat = null;

    // Initial state variables
    Color mInitialMatColor;

    private Enemy mEnemy;
    protected override void Awake()
    {
        base.Awake();
        mEnemy = GetComponent<Enemy>();
        if (mMaterialObject != null)
            mEnemyMat = mMaterialObject.GetComponent<Material>();
        if (mEnemyMat != null)
            mInitialMatColor = mEnemyMat.color;
    }

    public override void TakeDamage(int _damage, AttackFlags _damageFlags)
    {
        if (mHealthBar != null && !mHealthBar.activeInHierarchy)
            mHealthBar.SetActive(true);

        base.TakeDamage(_damage, _damageFlags);
        if (mHealthBarImage != null)
            mHealthBarImage.fillAmount = mCurrentHealth / (float)mMaxHealth;

        if (_damageFlags.HasFlag(AttackFlags.Player))
            mEnemy.HitByPlayer();

        StartCoroutine(FlashRed());
    }

    public override void Dying()
    {
        mHealthBar.SetActive(false);
        if (mEnemyDying != null)
        {
            mEnemyDying?.Invoke();
        }
        else
            base.Dying();
    }

    public override void Death()
    {
        GameManager.Instance.UpdateEnemiesRemaining(-1);
        mEnemyDeath?.Invoke();
        mEnemyDead?.Invoke();
        Destroy(gameObject);
    }

    IEnumerator FlashRed()
    {
        if (mEnemyMat != null)
        {
            mEnemyMat.color = Color.white;
            yield return new WaitForSeconds(.2f);
            mEnemyMat.color = mInitialMatColor;
        }
        else
            yield return new WaitForSeconds(.2f);
    }

    public override void Freeze(float _duration)
    {
        ApplyVFX(GameManager.Instance.mFreezeEffect, _duration);
        mEnemy?.Freeze(_duration);
    }

    public override void Stun(float _duration)
    {
        base.Stun(_duration);
        ApplyVFX(GameManager.Instance.mStunEffect, _duration);
        mEnemy?.Freeze(_duration);
    }

    private GameObject slowVFX;
    public override void SlowOverTime(int _timeTillZero, float _minMoveSpeed, bool _freeze, float _duration)
    {
#if UNITY_EDITOR
        Debug.Log("Slowing enemy now");
#endif
        if (slowVFX != null)
            Destroy(slowVFX);

        slowVFX = ApplyVFX(GameManager.Instance.mSlowEffect, _duration, true);
        mEnemy.SlowOverTime(_timeTillZero, _minMoveSpeed, _freeze, _duration);
    }

    public override void StopSlowOverTime()
    {
#if UNITY_EDITOR
        Debug.Log("Stop slowing enemy");
#endif
        if (slowVFX != null)
            Destroy(slowVFX);
        mEnemy.StopSlowOverTime();
    }
}
