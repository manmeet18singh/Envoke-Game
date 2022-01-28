using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lume Pickup Data", menuName = "Envoke/Pickup Data/Lume Pickup")]
public class LumePickupData : PickupData
{
    [SerializeField]
    protected Lume mLumeType; public Lume LumeType { get => mLumeType; }
    [SerializeField] private string mLumeName; public string LumeName { get => mLumeName; }

    public override void PickupSpawned(Pickup _pickup)
    {
        // TODO - show some kind of entry animation on model
    }

    public override PickupType GetPickupType()
    {
        return PickupType.Lume;
    }
}
