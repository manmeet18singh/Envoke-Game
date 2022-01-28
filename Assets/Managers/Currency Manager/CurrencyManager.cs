using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]
    private int mCoinPurse = 0;
    [SerializeField]
    private TextMeshProUGUI mUICoinPurse = null;
    [SerializeField]
    private TextMeshProUGUI mUIUpgradeCoinPurse = null;
    [SerializeField]
    public GoldPickupData mGoldData;

    public static CurrencyManager Instance { get; private set; }

    public int CoinBalance{get => mCoinPurse; private set => mCoinPurse = value;}

    private void Awake()
    {
        Instance = this;

        if (SavePointSystem.SavedStats)
        {
            AddGold(SavePointSystem.Gold);
        }
    }

    private void Start()
    {
        if(mCoinPurse == 0)
        {
            mUICoinPurse.gameObject.SetActive(false);
        }
    }

    // Returns a bool to determine whether or not to drop gold based on the
    // drop rate set in the GoldData SO
    public bool DropGold()
    {
        bool drop = false;
        if(Random.Range(0, 100) <= mGoldData.DropRate)
        {
            drop = true;
        }
        return drop;
    }

    public void MakePurchase(int _cost)
    {
        CoinBalance -= _cost;
        mUICoinPurse.text = mUIUpgradeCoinPurse.text = $"{mCoinPurse}";
    }

    public void AddGold(int _gold)
    {
        if (!mUICoinPurse.gameObject.activeSelf)
            mUICoinPurse.gameObject.SetActive(true);
        CoinBalance += _gold;
        mUICoinPurse.text = mUIUpgradeCoinPurse.text = $"{mCoinPurse}";
        AudioManager.instance.Play("Collect Gold");

    }

    private void OnValidate()
    {
        if(Application.isPlaying)
            mUICoinPurse.text = mUIUpgradeCoinPurse.text = $"{mCoinPurse}";
    }
}
