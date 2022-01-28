using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyHealth : Health
{
    [SerializeField]
    Image mHealthFillImage = null;

    public override void TakeDamage(int _damage, AttackFlags _damageFlags)
    {
        base.TakeDamage(_damage, _damageFlags);
        mHealthFillImage.fillAmount = mCurrentHealth / (float)mMaxHealth;
    }

    public override void Stun(float _duration)
    {
        base.Stun(_duration);
        ApplyVFX(GameManager.Instance.mStunEffect, _duration);
    }

    public override void Freeze(float _duration)
    {
        base.Freeze(_duration);
        ApplyVFX(GameManager.Instance.mFreezeEffect, _duration);
    }

    public override void Dying()
    {
        mCurrentHealth = mMaxHealth;
        mHealthFillImage.fillAmount = 1f;
    }
}
