using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAOEUpgrade", menuName = "Envoke/Upgrades/Path/Increase AOE Upgrade")]
public class IncreaseAOEUpgradeSO : BasePathUpgradeSO
{
    [SerializeField]
    private int mAmount = 0;
    [SerializeField]
    private IntEventChannelSO mUpgradeEvent = null;

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent(mAmount);
    }
}

