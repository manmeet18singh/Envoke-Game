using UnityEngine;

public class EdurMageBlastFactory : SpellFactoryBase
{
    [SerializeField] Transform mParentTransform = null;

    public override bool CastSpell()
    {
        Instantiate(mSpellPrefab, transform.position, mParentTransform.rotation).GetComponent<BaseProjectile>().Move();
        return true;
    }
}
