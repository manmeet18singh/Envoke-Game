using UnityEngine;

public class MinionMageBlastFactory : SpellFactoryBase
{
    [SerializeField] Transform mParentTransform = null;

    public override bool CastSpell()
    {
        Instantiate(mSpellPrefab, transform.position, mParentTransform.rotation).GetComponent<BaseProjectile>().Move();
        return true;
    }
}
