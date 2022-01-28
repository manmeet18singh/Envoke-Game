using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath : MonoBehaviour
{
    FireBreathData mSpellData;
    [SerializeField] string[] mCastSFX = null;

    public void Initialize(ref FireBreathData _spellData)
     {
        mSpellData = _spellData;
        StartCoroutine(WaitForDeath());
        AudioManager.PlayRandomSFX(mCastSFX);
     }

    private void OnDestroy()
    {
        //InputManager.controls.Player.Enable();
        //GameManager.Instance.ResumePlayerInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        IAffectable affectable = other.GetComponent<IAffectable>();
        if (affectable != null)
        {
            affectable.TakeDamage(mSpellData.InitialDamage, AttackFlags.Player | AttackFlags.Fire);
            if(mSpellData.CanDOT)
                affectable.DamageOverTime(mSpellData.DamagePerTick, mSpellData.Duration, AttackFlags.Player | AttackFlags.Fire);
        }

    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(mSpellData.LifeTime);
        Destroy(gameObject);
    }
}
