using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkDamage : MonoBehaviour
{
    [SerializeField]
    private LayerMask mEnemies = 0;

    private BlinkData mSpellData;

    [SerializeField]
    private string[] mHitSFX = null;

    public void Initialize(BlinkData _data)
    {
        mSpellData = _data;
        StartCoroutine(CauseDamage());
    }

    IEnumerator CauseDamage()
    {
        float duration = mSpellData.SpellDuration;
        while (duration > 0)
        {
            --duration;

            yield return new WaitForSeconds(1f);
            Collider[] colliders = Physics.OverlapSphere(transform.position, mSpellData.DamageRadius, mEnemies);
            foreach (Collider collider in colliders)
            {
                collider.GetComponent<IAffectable>().TakeDamage(mSpellData.DamagePerTick, AttackFlags.Player | AttackFlags.Arcane);
            }
            AudioManager.PlayRandomSFX(mHitSFX);

            yield return null;

#if UNITY_EDITOR
            Debug.Log("Blink caused Damage");
#endif
        }

        Destroy(gameObject);
    }
}
