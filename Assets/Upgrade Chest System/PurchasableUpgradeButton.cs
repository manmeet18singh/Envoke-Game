using TMPro;
using UnityEngine;

public class PurchasableUpgradeButton : UpgradeButton
{
    [SerializeField]
    TextMeshProUGUI mCostTMP = null;

    public override void SetInformation(BaseUpgradeSO _upgrade)
    {
        base.SetInformation(_upgrade);
        mCostTMP.text = $"Cost {_upgrade.Cost}";
    }

    public override void UpgradeChose()
    {
        if(CurrencyManager.Instance.CoinBalance >= mUpgrade.Cost)
        {
            mUpgrade.Upgrade();
            CurrencyManager.Instance.MakePurchase(mUpgrade.Cost);
            UpgradeSystem.Instance.IterateOnPath(mUpgrade);
            GameManager.Instance.UpgradeChose();
            //GameManager.Instance.TogglePurchUpgradeMenu(null, false);
        }
        else
        {
            NotificationManager.Instance.AddNotification("Not enough coins!", .45f);
        }
    }
}
