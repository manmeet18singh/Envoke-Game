using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI mRarityTMP;
    [SerializeField]
    protected TextMeshProUGUI mUpgradeNameTMP;
    [SerializeField]
    protected TextMeshProUGUI mDescriptionTMP;
    [SerializeField]
    protected Image mUpgradeImage;
    [SerializeField]
    protected RarityGradientSO mRarityGradients;

    protected BaseUpgradeSO mUpgrade = null;


    public virtual void SetInformation(BaseUpgradeSO _upgrade)
    {
        mUpgrade = _upgrade;
        mRarityTMP.text = _upgrade.Rarity.ToString();
        mRarityTMP.colorGradientPreset = mRarityGradients.mGradients[(int)_upgrade.Rarity];
        mUpgradeNameTMP.text = _upgrade.UpgradeName;
        mDescriptionTMP.text = _upgrade.UpgradeDesc;
        mUpgradeImage.sprite = _upgrade.UpgradeIcon;
    }

    public virtual void UpgradeChose()
    {
        mUpgrade.Upgrade();
        GameManager.Instance.UpgradeChose();
        UpgradeSystem.Instance.IterateOnPath(mUpgrade);
        GameManager.Instance.ToggleUpgradeMenu(null, false);
        GameManager.Instance.UnStopGame();
    }
}
