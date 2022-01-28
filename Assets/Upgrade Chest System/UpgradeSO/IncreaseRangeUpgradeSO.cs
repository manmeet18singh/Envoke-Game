using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseRangeUpgrade", menuName = "Envoke/Upgrades/Path/Increase Range Upgrade")]
public class IncreaseRangeUpgradeSO : BasePathUpgradeSO
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

