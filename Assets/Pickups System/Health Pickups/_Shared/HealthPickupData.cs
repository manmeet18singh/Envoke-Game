using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Pickup Data", menuName = "Envoke/Pickup Data/Health Pickup")]
public class HealthPickupData : PickupData
{
    public override PickupType GetPickupType()
    {
        return PickupType.Health;
    }

    public override void PickupSpawned(Pickup _pickup)
    {
        // TODO - show some kind of entry animation on model
    }
}
