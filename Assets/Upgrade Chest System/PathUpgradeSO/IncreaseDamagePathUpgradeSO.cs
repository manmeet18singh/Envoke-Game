using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDamageUpgrade", menuName = "Envoke/Upgrades/Path/IncreaseDamageUpgrade")]
public class IncreaseDamagePathUpgradeSO : BasePathUpgradeSO
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
