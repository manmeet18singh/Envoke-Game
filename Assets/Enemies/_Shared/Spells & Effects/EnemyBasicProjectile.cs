using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicProjectile : BaseProjectile
{
    [SerializeField]
    int mDamage = 0;

    [SerializeField]
    protected string[] castSFX = null;

    [SerializeField]
    protected string[] playerCollisionSFX = null;

    [SerializeField]
    protected string[] wallCollisionSFX = null;

    private void OnTriggerEnter(Collider other)
    {
        IAffectable affectable = other.GetComponent<IAffectable>();

        if (affectable != null)
        {
            AudioManager.PlayRandomSFX(mHitTargetSFX);
            affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Arcane);
        }

        Instantiate(mHitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
