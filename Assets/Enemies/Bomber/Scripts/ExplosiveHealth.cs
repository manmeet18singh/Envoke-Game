using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class ExplosiveHealth : EnemyHealth
{
    [Header("Explode On Death")]
    [SerializeField] protected GameObject mExplosionPrefab;

    public override void Dying()
    {
        Instantiate(mExplosionPrefab, transform.position, transform.rotation);
        base.Dying();
    }
}
