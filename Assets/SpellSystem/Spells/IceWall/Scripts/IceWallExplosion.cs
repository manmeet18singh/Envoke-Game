using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallExplosion : MonoBehaviour
{
    public void Explode(IceWallData _spellData)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _spellData.Radius, _spellData.ExplodeMask);

        foreach(Collider collider in colliders)
        {
            IAffectable affectable = collider.GetComponent<IAffectable>();
            
            if(affectable != null)
            {
                affectable.TakeDamage(_spellData.ExplosionDamage, AttackFlags.Ice | AttackFlags.Player);
            }
        }
    }
}
