using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlastProjectile : BaseProjectile
{
    [SerializeField]
    int mDamage = 20;

    private void OnTriggerEnter(Collider other)
    {

#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} collided with: {other.name}");
#endif

        IAffectable affectable = other.GetComponent<IAffectable>();

        if (affectable != null)
        {
            affectable.TakeDamage(mDamage, AttackFlags.Player | AttackFlags.Fire);
        }

        Destroy(gameObject);
    }
}
