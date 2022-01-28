using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_ExplosionFactory : SpellFactoryBase
{
    [SerializeField] EnemySettings mEnemySettings = null;
    [SerializeField] Enemy mEnemy = null;
    [SerializeField] LayerMask mLayerMask = 0;
    [SerializeField] private string[] mSfx = null;

    [SerializeField] int mDamage = 0;

    private float mForce = 20f;

    public override bool CastSpell()
    {
        mEnemy.mAnimator.SetBool("Attacking", false);

        Collider[] colliders = Physics.OverlapSphere(mEnemy.transform.position, mEnemySettings.mRadius + 5, mLayerMask);

        for (int i = 0; i < colliders.Length; i++)
        {

            PlayerMovement receiver = colliders[i].GetComponent<PlayerMovement>();
            IAffectable affectable = colliders[i].GetComponent<IAffectable>();
            CharacterController controller = colliders[i].GetComponent<CharacterController>();

            if (controller != null)
            {
                mEnemy.mAnimator.SetTrigger("Hit");
                if (affectable != null)
                {
                    affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Physical);
                    
                    if (receiver != null)
                    {
                        Vector3 dir = receiver.transform.position - transform.position;
                        receiver.AddKnockback(dir, mForce);
                    }

                }
            }

        }
        AudioManager.PlayRandomSFX(mSfx);
        Instantiate(mSpellPrefab, mEnemy.transform.position, mEnemy.transform.rotation);
        mEnemy.mHealth.TakeDamage(200, AttackFlags.Player);
        return true;
    }
}