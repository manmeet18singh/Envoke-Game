using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PurchasableUpgradeChest : BasicUpgradeChest
{
    bool mUpgradesRolled = false;
    bool mMenuOpen = false;
    static Vector3 mOffset = new Vector3(0f, 3f, 0f);

    private void OnDestroy()
    {
        GameManager.Instance.onUpgradeChose -= UpgradeChose;
        InputManager.controls.UI.Interact.performed -= ToggleMenu;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //need to make esc turn on menu
            GameManager.Instance.onUpgradeChose += UpgradeChose;
            InputManager.controls.UI.Interact.performed += ToggleMenu;
            InteractPopupManager.Instance.DisplayPopup(transform.position + mOffset);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractPopupManager.Instance.HidePopup();
            InputManager.controls.UI.Interact.performed -= ToggleMenu;
        }
    }

    protected override void DisplayUpgradeMenu()
    {
        Time.timeScale = 0;
        GameManager.Instance.EscapeButtonPressed = DisableUpgradeMenu;
        GameManager.Instance.PausePlayerInput();
        if (!mUpgradesRolled)
        {
            GameManager.Instance.TogglePurchUpgradeMenu(UpgradeSystem.Instance.RollUpgrades(), true);
            mUpgradesRolled = true;
            return;
        }

        GameManager.Instance.TogglePurchUpgradeMenu(null, true);
        mMenuOpen = true;
    }

    private void ToggleMenu(InputAction.CallbackContext _ctx)
    {
        if (!mMenuOpen)
        {
            DisplayUpgradeMenu();
        }
        else
        {
            DisableUpgradeMenu();
        }

        //mMenuOpen = !mMenuOpen;
    }

    private void DisableUpgradeMenu()
    {
        GameManager.Instance.EscapeButtonPressed = GameManager.Instance.TogglePause;
        Time.timeScale = 1;
        GameManager.Instance.TogglePurchUpgradeMenu(null, false);
        GameManager.Instance.ResumePlayerInput();
        mMenuOpen = false;
    }

    protected override void UpgradeChose()
    {
        DisableUpgradeMenu();
        InteractPopupManager.Instance.HidePopup();
        GameManager.Instance.onUpgradeChose -= UpgradeChose;
        if (gameObject)
            Destroy(gameObject);
    }
}
