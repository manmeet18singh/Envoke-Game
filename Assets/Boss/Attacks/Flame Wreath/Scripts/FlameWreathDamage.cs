using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWreathDamage : MonoBehaviour
{
    #region Properties
    [Header("Damage Properties")]
    [SerializeField] private int mInitialDamage = 10;
    [SerializeField] private int mDoTDamage = 4;
    [SerializeField] private int mDoTDuration = 5;
    [SerializeField] private int mTimeBetweenMultipleHits = 2;

    [Header("SFX")]
    [SerializeField] private string[] mHitSFX = null;
    [SerializeField] private string[] mBurnSFX;
    #endregion

    #region References
    public CastFlameWreath mParent;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (mParent.mTimeSinceLastHit == -1 || mParent.mTimeSinceLastHit > mTimeBetweenMultipleHits)
        {
            if (playerHealth != null)
            {
                DoDamage(playerHealth);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (mParent == null)
            return;

        if (mParent.mTimeSinceLastHit > mTimeBetweenMultipleHits)
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                DoDamage(playerHealth);
            }
        }
    }
    #region Helper Functions
    private void DoDamage(PlayerHealth _playerHealth)
    {
        if (mParent == null)
            return;

        AudioManager.PlayRandomSFX(mHitSFX);
        _playerHealth.TakeDamage(mInitialDamage, AttackFlags.Enemy | AttackFlags.Fire);
        if (!_playerHealth.CurrentStatus.HasFlag(StatusEffects.Burning))
        {
            _playerHealth.DamageOverTime(mDoTDamage, mDoTDuration, AttackFlags.Enemy | AttackFlags.Fire);
        }
        mParent.mTimeSinceLastHit = 0;
    }
    #endregion
}
