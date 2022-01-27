using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a sort-of hacky solution to attach pickup collision behavior on any prefab
/// we want. This scrip should NOT be manually added to any prefab! The <see cref="Pickup"/>
/// script will automatically attach this script on load. 
/// (see: <see cref="Pickup.LoadPickup(PickupData)"/>)
/// </summary>
public class PickupCollision : MonoBehaviour
{
    public Pickup mPickup;

    private void OnTriggerEnter(Collider other)
    {
        CollectorBehavior collector = other.GetComponent<CollectorBehavior>();

        if (collector != null)
        {
            collector.PickUp(mPickup);
        }
    }
}
