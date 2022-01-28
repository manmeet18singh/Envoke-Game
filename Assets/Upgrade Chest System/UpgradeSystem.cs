using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeSystem : MonoBehaviour
{

    public static UpgradeSystem Instance { get; private set; }

    [SerializeField]
    UpgradePathSO[] mUpgradePathLists = null;
    [SerializeField]
    RegularUpgradeList mRegularUpgradesSO = null;
    [SerializeField]
    int mNumRegularUpgradeSlots = 1;
    [field: SerializeField]
    public int NumUpgradeSlots { get; private set; }
    #region Attributes
#if UNITY_EDITOR
    [NamedArray(new string[] { "Common", "Rare", "Legendary" })]
#endif
    [Tooltip("Probability for rarity buckets")]
    [SerializeField]
    #endregion
    int[] mRarityBucketWeights = null;

    const int UpgradeRarityAmount = (int)UpgradeRarity.MAX;
    List<BasePathUpgradeSO>[] mPathUpgradeRarityBuckets = new List<BasePathUpgradeSO>[UpgradeRarityAmount];
    List<int>[] mPathUpgradeLootTables = new List<int>[UpgradeRarityAmount];
    int[] mPathUpgradeTotalWeights = new int[UpgradeRarityAmount];
    List<(UpgradeRarity, int)> mPathRarityWeights = new List<(UpgradeRarity, int)>();

    List<BaseUpgradeSO>[] mRegularUpgradeRarityBuckets = new List<BaseUpgradeSO>[UpgradeRarityAmount];
    List<int>[] mRegularUpgradeLootTables = new List<int>[UpgradeRarityAmount];
    int[] mRegularUpgradeTotalWeights = new int[UpgradeRarityAmount];
    List<(UpgradeRarity, int)> mRegRarityWeights = new List<(UpgradeRarity, int)>();
    List<BasePathUpgradeSO> mUnlockedUpgrades = new List<BasePathUpgradeSO>();

    //int mTotalRarityWeight;
    int mCurrentPathRarityTotalWeight;
    int mCurrentRegularTotalWeight;
    int mNumHeads = 0;
    int mMinRegSlots = 0;
    //BaseUpgradeSO[] mUpgradesRolled;
    System.Random rand = new System.Random();

    private void Awake()
    {
        GameManager.Instance.onSaveGame += SaveUpgrades;
        Instance = this;
        if (SavePointSystem.SavedStats)
        {
            LoadSave();
        }
        else
            Initialize();

        AlwaysIntialize();
    }

    private void Start()
    {
        for(int i = 0; i < mUnlockedUpgrades.Count;  ++i)
        {
            mUnlockedUpgrades[i].Upgrade();
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.onSaveGame -= SaveUpgrades;
    }

    private void SaveUpgrades()
    {
        SavePointSystem.PathRarityTotalWeight = mCurrentPathRarityTotalWeight;
        SavePointSystem.RegularRarityTotalWeight = mCurrentRegularTotalWeight;
        SavePointSystem.UnlockedPathUpgrades = new List<BasePathUpgradeSO>(mUnlockedUpgrades);
        SavePointSystem.PathUpgradeTotalWeights = (int[])mPathUpgradeTotalWeights.Clone();
        SavePointSystem.PathRarityWeights = new List<(UpgradeRarity, int)>(mPathRarityWeights);
        SavePointSystem.RegularUpgradeTotalWeights = (int[])mRegularUpgradeTotalWeights.Clone();
        SavePointSystem.RegRarityWeights = new List<(UpgradeRarity, int)>(mRegRarityWeights);

        for(int i = 0; i < UpgradeRarityAmount; ++i)
        {
            SavePointSystem.PathUpgradeRarityBuckets[i] = new List<BasePathUpgradeSO>(mPathUpgradeRarityBuckets[i]);
            SavePointSystem.PathUpgradeLootTables[i] = new List<int>(mPathUpgradeLootTables[i]);
            SavePointSystem.RegularUpgradeRarityBuckets[i] = new List<BaseUpgradeSO>(mRegularUpgradeRarityBuckets[i]);
            SavePointSystem.RegularUpgradeLootTables[i] = new List<int>(mRegularUpgradeLootTables[i]);
        }
    }

    private void LoadSave()
    {
        mCurrentPathRarityTotalWeight = SavePointSystem.PathRarityTotalWeight;
        mCurrentRegularTotalWeight = SavePointSystem.RegularRarityTotalWeight;
        mUnlockedUpgrades = SavePointSystem.UnlockedPathUpgrades;
        mPathUpgradeTotalWeights = SavePointSystem.PathUpgradeTotalWeights;
        mPathRarityWeights = SavePointSystem.PathRarityWeights;
        mRegularUpgradeTotalWeights = SavePointSystem.RegularUpgradeTotalWeights;
        mRegRarityWeights = SavePointSystem.RegRarityWeights;

        for (int i = 0; i < UpgradeRarityAmount; ++i)
        {
            mPathUpgradeRarityBuckets[i] = SavePointSystem.PathUpgradeRarityBuckets[i];
            mPathUpgradeLootTables[i] = SavePointSystem.PathUpgradeLootTables[i];
            mRegularUpgradeRarityBuckets[i] = SavePointSystem.RegularUpgradeRarityBuckets[i];
            mRegularUpgradeLootTables[i] = SavePointSystem.RegularUpgradeLootTables[i];
        }

        for(int i = 0; i < UpgradeRarityAmount; ++i)
        {
             mNumHeads += mPathUpgradeRarityBuckets[i].Count;
        }
    }

    private void AlwaysIntialize()
    {
        //mUpgradesRolled = new BaseUpgradeSO[NumUpgradeSlots];


        mMinRegSlots = mNumRegularUpgradeSlots;
        int numRegSlots = NumUpgradeSlots - mNumHeads;
        if (numRegSlots > mMinRegSlots)
        {
            mNumRegularUpgradeSlots = numRegSlots;
        }

        for (int i = 0; i < mUpgradePathLists.Length; ++i)
            mUpgradePathLists[i].InitializePath();
    }

    private void Initialize()
    {
        //mUpgradeHeads = new BasePathUpgradeSO[mUpgradePathLists.Length];

/*
        foreach (int weight in mRarityBucketWeights)
        {
            mTotalRarityWeight += weight;
        }*/

        for (int i = 0; i < UpgradeRarityAmount; ++i)
        {
            mPathUpgradeRarityBuckets[i] = new List<BasePathUpgradeSO>();
            mRegularUpgradeRarityBuckets[i] = new List<BaseUpgradeSO>();
        }

        BaseUpgradeSO upgradeSO;

        for (; mNumHeads < mUpgradePathLists.Length; ++mNumHeads)
        {
            upgradeSO = mUpgradePathLists[mNumHeads].Upgrades[0];
            mPathUpgradeRarityBuckets[(int)upgradeSO.Rarity].Add((BasePathUpgradeSO)upgradeSO);
        }

        for (int i = 0; i < UpgradeRarityAmount; ++i)
        {
            mPathUpgradeLootTables[i] = new List<int>();

            for (int j = 0; j < mPathUpgradeRarityBuckets[i].Count; ++j)
            {
                mPathUpgradeLootTables[i].Add(mPathUpgradeRarityBuckets[i][j].Weight);
                mPathUpgradeTotalWeights[i] += mPathUpgradeLootTables[i][j];
            }
        }


        for (int i = 0; i < mRegularUpgradesSO.RegularUpgrades.Length; ++i)
        {
            upgradeSO = mRegularUpgradesSO.RegularUpgrades[i];
            mRegularUpgradeRarityBuckets[(int)upgradeSO.Rarity].Add(upgradeSO);
        }

        for (int i = 0; i < (int)UpgradeRarity.MAX; ++i)
        {
            mRegularUpgradeLootTables[i] = new List<int>();

            for (int j = 0; j < mRegularUpgradeRarityBuckets[i].Count; ++j)
            {
                mRegularUpgradeLootTables[i].Add(mRegularUpgradeRarityBuckets[i][j].Weight);
                mRegularUpgradeTotalWeights[i] += mRegularUpgradeLootTables[i][j];
            }
        }

        for (int i = 0; i < UpgradeRarityAmount; ++i)
        {
            if (mPathUpgradeRarityBuckets[i].Count > 0)
            {
                mPathRarityWeights.Add(((UpgradeRarity)i, mRarityBucketWeights[i]));
                mCurrentPathRarityTotalWeight += mRarityBucketWeights[i];
            }

            if(mRegularUpgradeRarityBuckets[i].Count > 0)
            {
                mRegRarityWeights.Add(((UpgradeRarity)i, mRarityBucketWeights[i]));
                mCurrentRegularTotalWeight += mRarityBucketWeights[i];
            }
        }

    }

    public void IterateOnPath(BaseUpgradeSO _upgradeChose)
    {
        if (!(_upgradeChose is BasePathUpgradeSO))
            return;

        // int headIndex = mUpgradesRolled[_upgradeChose];
        BasePathUpgradeSO upgradeChose = (BasePathUpgradeSO)_upgradeChose;
        mUnlockedUpgrades.Add(upgradeChose);
        BasePathUpgradeSO nextUpgrade = upgradeChose.Next;
        int rarity = (int)upgradeChose.Rarity;
        int upgradeIndex = mPathUpgradeRarityBuckets[rarity].IndexOf(upgradeChose);
        //mPathUpgradeLootTables[rarity].Remove(upgradeChose.Weight);
        mPathUpgradeTotalWeights[rarity] -= upgradeChose.Weight;



        if (nextUpgrade == null)
        {
            mPathUpgradeRarityBuckets[rarity].RemoveAt(upgradeIndex);
            mPathUpgradeLootTables[rarity].RemoveAt(upgradeIndex);
            --mNumHeads;
            //mNumRegularUpgradeSlots = (NumUpgradeSlots - mNumHeads);
            int numRegSlots = NumUpgradeSlots - mNumHeads;
            if (numRegSlots >= mMinRegSlots)
            {
                mNumRegularUpgradeSlots = numRegSlots;
            }
        }
        else
        {

            int nextUpRarity = (int)nextUpgrade.Rarity;
            if (mPathUpgradeRarityBuckets[nextUpRarity].Count == 0)
            {
                mPathRarityWeights.Add((nextUpgrade.Rarity, mRarityBucketWeights[nextUpRarity]));
                mCurrentPathRarityTotalWeight += mRarityBucketWeights[nextUpRarity];

/*                int numRegSlots = NumUpgradeSlots - mRarityWeights.Count;
                if (numRegSlots > mMinRegSlots)
                {
                    mNumRegularUpgradeSlots = numRegSlots;
                }*/
            }

            
            if (upgradeChose.Rarity == nextUpgrade.Rarity)
            {
                mPathUpgradeRarityBuckets[rarity][upgradeIndex] = nextUpgrade;
                mPathUpgradeLootTables[rarity][upgradeIndex] = nextUpgrade.Weight;
            }
            else
            {
                mPathUpgradeRarityBuckets[rarity].RemoveAt(upgradeIndex);
                mPathUpgradeLootTables[rarity].RemoveAt(upgradeIndex);
                mPathUpgradeRarityBuckets[nextUpRarity].Add(nextUpgrade);
                mPathUpgradeLootTables[nextUpRarity].Add(nextUpgrade.Weight);
            }

            mPathUpgradeTotalWeights[nextUpRarity] += nextUpgrade.Weight;
        }

        if (mPathUpgradeTotalWeights[rarity] == 0)
        {
            mPathRarityWeights.Remove(((UpgradeRarity)rarity, mRarityBucketWeights[rarity]));
            mCurrentPathRarityTotalWeight -= mRarityBucketWeights[rarity];
        }

        //mUpgradeHeads[headIndex] = nextUpgrade;
    }

    public void AddPathUpgrade(BasePathUpgradeSO _pathUpgrade)
    {
        int rarity = (int)_pathUpgrade.Rarity;

        if (mPathUpgradeRarityBuckets[rarity].Count == 0)
        {
            mPathRarityWeights.Add((_pathUpgrade.Rarity, mRarityBucketWeights[rarity]));
            mCurrentPathRarityTotalWeight += mRarityBucketWeights[rarity];
        }

        mPathUpgradeRarityBuckets[rarity].Add(_pathUpgrade);
        mPathUpgradeLootTables[rarity].Add(_pathUpgrade.Weight);
        mPathUpgradeTotalWeights[rarity] += _pathUpgrade.Weight;
        ++mNumHeads;

        int numRegSlots = NumUpgradeSlots - mNumHeads;
        if (numRegSlots >= mMinRegSlots)
        {
            mNumRegularUpgradeSlots = numRegSlots;
        }

    }

    public void AddRegularUpgrade(BaseUpgradeSO _upgrade)
    {
        int rarity = (int)_upgrade.Rarity;

        if (mRegularUpgradeRarityBuckets[rarity].Count == 0)
        {
            mRegRarityWeights.Add((_upgrade.Rarity, mRarityBucketWeights[rarity]));
            mCurrentRegularTotalWeight += mRarityBucketWeights[rarity];
        }

        mRegularUpgradeRarityBuckets[rarity].Add(_upgrade);
        mRegularUpgradeLootTables[rarity].Add(_upgrade.Weight);
        mRegularUpgradeTotalWeights[rarity] += _upgrade.Weight;
    }

/*    public void SetRolledUpgrades(BaseUpgradeSO[] _rolledUpgrades)
    {
        mUpgradesRolled = _rolledUpgrades;
    }*/

    //TODO: Remove being able to roll dup regular upgrades & rarities that aren't in the list


    public BaseUpgradeSO[] RollUpgrades()
    {
        List<(UpgradeRarity, int)> rarityWeights = new List<(UpgradeRarity, int)>(mRegRarityWeights);
        int[] bucketListCounts = new int[(int)UpgradeRarity.MAX];
        int[] upgradeRarities = new int[NumUpgradeSlots];
        BaseUpgradeSO[] upgradesRolled = new BaseUpgradeSO[NumUpgradeSlots];

        int mWeight = mCurrentRegularTotalWeight;
        //int rolledIndex = 0;

        for (int i = 0; i < bucketListCounts.Length; ++i)
        {
            bucketListCounts[i] = mRegularUpgradeRarityBuckets[i].Count;
        }

        for (int i = 0; i < mNumRegularUpgradeSlots; ++i)
        {
            upgradeRarities[i] = RollRarity(rarityWeights, ref mWeight, bucketListCounts);

#if UNITY_EDITOR
            //Debug.Log(upgradeRarities[i]);
#endif
        }


        mWeight = mCurrentPathRarityTotalWeight;
        rarityWeights = new List<(UpgradeRarity, int)>(mPathRarityWeights);

        for (int i = 0; i < bucketListCounts.Length; ++i)
        {
            bucketListCounts[i] = mPathUpgradeRarityBuckets[i].Count;
        }

        for (int i = mNumRegularUpgradeSlots; i < NumUpgradeSlots; ++i)
        {
            upgradeRarities[i] = RollRarity(rarityWeights, ref mWeight, bucketListCounts);
        }

        int[] mNormalWeight = new int[mNumRegularUpgradeSlots];
        int[] mUpgradeIndecies = new int[NumUpgradeSlots];
        for (int i = 0; i < mNumRegularUpgradeSlots; ++i)
        {
            mUpgradeIndecies[i] = RollLootTable(mRegularUpgradeLootTables[upgradeRarities[i]].ToArray(), mRegularUpgradeTotalWeights[upgradeRarities[i]]);//rand.Next(mRegularUpgradeRarityBuckets[upgradeRarities[i]].Count);
            mNormalWeight[i] = mRegularUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]];  
            mRegularUpgradeTotalWeights[upgradeRarities[i]] -= mNormalWeight[i];
            mRegularUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = -1;
            upgradesRolled[i] = mRegularUpgradeRarityBuckets[upgradeRarities[i]][mUpgradeIndecies[i]];
        }

        List<int> mPathNormalWeights = new List<int>();
        for (int i = mNumRegularUpgradeSlots; i < NumUpgradeSlots; ++i)
        {
            mUpgradeIndecies[i] = RollLootTable(mPathUpgradeLootTables[upgradeRarities[i]].ToArray(), mPathUpgradeTotalWeights[upgradeRarities[i]]);//rand.Next(mHeadIndexRarityBuckets[upgradeRarities[i]].Count);
            //mUpgradesRolled[i] = mHeadIndexRarityBuckets[upgradeRarities[i]][mUpgradesRolled[i]];
            mPathNormalWeights.Add(mPathUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]]);
            mPathUpgradeTotalWeights[upgradeRarities[i]] -= mPathNormalWeights[i - mNumRegularUpgradeSlots];
            mPathUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = -1;
            upgradesRolled[i] = mPathUpgradeRarityBuckets[upgradeRarities[i]][mUpgradeIndecies[i]];
        }


        for (int i = 0; i < mNumRegularUpgradeSlots; ++i)
        {
            mRegularUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = mNormalWeight[i];
            mRegularUpgradeTotalWeights[upgradeRarities[i]] += mNormalWeight[i];
        }

        for (int i = mNumRegularUpgradeSlots; i < NumUpgradeSlots; ++i)
        {
            mPathUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = mPathNormalWeights[i - mNumRegularUpgradeSlots];
            mPathUpgradeTotalWeights[upgradeRarities[i]] += mPathNormalWeights[i - mNumRegularUpgradeSlots];
            //mUpgradesRolled[i] = mPathUpgradeRarityBuckets[upgradeRarities[i]][mUpgradeIndecies[i]];
        }

#if UNITY_EDITOR
        for (int i = 0; i < upgradesRolled.Length; ++i)
        {
            for (int j = 0; j < upgradesRolled.Length; ++j)
            {
                if (i == j)
                    continue;
                else
                {
                    if (upgradesRolled[i].UpgradeName == upgradesRolled[j].UpgradeName)
                    {
                        //TELL YASH IF IT EVER BREAKS HERE!!!
                        Debug.Break();
                    }
                }
            }
        }
#endif



        //Return upgrades
        return upgradesRolled;
    }

    public BaseUpgradeSO[] RollUpgrades(int _numRegularUpgrades, int[] _regRarityLootTable, int[] _pathRarityLootTable)
    {
        /*if (_numRegularUpgrades < mNumRegularUpgradeSlots)
            _numRegularUpgrades = mNumRegularUpgradeSlots;*/


        List<(UpgradeRarity, int)> rarityWeights = new List<(UpgradeRarity, int)>();
        int[] bucketListCounts = new int[(int)UpgradeRarity.MAX];
        int[] upgradeRarities = new int[NumUpgradeSlots];
        BaseUpgradeSO[] upgradesRolled = new BaseUpgradeSO[NumUpgradeSlots];

        int mWeight = 0;
        int pathHeads = 0;
        //int rolledIndex = 0;


        for (int i = 0; i < bucketListCounts.Length; ++i)
        {
            bucketListCounts[i] = mPathUpgradeRarityBuckets[i].Count;
            pathHeads += bucketListCounts[i];
        }


        int upgradeSlots = 0;
        upgradeSlots = NumUpgradeSlots - pathHeads;

        if (upgradeSlots > _numRegularUpgrades)
        {
            _numRegularUpgrades = upgradeSlots;
        }
        int[] mUpgradeIndecies = new int[NumUpgradeSlots];

        if (_numRegularUpgrades < NumUpgradeSlots)
        {

            for (int i = 0; i < UpgradeRarityAmount; ++i)
            {
                if (bucketListCounts[i] > 0 && _pathRarityLootTable[i] > 0)
                {
                    rarityWeights.Add(((UpgradeRarity)i, _pathRarityLootTable[i]));
                    mWeight += _pathRarityLootTable[i];
                }
            }

            for (int i = _numRegularUpgrades; i < NumUpgradeSlots; ++i)
            {
                upgradeRarities[i] = RollRarity(rarityWeights, ref mWeight, bucketListCounts);
            }

            List<int> mPathNormalWeights = new List<int>();
            for (int i = _numRegularUpgrades; i < NumUpgradeSlots; ++i)
            {
                mUpgradeIndecies[i] = RollLootTable(mPathUpgradeLootTables[upgradeRarities[i]].ToArray(), mPathUpgradeTotalWeights[upgradeRarities[i]]);//rand.Next(mHeadIndexRarityBuckets[upgradeRarities[i]].Count);
                                                                                                                                                        //mUpgradesRolled[i] = mHeadIndexRarityBuckets[upgradeRarities[i]][mUpgradesRolled[i]];
                mPathNormalWeights.Add(mPathUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]]);
                mPathUpgradeTotalWeights[upgradeRarities[i]] -= mPathNormalWeights[i - _numRegularUpgrades];
                mPathUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = -1;
                upgradesRolled[i] = mPathUpgradeRarityBuckets[upgradeRarities[i]][mUpgradeIndecies[i]];
            }



            for (int i = _numRegularUpgrades; i < NumUpgradeSlots; ++i)
            {
                mPathUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = mPathNormalWeights[i - _numRegularUpgrades];
                mPathUpgradeTotalWeights[upgradeRarities[i]] += mPathNormalWeights[i - _numRegularUpgrades];
                //mUpgradesRolled[i] = mPathUpgradeRarityBuckets[upgradeRarities[i]][mUpgradeIndecies[i]];
            }
        }

        if(_numRegularUpgrades > 0)
        {

            mWeight = 0;
            rarityWeights = new List<(UpgradeRarity, int)>();

            for (int i = 0; i < bucketListCounts.Length; ++i)
            {
                bucketListCounts[i] = mRegularUpgradeRarityBuckets[i].Count;
            }

            for (int i = 0; i < UpgradeRarityAmount; ++i)
            {
                if (bucketListCounts[i] > 0 && _regRarityLootTable[i] > 0)
                {
                    rarityWeights.Add(((UpgradeRarity)i, _regRarityLootTable[i]));
                    mWeight += _regRarityLootTable[i];
                }
            }


            for (int i = 0; i < _numRegularUpgrades; ++i)
            {
                upgradeRarities[i] = RollRarity(rarityWeights, ref mWeight, bucketListCounts);
            }

            //BaseUpgradeSO[] upgrades = new BaseUpgradeSO[NumUpgradeSlots];
            int[] mNormalWeight = new int[_numRegularUpgrades];

           // Debug.Log("Rolling upgrades");
            for (int i = 0; i < _numRegularUpgrades; ++i)
            {
                mUpgradeIndecies[i] = RollLootTable(mRegularUpgradeLootTables[upgradeRarities[i]].ToArray(), mRegularUpgradeTotalWeights[upgradeRarities[i]]);//rand.Next(mRegularUpgradeRarityBuckets[upgradeRarities[i]].Count);
                mNormalWeight[i] = mRegularUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]];
                mRegularUpgradeTotalWeights[upgradeRarities[i]] -= mNormalWeight[i];
                mRegularUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = -1;
                upgradesRolled[i] = mRegularUpgradeRarityBuckets[upgradeRarities[i]][mUpgradeIndecies[i]];
            }


            for (int i = 0; i < _numRegularUpgrades; ++i)
            {
                mRegularUpgradeLootTables[upgradeRarities[i]][mUpgradeIndecies[i]] = mNormalWeight[i];
                mRegularUpgradeTotalWeights[upgradeRarities[i]] += mNormalWeight[i];
            }
           // Debug.Log("Resetting weights");

        }










#if UNITY_EDITOR
        for(int i = 0; i < upgradesRolled.Length; ++i)
        {
            for(int j = 0; j < upgradesRolled.Length; ++j)
            {
                if (i == j)
                    continue;
                else
                {
                    if(upgradesRolled[i].UpgradeName == upgradesRolled[j].UpgradeName)
                    {
                        //TELL YASH IF IT EVER BREAKS HERE!!!
                        Debug.Break();
                    }
                }
            }
        }
#endif

        //Return upgrades
        return upgradesRolled;
    }

    private int RollRarity(List<(UpgradeRarity, int)> _rarityWeights, ref int _weight, int[] _bucketListCounts)
    {
        int rolledNum = rand.Next(0, _weight);

        for (int i = 0; i < _rarityWeights.Count; ++i)
        {
            rolledNum -= _rarityWeights[i].Item2;

            if (rolledNum <= 0)
            {
                rolledNum = (int)_rarityWeights[i].Item1;
                --_bucketListCounts[rolledNum];
                if (_bucketListCounts[rolledNum] <= 0)
                {
                    _weight -= _rarityWeights[i].Item2;
                    _rarityWeights.Remove(_rarityWeights[i]);
                }
                return rolledNum;
            }
        }

        return -1;
    }

    private int RollLootTable(int[] _lootTable, int _totalWeight)
    {
        int rolledNum = rand.Next(0, _totalWeight);

        for (int i = 0; i < _lootTable.Length; ++i)
        {
            rolledNum -= _lootTable[i];

            if (rolledNum <= 0)
                return i;
        }

        return -1;
    }
}
