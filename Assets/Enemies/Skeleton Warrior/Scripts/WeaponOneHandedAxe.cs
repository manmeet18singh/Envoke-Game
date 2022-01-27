using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOneHandedAxe : MonoBehaviour
{
    [SerializeField]
    private int mDamage = 5;
    [SerializeField]
    private Enemy mEnemy = null;
    [SerializeField]
    private GameObject mHitEffect = null;
    [SerializeField]
    private string[] mHitSFX = null;

    private EnemySettings mSettings;

    private void Awake()
    {
        mSettings = mEnemy.mEnemySettings;
    }

    // State variables
    float mTimeSinceLastAttack = 0;

    private void OnTriggerEnter(Collider _other)
    {
        IAffectable affectable = _other.GetComponent<IAffectable>();

        if (affectable != null)
        {
            if (Time.time - mTimeSinceLastAttack > mSettings.mAttackDelay)
            {
                affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Physical);
                Instantiate(mHitEffect, _other.transform.position, Quaternion.identity);
                mTimeSinceLastAttack = Time.time;
                AudioManager.PlayRandomSFX(mHitSFX);
            }
        }
    }
}
