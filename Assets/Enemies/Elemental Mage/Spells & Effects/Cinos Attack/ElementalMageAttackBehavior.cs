using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalMageAttackBehavior : EnemyAttackBehavior
{
    [Header("VFX")]
    [SerializeField] GameObject mRightHandEffect = null;
    [SerializeField] GameObject mLeftHandEffect = null;

    private void Awake()
    {
        mRightHandEffect.SetActive(false);
        mLeftHandEffect.SetActive(false);
    }

    public void StartCastingEffect()
    {
        mRightHandEffect.SetActive(true);
        mLeftHandEffect.SetActive(true);
    }

    public void StopCastingEffect()
    {
        mRightHandEffect.SetActive(false);
        mLeftHandEffect.SetActive(false);
    }
}
