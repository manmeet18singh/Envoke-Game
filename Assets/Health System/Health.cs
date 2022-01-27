using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum StatusEffects
{
    Bleeding = 0,
    Burning = 1,
    Frozen = 2,
    Slowed = 3
}
public abstract class Health : MonoBehaviour, IAffectable
{
    [Header("Base Stats")]
    [SerializeField] protected int mMaxHealth;
    public int MaxHealth { get => mMaxHealth; }
    [SerializeField] protected int mCurrentHealth;
    public int CurrentHealth { get => mCurrentHealth; }
    [Header("VFX References")]
    [SerializeField] private GameObject mVFXAttachment = null;

    protected StatusEffects mCurrentStatus;
    public StatusEffects CurrentStatus { get => mCurrentStatus; }

    public int CurrentHealthPercent { get => (int)(mCurrentHealth / (float)mMaxHealth * 100); }

    protected virtual void Awake()
    {
        mCurrentHealth = mMaxHealth;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// TakeDamage
    /// 
    /// <summary>
    ///	Modifies health based on _damage.
    ///	Note: For health classes that overrid this function, modifiers based
    ///	    on _damageType should be done by calling base.TakeDamage() and passing in 
    ///	    the adjusted damage value into _damage.
    /// </summary>
    /// 
    /// <param name="_damage">Amount to reduce health by.</param>
    /// <param name="_damageFlags">Contains info on damage type and source.</param>
    public virtual void TakeDamage(int _damage, AttackFlags _damageFlags)
    {
        if (mCurrentHealth <= 0)
            return;

        mCurrentHealth = Mathf.Clamp(mCurrentHealth - _damage, 0, mMaxHealth);

        if (mCurrentHealth <= 0)
            Dying();
    }


    public virtual void Dying()
    {
        Death();
    }

    public virtual void Heal(int _healAmount)
    {
        mCurrentHealth = Mathf.Clamp(mCurrentHealth + _healAmount, 0, mMaxHealth);
    }

    public virtual void Death(){ }

    public virtual void Freeze(float _duration)
    {}

    public virtual void DamageOverTime(int _damagePerTick, int _duration, AttackFlags _damageFlags)
    {
        StartCoroutine(DOT(_damagePerTick, _duration, _damageFlags));
        if (_damageFlags.HasFlag(AttackFlags.Fire))
        {
            ApplyVFX(GameManager.Instance.mBurningEffect, _duration);
        }
    }
    
    public virtual IEnumerator DOT(int _damagePerTick, int _duration, AttackFlags _damageFlags)
    {
        while(_duration != 0)
        {
            --_duration;
            yield return new WaitForSeconds(1f);
            TakeDamage(_damagePerTick, _damageFlags);
        }

        if (_damageFlags.HasFlag(AttackFlags.Fire))
        {
            mCurrentStatus &= ~StatusEffects.Burning;
        }
    }

    public virtual void SlowOverTime(int _timeTillZero, float _minMoveSpeed, bool _freeze, float _duration) { }
    public virtual void StopSlowOverTime() { }

    public virtual GameObject ApplyVFX(GameObject _VFX, float _duration, bool _useBaseTransform = false)
    {
        if (_VFX == null)
            return null;

        GameObject newEffect = Instantiate(_VFX, _useBaseTransform ? transform : mVFXAttachment.transform);
        newEffect.GetComponent<EffectVFX>().InitializeVFX(_duration, _useBaseTransform ? gameObject : mVFXAttachment);
        return newEffect;
    }

    public virtual void Stun(float _duration)
    {}
}
