using System;
using UnityEngine;

public abstract class BaseUpgradeSO : ScriptableObject
{
    [field: SerializeField]
    public Sprite UpgradeIcon { get; private set; }
    [field: SerializeField]
    public UpgradeRarity Rarity { get; private set; }
    [field : SerializeField]
    public int Weight { get; private set; }
/*    [field: Tooltip("Number of times upgrade will show up till next upgrade in path will unlock")]
    [field: SerializeField]
    public int CountTillRemove { get; private set; }*/
    [field : SerializeField]
    public int Cost { get; private set; }
    [field: SerializeField]
    public string UpgradeName { get; private set; }
    [field: SerializeField]
    [field: TextArea]
    public string UpgradeDesc { get; private set; }

    public virtual void Upgrade() { }

}
