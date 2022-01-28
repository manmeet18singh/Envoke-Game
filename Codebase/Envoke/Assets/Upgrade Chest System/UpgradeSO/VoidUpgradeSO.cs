using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidUpgrade", menuName = "Envoke/Upgrades/Reg/VoidUpgrade")]
public class VoidUpgradeSO : BaseUpgradeSO
{
    [SerializeField]
    private VoidEventChannelSO mUpgradeEvent =null;

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent();
    }
}
