using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealUpgradeSO", menuName = "Envoke/Upgrades/Reg/HealUpgrade")]
public class HealUpgradeSO : BaseUpgradeSO
{
    [SerializeField]
    private int mPercentageToHeal = 0;
    [SerializeField]
    private IntEventChannelSO mUpgradeEvent = null;

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent(mPercentageToHeal);
    }
}
