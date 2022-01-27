using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IceWallHealth : Health
{
    [SerializeField] Image mHealthBarImage = null;
    [SerializeField] GameObject mExplosionEffect = null;

    IceWallData mSpellData;

    [SerializeField] string[] mCastSFX = null;

    private void Start()
    {
        AudioManager.PlayRandomSFX(mCastSFX);
    }

    public void Initialize(IceWallData _spellData)
    {
        mSpellData = _spellData;
        mMaxHealth = _spellData.MaxHealth;
        mCurrentHealth = mMaxHealth;
        StartCoroutine(Melt());
    }

    
    public override void TakeDamage(int _damage, AttackFlags _damageFlags)
    {
        // Ice walls should take extra damage from fire attacks
        if (!_damageFlags.HasFlag(AttackFlags.Enemy))
            return;

        base.TakeDamage(_damageFlags.HasFlag(AttackFlags.Fire) ? _damage * 2 : _damage, _damageFlags);
        mHealthBarImage.fillAmount = mCurrentHealth / (float)mMaxHealth;
    }

    public override void Death()
    {
        if (mSpellData.CanExplode)
        {
            IceWallExplosion explosion = Instantiate(mExplosionEffect, transform.position, Quaternion.identity).GetComponent<IceWallExplosion>();
            explosion.Explode(mSpellData);
        }

        Destroy(gameObject);
    }

    IEnumerator Melt()
    {
        int damage = mMaxHealth / mSpellData.LifeTime;

        while (true)
        {
            yield return new WaitForSeconds(1f);
            TakeDamage(damage, AttackFlags.Enemy);
        }
    }

    public override void Freeze(float _duration)
    {
    }

    public override GameObject ApplyVFX(GameObject _VFX, float _duration, bool _useBaseTransform = false)
    {
        return null;
    }
}
