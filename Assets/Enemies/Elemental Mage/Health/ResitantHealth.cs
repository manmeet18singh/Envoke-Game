using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResitantHealth : EnemyHealth
{
    [Header("Resitance Properties")]
    [SerializeField] public AttackFlags mResistanceType;
    [Range(0,100)] [SerializeField] public int mDamageReduction;

    public override void TakeDamage(int _damage, AttackFlags _damageFlags)
    {
        if (_damageFlags.HasFlag(mResistanceType))
        {
            float resistantDamage = _damage * ((float) (100 - mDamageReduction) / 100);
            base.TakeDamage(Mathf.RoundToInt(resistantDamage), _damageFlags);
        }
        else
        {
            base.TakeDamage(_damage, _damageFlags);
        }
    }

    public override void Freeze(float _duration)
    {
        if (mResistanceType.HasFlag(AttackFlags.Ice))
            return;

        base.Freeze(_duration);
    }
}
