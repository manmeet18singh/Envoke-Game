using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExploder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mParent;
    [SerializeField] private GameObject mInitialFuseEffect;
    [SerializeField] private GameObject mSecondFuseEffect;
    [SerializeField] private LayerMask mLayerMask;

    [Header("Effects")]
    [SerializeField] private GameObject mExplosionVFX;
    [SerializeField] private string[] mExplosionSFX;

    [Header("Damage Properties")]
    [SerializeField] private float mExplosionRadius = 10;
    [SerializeField] private int mInitialDamage = 10;
    [SerializeField] private int mDoTAmount = 3;
    [SerializeField] private int mDoTDuration = 3;
    [SerializeField] private float mKnockbackForce = 20f;

    [Header("Trigger Properties")]
    [SerializeField] private float mCookTime = 3;
    [SerializeField] private float mDetonationTime = 1;

    private float mTimeCooking = 0;
    private bool mHasExploded = false;
    private bool mTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BombTriggered()
    {
        mTriggered = true;
        mInitialFuseEffect.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
            BombTriggered();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mTriggered)
            return;

        mTimeCooking += Time.deltaTime;

        if (mTimeCooking > mCookTime)
        {
            mSecondFuseEffect.SetActive(true);
        }

        if (!mHasExploded && mTimeCooking > mCookTime + mDetonationTime)
        {
            Debug.Log("BOOM");
            mHasExploded = true;
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, mExplosionRadius);
    }

    private void Explode()
    {
        Vector3 playerDistance = (GameManager.Instance.mPlayer.transform.position - transform.position);

        if (playerDistance.magnitude < mExplosionRadius)
        {
            GameManager.Instance.mPlayerHealth.TakeDamage(mInitialDamage, AttackFlags.Enemy | AttackFlags.Fire);
            GameManager.Instance.mPlayerHealth.DamageOverTime(mDoTAmount, mDoTDuration, AttackFlags.Fire);

            PlayerMovement receiver = GameManager.Instance.mPlayerMovement;
            Vector3 dir = receiver.transform.position - transform.position;
            receiver.AddKnockback(dir, mKnockbackForce);
        }

        AudioManager.PlayRandomSFX(mExplosionSFX);
        Instantiate(mExplosionVFX, transform.position, Quaternion.identity);
        Destroy(mParent);
    }
}
