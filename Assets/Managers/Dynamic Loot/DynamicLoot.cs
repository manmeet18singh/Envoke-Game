using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLoot : MonoBehaviour
{
    [SerializeField]
    private List<PickupData> mSpawnablePickups = null;

    [SerializeField]
    //Scriptable object loot table containing PickupData and weight
    //Health needs to stay at index three [3]
    private LootTable mLootTable = null;

    [SerializeField]
    private int[] mDynamicWeights = new int[4];

    private LootTable.Drop[] mTable;

    int totalWeight;
    public int TotalWeight
    {
        get
        {
            CalculateTotalWeight();
            return totalWeight;
        }
    }

    public static DynamicLoot Instance { get; private set; }

    public List<PickupData> GetSpawnablePickups { get => mSpawnablePickups; }

    private void Awake()
    {
        Instance = this;
        SpellEvents.Instance.mConfirmedLumeUnlockCallback += AddLumeToSpawnable;

        LumeInventory.mLumeAdded += RemoveWeight;
        LumeInventory.mLumeRemoved += AddWeight;

        PlayerHealth.mDamage += AddWeightToHealth;
        PlayerHealth.mHeal += RemoveWeightFromHealth;
    }


    private void Start()
    {
        mTable = mLootTable.table;
        // Loot table must be in this order > Edur, Cinos, Soleis, Health
        // Resets dynamic weight to base weight
        for (int i = 0; i < mDynamicWeights.Length; i++)
        {
            mDynamicWeights[i] = mTable[i].mWeight;
        }

        if (SavePointSystem.SavedStats)
        {
            int[] unlockedLumes = SavePointSystem.UnlockedLumes;
            for (int i = 0; i < SavePointSystem.NumLumesUnlocked; i++)
            {
                mSpawnablePickups.Add(mTable[unlockedLumes[i]].mDrop);
            }
        }
    }

    // once player has clicked "yes" and confirmed it is added to Spawnable Pickups
    private void AddLumeToSpawnable()
    {
        mSpawnablePickups.Add(mTable[LumeUnlockManager.Instance.LumeChose].mDrop);
    }

    // calculates the total weight of the drops in the loot table
    private void CalculateTotalWeight()
    {
        totalWeight = 0;
        for (int i = 0; i < mDynamicWeights.Length; i++)
        {
            totalWeight += mDynamicWeights[i];
        }
    }

    // gets a random drop based on weight
    public PickupData GetDrop()
    {
        int roll = UnityEngine.Random.Range(0, TotalWeight);

        for (int i = 0; i < mTable.Length; i++)
        {
            roll -= mTable[i].mWeight;

            if (roll < 0)
            {
                return mTable[i].mDrop;
            }
        }

        return mTable[0].mDrop;
    }

    public void AddWeight(int _lumeIndex, int _weight = 1)
    {
        mDynamicWeights[_lumeIndex] += _weight * 70;
    }

    public void RemoveWeight(int _lumeIndex, int _weight = 1)
    {
        mDynamicWeights[_lumeIndex] -= _weight * 70;
        if (mDynamicWeights[_lumeIndex] < 0)
            mDynamicWeights[_lumeIndex] = mTable[_lumeIndex].mWeight;
    }
    private void AddWeightToHealth(int _weight)
    {
        mDynamicWeights[3] += _weight;
    }

    private void RemoveWeightFromHealth(int _weight)
    {
        mDynamicWeights[3] -= _weight;
        if (mDynamicWeights[3] < 0)
            mDynamicWeights[3] = mTable[3].mWeight;
    }

    private void OnDestroy()
    {
        SpellEvents.Instance.mConfirmedLumeUnlockCallback -= AddLumeToSpawnable;

        LumeInventory.mLumeAdded -= AddWeight;
        LumeInventory.mLumeRemoved -= RemoveWeight;
        PlayerHealth.mDamage -= AddWeightToHealth;
        PlayerHealth.mHeal -= RemoveWeightFromHealth;
    }

}
