using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject mUpgradeMenu = null;
    [SerializeField]
    GameObject mUpgradeButton = null;
    [SerializeField]
    GameObject[] mDisableObjs = null;

    UpgradeButton[] mUpgradeButtons;

    private void Start()
    {
        mUpgradeButtons = new UpgradeButton[UpgradeSystem.Instance.NumUpgradeSlots];

        for (int i = 0; i < UpgradeSystem.Instance.NumUpgradeSlots; ++i)
        {
            mUpgradeButtons[i] = Instantiate(mUpgradeButton, mUpgradeMenu.transform, false).GetComponent<UpgradeButton>();
        }

        GameManager.Instance.onToggleUpgradeMenu += ToggleUpgradeMenu;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onToggleUpgradeMenu -= ToggleUpgradeMenu;
    }

    public void ToggleUpgradeMenu(BaseUpgradeSO[] _upgrades, bool _active)
    {
        if (_active)
        {
            //ButtonSelectedManager.Instance.SetSelectedButton(mUpgradeButtons[0].gameObject);
            for (int i = 0; i < mUpgradeButtons.Length; ++i)
            {
                mUpgradeButtons[i].SetInformation(_upgrades[i]);
            }
        }

        for (int i = 0; i < mDisableObjs.Length; ++i)
        {
            mDisableObjs[i].SetActive(!_active);
        }

        mUpgradeMenu.SetActive(_active);
    }

}
