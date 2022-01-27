using UnityEngine;

public enum UpgradeRarity
{
    Common = 0,
    Rare = 1,
    Legendary = 2,
    MAX
}

[CreateAssetMenu(fileName = "UpgradePathSO", menuName = "Envoke/Upgrades/UpgradeList")]
public class UpgradePathSO : ScriptableObject
{
    [field: SerializeField]
    public BasePathUpgradeSO[] Upgrades { get; private set; }

    public void InitializePath()
    {
        for (int i = 1; i < Upgrades.Length; ++i)
        {
            if (Upgrades[i - 1] && Upgrades[i])
                Upgrades[i - 1].Next = Upgrades[i];
        }

        Upgrades[Upgrades.Length - 1].Next = null;
    }


}
