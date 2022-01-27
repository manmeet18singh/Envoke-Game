using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDyingAnimationHandler : MonoBehaviour
{
    [SerializeField] EnemyHealth mEnemyHealth = null;

    public void DyingAnimationEnded()
    {
        mEnemyHealth.Death();
    }
}
