using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionMageBlastProjectile : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    protected string mSpellName;
#endif

    [SerializeField]
    protected GameObject mSpellPrefab;

    public virtual void Attack()
    {
#if UNITY_EDITOR
        Debug.Log($"CastSpell: {mSpellName}");
#endif
    }
}
