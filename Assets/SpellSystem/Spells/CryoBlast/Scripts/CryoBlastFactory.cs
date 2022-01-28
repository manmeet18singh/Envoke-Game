#region HEADER
////////////////////////////////////////////////////////////////////////////////
/// Purpose: Cryoblast (1,1) used to cast cryoblast projectile in the direction
/// player is facing
///
/// Author: Yashwant Patel
////////////////////////////////////////////////////////////////////////////////
#endregion
using UnityEngine;

public class CryoBlastFactory : SpellFactoryBase
{
    public override bool CastSpell()
    {
        Instantiate(mSpellPrefab, GameManager.Instance.mSpellFirePoint.position, GameManager.Instance.mPlayer.transform.rotation).GetComponent<BaseProjectile>().Move();
        return true;
    }
}
