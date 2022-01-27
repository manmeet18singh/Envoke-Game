using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoBlastProjectile : BaseProjectile
{
    [SerializeField]
    private int mDamage = 5;
    private float mDuration = 5;

    private void OnTriggerEnter(Collider other)
    {

#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} collided with: {other.name}");
#endif

        IAffectable affectable = other.GetComponent<IAffectable>();

        if (affectable != null)
        {
            affectable.Freeze(mDuration);
            affectable.TakeDamage(mDamage, AttackFlags.Player | AttackFlags.Ice);
        }

        Destroy(gameObject);
    }

}
