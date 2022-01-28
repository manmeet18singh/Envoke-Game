using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RushChargeFactory : SpellFactoryBase
{ 
    [SerializeField] Enemy mEnemy = null;
    [SerializeField] int mDamage = 0;
    [SerializeField] int mKnockbackForce = 0;
    [SerializeField] LayerMask mLayerMask = 0;
    [SerializeField] private string[] mSfx = null;

    public override bool CastSpell()
    {
        AudioManager.PlayRandomSFX(mSfx);

        Collider[] colliders = Physics.OverlapSphere(mEnemy.transform.position, 4, mLayerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Break();
            PlayerMovement receiver = colliders[i].GetComponent<PlayerMovement>();
            IAffectable affectable = colliders[i].GetComponent<IAffectable>();
            CharacterController controller = colliders[i].GetComponent<CharacterController>();

            if (controller != null)
            {
                mEnemy.mAnimator.SetBool("Attacking", false);

                if (affectable != null)
                {
                    affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Physical);

                    if (receiver != null)
                    {
                        mEnemy.mAnimator.SetTrigger("Hit");
                        Vector3 dir = receiver.transform.position - transform.position;
                        receiver.AddKnockback(dir, mKnockbackForce);
                        //mEnemy.mAnimator.ResetTrigger("Hit");
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
