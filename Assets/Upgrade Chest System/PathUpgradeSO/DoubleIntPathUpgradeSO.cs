using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleIntPathUpgrade", menuName = "Envoke/Upgrades/Path/DoubleIntPathUp")]
public class DoubleIntPathUpgradeSO : BasePathUpgradeSO
{
    [SerializeField]
    private int mAmt1 = 0;
    [SerializeField]
    private int mAmt2 = 0;
    [SerializeField]
    private DoubleIntEventChannelSO mUpgradeEvent = null;

#if UNITY_EDITOR
    [TextArea][SerializeField] string VarDescription;
#endif

    public override void Upgrade()
    {
        mUpgradeEvent.RaiseEvent((int)mAmt1, mAmt2);
    }
}
