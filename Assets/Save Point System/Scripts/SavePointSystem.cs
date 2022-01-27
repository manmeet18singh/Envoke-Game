using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavePointSystem
{
    public static int LastRoomSaved = -1;
    public static bool SavedStats = false;
    public static int Health;
    public static int MaxHealth;
    public static int Gold;
    public static int PathRarityTotalWeight;
    public static int RegularRarityTotalWeight;
    public static int[] CurrentLumes;
    public static int[] MaxLumes;
    public static int[] UnlockedLumes;
    public static int NumLumesUnlocked;
/*    public static int[] UnlockedLumes;
    public static int NumUnlockedLumes;*/
    public static List<BasePathUpgradeSO> UnlockedPathUpgrades;
    public static List<BasePathUpgradeSO>[] PathUpgradeRarityBuckets = new List<BasePathUpgradeSO>[(int)UpgradeRarity.MAX];
    public static List<int>[] PathUpgradeLootTables = new List<int>[(int)UpgradeRarity.MAX];
    public static int[] PathUpgradeTotalWeights;
    public static List<(UpgradeRarity, int)> PathRarityWeights;
    public static List<BaseUpgradeSO>[] RegularUpgradeRarityBuckets = new List<BaseUpgradeSO>[(int)UpgradeRarity.MAX];
    public static List<int>[] RegularUpgradeLootTables = new List<int>[(int)UpgradeRarity.MAX];
    public static int[] RegularUpgradeTotalWeights;
    public static List<(UpgradeRarity, int)> RegRarityWeights;

}
