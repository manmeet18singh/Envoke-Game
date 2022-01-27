using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAnimationEventManager : MonoBehaviour
{
    #region References
    [SerializeField] private CastIceTorrent mCastIceTorrent = null;
    [SerializeField] private CastFlameWreath mCastFlameWreath = null;
    [SerializeField] private CastSummonPortals mCastSummonPortals = null;
    [SerializeField] private CastBossLaserSpell mCastBossLaserSpell = null;
    [SerializeField] private CastSummonPortals mCastFrenzySummonPortals = null;
    [SerializeField] private FinalBoss mBoss = null;
    #endregion

    public void CastIceTorrent()
    {
        mCastIceTorrent.SetSpellVariation(Random.Range(0, mCastIceTorrent.NumberOfVariations));
        mCastIceTorrent.CastSpell();
    }

    public void CastSummonPortals()
    {
        mCastSummonPortals.SetSpellVariation(Random.Range(0, mCastSummonPortals.NumberOfVariations));
        mCastSummonPortals.CastSpell();
        mBoss.DoneCastingFrenzyPortals();
    }

    public void CastFlameWreath()
    {
        mCastFlameWreath.SetSpellVariation(Random.Range(0, mCastFlameWreath.NumberOfVariations));
        mCastFlameWreath.CastSpell();
    }

    public void CastLaserSpell()
    {
        mCastBossLaserSpell.SetSpellVariation(Random.Range(0, mCastBossLaserSpell.NumberOfVariations));
        mCastBossLaserSpell.CastSpell();
    }

    public void CastFrenzySummonPortals()
    {
        mCastFrenzySummonPortals.SetSpellVariation(Random.Range(0, mCastFrenzySummonPortals.NumberOfVariations));
        mCastFrenzySummonPortals.CastSpell();
        mBoss.DoneCastingFrenzyPortals();
    }

    public void CastTeleportSummonPortals()
    {
        mBoss.CastTeleportAndSummon();
    }

    public void FinishSlashAttack()
    {
        mBoss.FinishedSlashing();
    }

    public void FinishStageTransition()
    {
        mBoss.FinishedStageTransition();
    }

    public void SummonSword()
    {
        mBoss.SummonSword();
    }

    public void DoneDying()
    {
        mBoss.mHealth.Death();
    }

}
