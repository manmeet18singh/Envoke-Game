using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpgradeChest : MonoBehaviour
{
    [SerializeField]
    Collider mTrigger = null;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisplayUpgradeMenu();
            mTrigger.enabled = false;
            GameManager.Instance.onUpgradeChose += UpgradeChose;
        }
    }

    protected virtual void DisplayUpgradeMenu()
    {
        GameManager.Instance.ToggleUpgradeMenu(UpgradeSystem.Instance.RollUpgrades(), true);
        GameManager.Instance.StopGame();
    }

    protected virtual void UpgradeChose()
    {
        InteractPopupManager.Instance.HidePopup();
        GameManager.Instance.onUpgradeChose -= UpgradeChose;
        if (gameObject)
            Destroy(gameObject);
    }
}
