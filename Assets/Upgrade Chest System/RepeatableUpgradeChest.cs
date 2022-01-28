using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepeatableUpgradeChest : BasicUpgradeChest
{
    #region Attributes
#if UNITY_EDITOR
    [NamedArray(new string[] { "Common", "Rare", "Legendary" })]
#endif
    [Tooltip("Probability for rarity buckets")]
    [SerializeField]
    #endregion
    int[] mRegRarityLootTable = null;

    #region Attributes
#if UNITY_EDITOR
    [NamedArray(new string[] { "Common", "Rare", "Legendary" })]
#endif
    [Tooltip("Probability for rarity buckets")]
    [SerializeField]
    #endregion
    int[] mPathRarityLootTable = null;
    [SerializeField]
    int mNumRegularSlots = 0;

    bool mMenuOpen = false;
    bool mUpgradesRolled = false;
    private BaseUpgradeSO[] mRolledUprades = null;
    static Vector3 mOffset = new Vector3(0f, 3f, 0f);


    private void OnDestroy()
    {
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
            GameManager.Instance.onUpgradeChose -= UpgradeChose;
            InputManager.controls.UI.Interact.performed -= ToggleMenu;
        }
    }


    protected override void DisplayUpgradeMenu()
    {
        Time.timeScale = 0;
        GameManager.Instance.EscapeButtonPressed = DisableUpgradeMenu;
        GameManager.Instance.PausePlayerInput();
        mMenuOpen = true;
        if (!mUpgradesRolled)
        {
            mRolledUprades = UpgradeSystem.Instance.RollUpgrades(mNumRegularSlots, mRegRarityLootTable, mPathRarityLootTable);
            GameManager.Instance.TogglePurchUpgradeMenu(mRolledUprades, true);
            mUpgradesRolled = true;
            return;
        }

        Debug.Log("Called on: " + gameObject.name);
        GameManager.Instance.TogglePurchUpgradeMenu(mRolledUprades, true);

    }

    private void ToggleMenu(InputAction.CallbackContext _ctx)
    {
        if (!mMenuOpen)
        {
            //Cursor.visible = false;
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
        mRolledUprades = UpgradeSystem.Instance.RollUpgrades(mNumRegularSlots, mRegRarityLootTable, mPathRarityLootTable);
        GameManager.Instance.TogglePurchUpgradeMenu(mRolledUprades, true);
    }
}
