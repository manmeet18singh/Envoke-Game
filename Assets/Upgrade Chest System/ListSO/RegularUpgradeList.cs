using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegularUpgradeList", menuName = "Envoke/Upgrades/RegularUpgradeList")]
public class RegularUpgradeList : ScriptableObject
{
    [field: SerializeField]
    public BaseUpgradeSO[] RegularUpgrades = null;
}
