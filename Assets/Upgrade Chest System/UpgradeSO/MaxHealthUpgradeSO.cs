using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxHealthIncreaseUpgrade", menuName = "Envoke/Upgrades/Reg/MaxHPIncreaseUpgrade")]
public class MaxHealthUpgradeSO : BaseUpgradeSO
{
    [Tooltip("How much to add to max player health")]
    [SerializeField]
    private int mMaxHealthIncreaseAmount = 0;
    [SerializeField]
    private IntEventChannelSO mUpgradeEvent = null;

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent(mMaxHealthIncreaseAmount);
    }
}
