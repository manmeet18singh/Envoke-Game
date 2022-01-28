using UnityEngine;

public class SoleisMageBlastFactory : SpellFactoryBase
{
    [SerializeField] Transform mParentTransform = null;

    public override bool CastSpell()
    {
        Instantiate(mSpellPrefab, transform.position, mParentTransform.rotation).GetComponentInChildren<BaseProjectile>().Move();
        return true;
    }
}
