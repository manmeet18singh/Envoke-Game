using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLume", menuName = "Envoke/Upgrades/Reg/IncreaseLume")]
public class IncreaseLumeUpgradeSO : BaseUpgradeSO
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
