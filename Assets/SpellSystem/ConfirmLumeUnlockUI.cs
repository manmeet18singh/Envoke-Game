using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class ConfirmLumeUnlockUI : MonoBehaviour
{
    [SerializeField]
    GameObject mPrompt = null;
    [SerializeField]
    TextMeshProUGUI mPromptTMP = null;
    [SerializeField]
    GameObject mSelectedButton = null;

    private void Awake()
    {
        SpellEvents.Instance.mConfirmLumeUnlockCallback += DisplayPrompt;
    }

    private void OnDestroy()
    {
        SpellEvents.Instance.mConfirmLumeUnlockCallback -= DisplayPrompt;
    }

    void DisplayPrompt(int _lume)
    {
        ButtonSelectedManager.Instance.SetSelectedButton(mSelectedButton);
        mPromptTMP.text = $"Are you sure you would like to\nincrease your max {((Lume)_lume)} capacity?\n(Yes to confirm & No to repick)";
        GameManager.Instance.StopGame();

        mPrompt.SetActive(true);

    }

    public void LumeConfirmed()
    {
        mPrompt.SetActive(false);
        SpellEvents.Instance.ConfirmedLumeUnlock();
        GameManager.Instance.UnStopGame();
    }

    public void LumeRejected()
    {
        mPrompt.SetActive(false);
        GameManager.Instance.UnStopGame();
    }
}
