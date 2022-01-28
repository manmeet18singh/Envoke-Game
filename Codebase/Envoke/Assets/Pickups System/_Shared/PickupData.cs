using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Lume = 0,
    Health = 1,
    Gold = 2
}

/// <summary>
/// PickupData controls all of the static information per type of pickup we 
/// create. 
/// </summary>
public abstract class PickupData : ScriptableObject
{
    [Header("General Properties")]
    [SerializeField]
    private string mName = null;
    public string Name { get => mName; }
    [SerializeField]
    private int mAmount = 1; public int Amount { get => mAmount; }
    [SerializeField]
    private float mLifeSpan = -1; public float LifeSpan { get => mLifeSpan; }
    [SerializeField] private string[] mSfx = null;
    public string[] SFX { get => mSfx; }

    [Header("Visual Properties")]
    [SerializeField]
    private GameObject mModel = null;
    public GameObject Model { get => mModel; }
    [SerializeField]
    private float mLerpSpeed = 15; public float LerpSpeed { get => mLerpSpeed; }

    // Called when the pickup is first spawned into the map
    public abstract void PickupSpawned(Pickup _pickup);

    // Called when the pickup is collected by either an enemy or player
    public virtual void PickupCollected(GameObject _collector, Pickup _pickup)
    {
        _pickup.CleanUp();
    }

    // Called to dynamically determine what type of pickup the data belongs to
    public abstract PickupType GetPickupType();
}
