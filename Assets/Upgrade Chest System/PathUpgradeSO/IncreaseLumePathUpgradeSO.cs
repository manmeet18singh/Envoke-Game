using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLumePathUp", menuName = "Envoke/Upgrades/Path/IncreaseLumePathUp")]
public class IncreaseLumePathUpgradeSO : BasePathUpgradeSO
{
    [SerializeField]
    private Lume mLumeType = 0;
    [SerializeField]
    private int mAmount = 0;
    [SerializeField]
    private DoubleIntEventChannelSO mUpgradeEvent = null;

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent((int)mLumeType, mAmount);
    }
}
