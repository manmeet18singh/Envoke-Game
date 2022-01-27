#region HEADER
////////////////////////////////////////////////////////////////////////////////
/// Purpose: Fireblast (2,2) used to cast fireblast projectile in direction player
/// is facing
///
/// Author: Yashwant Patel
////////////////////////////////////////////////////////////////////////////////
#endregion

using UnityEngine;

public class FireBlastFactory : SpellFactoryBase
{

    public override bool CastSpell()
    {
        Instantiate(mSpellPrefab, GameManager.Instance.mSpellFirePoint.position, GameManager.Instance.mPlayer.transform.rotation).GetComponent<BaseProjectile>().Move();
        return true;
    }
}
