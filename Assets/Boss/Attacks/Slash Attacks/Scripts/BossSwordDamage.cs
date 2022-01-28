using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordDamage : MonoBehaviour
{
    #region Properties
    [SerializeField] private int mDamage = 5;
    [SerializeField] private GameObject mHitEffect = null;
    [SerializeField] private string[] mHitSFX = null;
    [SerializeField] private float mAttackDelay = 0f;
    #endregion

    // State variables
    float mTimeSinceLastAttack = 0;
    [HideInInspector] public bool mColliderEnabled = false;

    private void OnTriggerEnter(Collider _other)
    {
        if (!mColliderEnabled)
            return;

        IAffectable affectable = _other.GetComponent<IAffectable>();

        if (affectable != null)
        {
            if (Time.time - mTimeSinceLastAttack > mAttackDelay)
            {
                affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Physical);
                if (mHitEffect != null)
                    Instantiate(mHitEffect, _other.transform.position, Quaternion.identity);
                mTimeSinceLastAttack = Time.time;
                AudioManager.PlayRandomSFX(mHitSFX);
            }
        }
    }
}
