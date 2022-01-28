using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidPathUpgrade", menuName = "Envoke/Upgrades/Path/VoidPathUpgrade")]
public class VoidPathUpgradeSO : BasePathUpgradeSO
{
    [SerializeField]
    VoidEventChannelSO mUpgradeEvent = null;

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent();
    }
}
