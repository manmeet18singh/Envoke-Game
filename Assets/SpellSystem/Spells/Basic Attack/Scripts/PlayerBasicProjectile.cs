using UnityEngine;

public class PlayerBasicProjectile : BaseProjectile
{
    [SerializeField]
    int mDamage = 0;

    private void OnTriggerEnter(Collider other)
    {
        IAffectable enemyHealth = other.GetComponent<IAffectable>();

        if (enemyHealth != null)
        {
            AudioManager.PlayRandomSFX(mHitTargetSFX);
            enemyHealth.TakeDamage(mDamage, AttackFlags.Player | AttackFlags.Arcane);
        }

        Instantiate(mHitEffect, transform.position, Quaternion.identity);
#if UNITY_EDITOR
        Debug.Log("Player projectile hit: " + other.name);
#endif

        Destroy(gameObject);
    }
}
