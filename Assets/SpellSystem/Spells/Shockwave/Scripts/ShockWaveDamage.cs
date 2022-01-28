using UnityEngine;


public class ShockWaveDamage : MonoBehaviour
{
    [SerializeField] private LayerMask mEnemies = 0;

    private ShockWaveData mSpellData;

    [SerializeField] private string[] mHitSFX = null;

    public void Initialize(ShockWaveData _data)
    {
        mSpellData = _data;
        CastSpell();
        AudioManager.PlayRandomSFX(mHitSFX);
    }

    #region Spell Logic
    /// <summary>
    /// A big blast of energy that deals a lot of damage
    /// </summary>
    private void CastSpell()
    {
        Collider[] mColliders = Physics.OverlapSphere(transform.position, mSpellData.BlastRadius, mEnemies);
        foreach (Collider collider in mColliders)
        {
            if (collider != null)
            {
                IAffectable affectable = collider.GetComponent<IAffectable>();
                if (affectable != null) {
                    affectable.TakeDamage(mSpellData.Damage, AttackFlags.Player | AttackFlags.Lightning);
                    if(mSpellData.CanStun)
                        affectable.Stun(3);
                }
            }
        }
    }
    #endregion

}