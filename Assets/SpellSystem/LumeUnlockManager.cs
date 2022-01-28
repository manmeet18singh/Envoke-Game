using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumeUnlockManager : MonoBehaviour
{
    public static LumeUnlockManager Instance;

    [SerializeField]
    int mNumLumeToGive = 2;
    [SerializeField]
    Vector3 mPaddingSpace = Vector3.zero;
    #region NamedArrayAttributes
#if UNITY_EDITOR
    [NamedArray(new string[] { "Edur", "Cinos", "Soleis" })]
#endif
    #endregion
    [SerializeField]
    GameObject[] mUnlockTriggers = null;
    #region NamedArrayAttributes
#if UNITY_EDITOR
    [NamedArray(new string[]
{
        "(E,E)",
        "LEAVE EMPTY",
        "(C,E)",
        "(C,C)",
        "(E,S)",
        "(C,S)",
        "(S,S)"
})]
#endif
    #endregion
    [SerializeField]
    UpgradePathSO[] mUpgradePaths = null;
    [SerializeField]
    BaseUpgradeSO[] mRegularEdurUpgrades = null;
    [SerializeField]
    BaseUpgradeSO[] mRegularCinosUpgrades = null;
    [SerializeField]
    BaseUpgradeSO[] mRegularSoleisUpgrades = null;
    [SerializeField]
    BaseUpgradeSO[] mAnyUpgrades = null;

    [SerializeField]
    Collider mConfirmTrigger = null;

    BaseUpgradeSO[][] mRegularUpgrades = new BaseUpgradeSO[3][];

    public int LumeChose { get; private set; } = 0;

    Vector3[] mTriggerPositions = new Vector3[LumeInventory.NumLumeTypes];

    bool[] mUnlockedLumes;

    private void Awake()
    {
        Instance = this;
        SpellEvents.Instance.mConfirmedLumeUnlockCallback += LumePickConfirmed;
        for (int i = 0; i < mTriggerPositions.Length; ++i)
        {
            mTriggerPositions[i] = new Vector3(transform.position.x + (mPaddingSpace.x * (i - 1)), transform.position.y,
                                               transform.position.z + (mPaddingSpace.z * (i - 1)));
        }

        for(int i = 0; i < mUpgradePaths.Length; ++i)
        {
            if(mUpgradePaths[i] != null)
                mUpgradePaths[i].InitializePath();
        }


        mRegularUpgrades[0] = mRegularEdurUpgrades;
        mRegularUpgrades[1] = mRegularCinosUpgrades;
        mRegularUpgrades[2] = mRegularSoleisUpgrades;
    }

    private void OnEnable()
    {
        mConfirmTrigger.isTrigger = false;
        ActivateLumeUnlockTriggers();  
    }

    private void OnDestroy()
    {
        SpellEvents.Instance.mConfirmedLumeUnlockCallback -= LumePickConfirmed;
    }

    void ActivateLumeUnlockTriggers()
    {

        mUnlockedLumes = LumeInventory.GetUnlockedLumes();

        int j = 0;

        for (int i = 0; i < LumeInventory.NumLumeTypes; ++i)
        {
            if (!mUnlockedLumes[i])
            {
                mUnlockTriggers[i].transform.parent.transform.position = mTriggerPositions[j++];
                mUnlockTriggers[i].transform.parent.gameObject.SetActive(true);
            }
        }

        if(j == 0)
            gameObject.SetActive(false);

    }

    public void LumePicked(int _lume)
    {
        if (!mUnlockedLumes[LumeChose])
        {
            //TODO: Fix lume queue not clearing
            LumeInventory.LockLume(LumeChose);
            mUnlockTriggers[LumeChose].SetActive(true);
            SpellEvents.Instance.LockedLume(LumeChose);
        }

        LumeInventory.IncreaseMaxCapacity(_lume, mNumLumeToGive);
        LumeInventory.UnSafeAddLume(_lume, mNumLumeToGive);
        mUnlockTriggers[_lume].SetActive(false);
        SpellEvents.Instance.UnlockedLume(_lume);

        LumeChose = _lume;
        mConfirmTrigger.isTrigger = true;
    }

    //TODO: Reset lume amounts to default values
    void LumePickConfirmed()
    {
        LumeInventory.SetLumeAmounts(LumeChose, 0, 0);
        SavePointSystem.CurrentLumes = LumeInventory.GetCurrentLumes();
        SavePointSystem.MaxLumes = LumeInventory.GetMaxLumes();
        SavePointSystem.UnlockedLumes = LumeInventory.GetLumesUnlocked();
        SavePointSystem.NumLumesUnlocked = LumeInventory.NumLumesUnlocked;
        LumeInventory.SetLumeAmounts(LumeChose, mNumLumeToGive, mNumLumeToGive);

        LumeInventory.UnlockLume(LumeChose);
        int[] lumesUnlocked = LumeInventory.GetLumesUnlocked();


        int pathIndex = LumeChose * 3;
#if UNITY_EDITOR

        for (int i = 0; i < LumeInventory.NumLumesUnlocked; ++i)
        {
            if (mUpgradePaths[pathIndex] != null)
                UpgradeSystem.Instance.AddPathUpgrade(mUpgradePaths[pathIndex].Upgrades[0]);
            pathIndex = lumesUnlocked[i] + LumeChose + (lumesUnlocked[i] >= LumeChose ? lumesUnlocked[i] : LumeChose);
        }
#else

        for (int i = 0; i < LumeInventory.NumLumesUnlocked; ++i)
        {
            UpgradeSystem.Instance.AddPathUpgrade(mUpgradePaths[pathIndex].Upgrades[0]);
            pathIndex = lumesUnlocked[i] + LumeChose + (lumesUnlocked[i] >= LumeChose ? lumesUnlocked[i] : LumeChose);
        }
#endif

        for(int i = 0; i < mRegularUpgrades[LumeChose].Length; ++i)
        {
            UpgradeSystem.Instance.AddRegularUpgrade(mRegularUpgrades[LumeChose][i]);
        }

        if (mAnyUpgrades != null)
        {
            for(int i = 0; i < mAnyUpgrades.Length; ++i)
            {
                UpgradeSystem.Instance.AddRegularUpgrade(mAnyUpgrades[i]);
            }

            mAnyUpgrades = null;

        }

        gameObject.SetActive(false);

    }
}
