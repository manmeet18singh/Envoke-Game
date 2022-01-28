using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HailSlowUpgrade", menuName = "Envoke/Upgrades/Path/HailSlowUpgrade")]
public class HailSlowPathUpgradeSO : BasePathUpgradeSO
{
    [Tooltip("How much time it takes till enemy's speed is zero")]
    [SerializeField]
    private int mTimeTillImmobile = 0;
    [SerializeField]
    private IntEventChannelSO mUpgradeEvent = null;

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent(mTimeTillImmobile);
    }
}
