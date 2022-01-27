using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class CollectorBehavior : MonoBehaviour
{
    public abstract void PickUp(Pickup _pickup);

    public abstract bool CanBePickedUp(Pickup pickup);
}
