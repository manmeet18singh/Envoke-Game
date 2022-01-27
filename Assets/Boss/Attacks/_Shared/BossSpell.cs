using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSpell : MonoBehaviour
{
    public FinalBoss mBoss;
    protected bool mIsDoneCasting = false;
    public bool IsDoneCasting { get => mIsDoneCasting; }

    public abstract void CastSpell();

    public abstract void CancelCast();

    public abstract void SetSpellVariation(int _index);

    public virtual void ResetSpell()
    {
        mIsDoneCasting = false;
    }

}
