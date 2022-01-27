using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinosMageProjectile : BaseProjectile
{
    [SerializeField]
    int mDamage = 0;

    [SerializeField]
    protected string[] playerCollisionSFX;

    [SerializeField]
    protected string[] wallCollisionSFX;

    private void OnTriggerEnter(Collider other)
    {
        IAffectable affectable = other.GetComponent<IAffectable>();

        if (affectable != null)
        {
            AudioManager.PlayRandomSFX(mHitTargetSFX);
            affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Fire);
            affectable.DamageOverTime(2, 5, AttackFlags.Enemy | AttackFlags.Fire);
        }
        else
        {
            StartCoroutine(DestroyAfterTime());
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(1);
        Instantiate(mHitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
