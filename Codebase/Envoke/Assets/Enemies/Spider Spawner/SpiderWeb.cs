using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : BaseProjectile
{
    [SerializeField]
    private int mDamage = 0;
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
            affectable.TakeDamage(mDamage, AttackFlags.Enemy | AttackFlags.Ice);
        }
        Destroy(gameObject);
    }
}
