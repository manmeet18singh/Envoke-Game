using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "GoldPickup", menuName = "Envoke/Pickup Data/Gold Pickup")]
public class GoldPickupData : PickupData
{
    [SerializeField]
    private int mDropRate;

    public int DropRate { get => mDropRate; private set => mDropRate = value; }

    public override PickupType GetPickupType()
    {
        return PickupType.Gold;
    }

    public override void PickupSpawned(Pickup _pickup)
    {
        // TODO - show some kind of entry animation on model
    }
}
