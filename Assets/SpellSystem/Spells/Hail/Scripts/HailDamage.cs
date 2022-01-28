using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailDamage : MonoBehaviour
{
    [SerializeField]
    private Collider mCollider;
    [SerializeField]
    private LayerMask mEnemies = 0;

    private HailData mSpellData;

    [SerializeField]
    private string[] mHitSFX = null;


    public void Initialize(HailData _data)
    {
        mSpellData = _data;
        StartCoroutine(ToggleTrigger());
    }


    IEnumerator ToggleTrigger()
    {
        float duration = mSpellData.SpellDuration;
        while (duration > 0)
        {
            --duration;

            yield return new WaitForSeconds(1f);
            Collider[] mColliders = Physics.OverlapSphere(transform.position, 5f, mEnemies);
            foreach (Collider collider in mColliders)
            {
                if (collider != null)
                {
                    IAffectable affectable = collider.GetComponent<IAffectable>();
                    if (affectable != null)
                        affectable.TakeDamage(mSpellData.DamagePerTick, AttackFlags.Player | AttackFlags.Ice);
                }
            }
            AudioManager.PlayRandomSFX(mHitSFX);

            yield return null;

#if UNITY_EDITOR
            Debug.Log("Toggled hail damage trigger");
#endif
        }

        Destroy(gameObject.transform.parent.gameObject);
    }
}
