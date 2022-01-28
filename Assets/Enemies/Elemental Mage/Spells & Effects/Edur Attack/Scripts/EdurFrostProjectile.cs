using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdurFrostProjectile : BaseProjectile
{
    [SerializeField]
    int mDamage = 0;

    private void Start()
    {
        AudioManager.PlayRandomSFX(mCastSFX);
    }

    private void OnTriggerEnter(Collider other)
    {
        IAffectable affectable = other.GetComponent<IAffectable>();

        if (affectable != null)
        {
            Health health = other.GetComponent<Health>();
            AudioManager.PlayRandomSFX(mHitTargetSFX);
            affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Ice);
            if (!health.CurrentStatus.HasFlag(StatusEffects.Slowed))
                affectable.SlowOverTime(3, 1);
        }

        Instantiate(mHitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
