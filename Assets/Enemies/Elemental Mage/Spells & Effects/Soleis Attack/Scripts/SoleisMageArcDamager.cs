using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoleisMageArcDamager : MonoBehaviour
{
    #region Properties
    [Header("Damage Properties")]
    [SerializeField]
    private int mDamage = 0;
    [SerializeField]
    private float mTimeBetweenZaps = 0.5f;

    [Header("VFX")]
    [SerializeField] private GameObject mLightningBolt = null;

    [Header("SFX")]
    [SerializeField]
    protected string[] mHitTargetSFX;
    #endregion

    #region State Variables
    private float mTimeSinceLastZap = 0;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        IAffectable affectable = other.GetComponent<IAffectable>();
        if (affectable != null)
        {
            AudioManager.PlayRandomSFX(mHitTargetSFX);
            affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Lightning);
            affectable.Freeze(0.5f);

            SpawnLightningEffect(other.gameObject);
            mTimeSinceLastZap = 0;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (mTimeSinceLastZap < mTimeBetweenZaps)
            return;

        IAffectable affectable = other.GetComponent<IAffectable>();
        if (affectable != null)
        {
            AudioManager.PlayRandomSFX(mHitTargetSFX);
            affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Lightning);

            SpawnLightningEffect(other.gameObject);
            mTimeSinceLastZap = 0;
        }
    }

    private void SpawnLightningEffect(GameObject _destination)
    {
        GameObject lightningBolt = Instantiate(mLightningBolt, transform.position, Quaternion.identity);
        LineController lineController = lightningBolt.GetComponent<LineController>();
        lineController.mStaticDestinationYPos = 1.5f;
        lineController.mSource = gameObject;
        lineController.mDestination = _destination;
    }

    private void Update()
    {
        mTimeSinceLastZap += Time.deltaTime;
    }
}
