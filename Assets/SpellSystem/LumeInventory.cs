using System;
using UnityEngine;

public static class LumeInventory
{
    public const byte NumLumeTypes = 3;
    public static int NumLumesUnlocked { get; set; }

    private const int NumStartingLumes = 0;
    private static bool[] mUnlockedLumes = new bool[NumLumeTypes];
    private static int[] mLumesUnlocked = new int[NumLumeTypes];
    private static int[] mCurrentLumes = new int[NumLumeTypes];
    private static int[] mMaxLumes = new int[NumLumeTypes];
    private static bool[] emptyLumes = new bool[NumLumeTypes];
    private static byte[] mNumLumeTypeQueued;

    public static event Action<int, int> mLumeAdded;
    public static event Action<int, int> mLumeRemoved;

    static LumeInventory()
    {
        ResetInventory();
    }

    public static void ResetInventory()
    {
        for (int i = 0; i < mCurrentLumes.Length; ++i)
        {
            mCurrentLumes[i] = mMaxLumes[i] = NumStartingLumes;
            NumLumesUnlocked = NumStartingLumes;
            mUnlockedLumes[i] = false;
        }
    }

    public static void ReinitializeInventory()
    {
        for(int i = 0; i < NumLumesUnlocked; ++i)
        {
            
        }
    }

    public static void SetNumLumeTypeQueued(byte[] _mNumLumeTypeQueued)
    {
        mNumLumeTypeQueued = _mNumLumeTypeQueued;
    }

    public static bool TryAddLume(int _lumeIndex, int _amount = 1)
    {
        if (IsLumeTypeFull(_lumeIndex))
            return false;
        else if (mCurrentLumes[_lumeIndex] == 0)
            SpellEvents.Instance.LumeRegenStopped(_lumeIndex);


        mCurrentLumes[_lumeIndex] += _amount;

        if (mCurrentLumes[_lumeIndex] > mMaxLumes[_lumeIndex])
            mCurrentLumes[_lumeIndex] = mMaxLumes[_lumeIndex];

        mLumeAdded?.Invoke(_lumeIndex, 1);
        SpellEvents.Instance.LumeAmountChanged(_lumeIndex, mCurrentLumes[_lumeIndex],
                                               mMaxLumes[_lumeIndex]);

        return true;
    }

    public static void UnSafeAddLume(int _lumeIndex, int _amount = 1)
    {
        mCurrentLumes[_lumeIndex] += _amount;

        mLumeAdded?.Invoke(_lumeIndex, _amount);
        SpellEvents.Instance.LumeAmountChanged(_lumeIndex, mCurrentLumes[_lumeIndex],
                                               mMaxLumes[_lumeIndex]);
    }


    public static bool TryRemoveLume(int _lumeIndex, int _amount = 1)
    {
        if (!IsLumeNotEmpty(_lumeIndex))
            return false;

        mCurrentLumes[_lumeIndex] -= _amount;

        if (mCurrentLumes[_lumeIndex] < 1)
            mCurrentLumes[_lumeIndex] = 0;

        mLumeRemoved?.Invoke(_lumeIndex, 1);
        SpellEvents.Instance.LumeAmountChanged(_lumeIndex, mCurrentLumes[_lumeIndex],
                                       mMaxLumes[_lumeIndex]);

        return true;
    }

    public static void UnSafeRemoveLume(int _lumeIndex)
    {
        --mCurrentLumes[_lumeIndex];

        mLumeRemoved?.Invoke(_lumeIndex, 1);
        SpellEvents.Instance.LumeAmountChanged(_lumeIndex, mCurrentLumes[_lumeIndex],
                                               mMaxLumes[_lumeIndex]);
    }

    public static void SetLumeAmounts(int _lumeIndex, int _currentAmount, int _maxAmount)
    {
        mCurrentLumes[_lumeIndex] = _currentAmount;
        mMaxLumes[_lumeIndex] = _maxAmount;
        SpellEvents.Instance.LumeAmountChanged(_lumeIndex, _currentAmount, _maxAmount);
    }

    public static void IncreaseMaxCapacity(int _lumeIndex, int _amount = 1)
    {
        mMaxLumes[_lumeIndex] += _amount;

        mLumeRemoved?.Invoke(_lumeIndex, _amount);
        SpellEvents.Instance.LumeAmountChanged(_lumeIndex, mCurrentLumes[_lumeIndex],
                                       mMaxLumes[_lumeIndex]);
    }

    public static bool IsLumeTypeFull(int _lumeIndex)
    {
        return mCurrentLumes[_lumeIndex] + mNumLumeTypeQueued[_lumeIndex] == mMaxLumes[_lumeIndex];
    }

    public static bool IsInventoryFull(int _lumeIndex)
    {
        return mCurrentLumes[_lumeIndex] == mMaxLumes[_lumeIndex];
    }

    public static bool IsLumeNotEmpty(int _lumeIndex)
    {
        return mCurrentLumes[_lumeIndex] > 0;
    }

    public static int GetCurrentLumeAmount(int _lumeIndex)
    {
        return mCurrentLumes[(int)_lumeIndex];
    }

    public static int[] GetCurrentLumes()
    {
        return (int[])mCurrentLumes.Clone();
    }

    public static int GetMaxLumeAmount(int _lumeIndex)
    {
        return mMaxLumes[(int)_lumeIndex];
    }

    public static int[] GetMaxLumes()
    {
        return (int[])mMaxLumes.Clone();
    }

    public static bool[] GetEmptyLumeTypes()
    {
        for (int i = 0; i < emptyLumes.Length; ++i)
        {
            emptyLumes[i] = mCurrentLumes[i] == 0;
        }

        return emptyLumes;
    }

    public static bool[] GetUnlockedLumes()
    {
        return (bool[])mUnlockedLumes.Clone();
    }
    
    public static int[] GetLumesUnlocked()
    {
        return mLumesUnlocked;
    }

    public static void UnlockLume(int _lumeIndex)
    {
        mUnlockedLumes[_lumeIndex] = true;
        mLumesUnlocked[NumLumesUnlocked] = _lumeIndex;
        ++NumLumesUnlocked;
    }

    public static void LockLume(int _lumeIndex)
    {
        mCurrentLumes[_lumeIndex] = mMaxLumes[_lumeIndex] = 0;
    }

}
