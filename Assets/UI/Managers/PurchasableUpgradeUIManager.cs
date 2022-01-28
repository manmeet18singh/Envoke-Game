using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchasableUpgradeUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject mUpgradeMenu = null;
    [SerializeField]
    GameObject mUpgradeButton = null;
    [SerializeField]
    GameObject[] mDisableObjs = null;
    [SerializeField]
    GameObject mGoldUI = null;

    PurchasableUpgradeButton[] mUpgradeButtons;

    private void Start()
    {
        mUpgradeButtons = new PurchasableUpgradeButton[UpgradeSystem.Instance.NumUpgradeSlots];

        for (int i = 0; i < UpgradeSystem.Instance.NumUpgradeSlots; ++i)
        {
            mUpgradeButtons[i] = Instantiate(mUpgradeButton, mUpgradeMenu.transform, false).GetComponent<PurchasableUpgradeButton>();
        }

        GameManager.Instance.onTogglePurchUpgradeMenu += ToggleUpgradeMenu;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onTogglePurchUpgradeMenu -= ToggleUpgradeMenu;
    }

    public void ToggleUpgradeMenu(BaseUpgradeSO[] _upgrades, bool _active)
    {
        if (_upgrades != null)
        {
            //ButtonSelectedManager.Instance.SetSelectedButton(mUpgradeButtons[0].gameObject);
            for (int i = 0; i < mUpgradeButtons.Length; ++i)
            {
                mUpgradeButtons[i].SetInformation(_upgrades[i]);
            }
        }

        for(int i = 0; i < mDisableObjs.Length; ++i)
        {
            mDisableObjs[i].SetActive(!_active);
        }

        mUpgradeMenu.SetActive(_active);
        mGoldUI.SetActive(_active);
    }

}
