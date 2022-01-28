using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLumes", menuName = "Envoke/Upgrades/Reg/IncreaseLumes")]
public class IncreaseLumesUpgradeSO : BaseUpgradeSO
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
