using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehavior : MonoBehaviour
{
    [SerializeField] private SpellFactoryBase mEnemyAttack = null;
    [SerializeField] private float mTimeBetweenAttacks = 0f;

    // State variables
    private bool mAlreadyAttacked;

    public void Attack()
    {
        if (!mAlreadyAttacked)
        {
            mEnemyAttack.CastSpell();
            mAlreadyAttacked = true;
            Invoke(nameof(ResetAttack), mTimeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        mAlreadyAttacked = false;
    }
}
