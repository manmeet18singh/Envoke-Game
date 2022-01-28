using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellFactoryBase : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    protected string mSpellName;
#endif

    [SerializeField]
    protected GameObject mSpellPrefab;
    [SerializeField]
    public Sprite SpellUISprite;

    protected RaycastHit mHit;
    protected bool mCanCast = true;

    public virtual bool CastSpell()
    {

#if UNITY_EDITOR
        Debug.Log($"CastSpell: {mSpellName}");
#endif
        return false;
    }

}
