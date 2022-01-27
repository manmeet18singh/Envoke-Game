using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the player's ability to pick up various pickups.
/// It also handles how the player interacts with various pickup types.
/// </summary>
public class PlayerPickupCollector : CollectorBehavior
{
    #region Properties
    [SerializeField] private int mTimesToShowHealthWarning = 5;
    [SerializeField] private int mTimesToShowSoleisWarning = 5;
    [SerializeField] private int mTimesToShowCinosWarning = 5;
    [SerializeField] private int mTimesToShowEdurWarning = 5;
    #endregion

    #region State Variables
    private int mTimesShownHealthWarning = 0;
    private int mTimesShownSoleisWarning = 0;
    private int mTimesShownEdurWarning = 0;
    private int mTimesShownCinosWarning = 0;
    #endregion


    /// <summary>
    /// Moves the pickup from its position in the game world to its respective
    /// UI position. The pickup is only considered "collected" AFTER the lerp
    /// animation has completed.
    /// </summary>
    /// <param name="_pickup">The pickup to lerp into the UI.</param>
    private IEnumerator LerpPickup(Pickup _pickup)
    {
        Vector3 startPostion = _pickup.transform.position;
        float startTime = Time.time;
        float journeyLength;
        _pickup.mCollider.enabled = false;
        //GameObject target = _pickup.mPickupData.GetCorrespondingUIDummyObjet();
        Vector3 destination = transform.position;
        float timeout = 3;
        float currentDuration = 0;

        float distCovered = 0;
        float fractionOfJourney = 0.1f;
        while (Vector3.Distance(_pickup.transform.position, destination) > 1 && _pickup != null)
        {
            currentDuration += Time.deltaTime;
            yield return null;
            destination = transform.position;
            journeyLength = Vector3.Distance(startPostion, destination);
            if (_pickup == null || currentDuration > timeout)
                break;
            distCovered += Time.deltaTime * _pickup.mPickupData.LerpSpeed;
            fractionOfJourney = distCovered / journeyLength;
            _pickup.transform.position = Vector3.Lerp(startPostion, destination, fractionOfJourney);
            _pickup.transform.localScale *= (1 - (fractionOfJourney / 100));
        }

        if (_pickup != null)
            PickupCollected(_pickup);
    }

    /// <summary>
    /// This function is called after a pickup has completed the lerp animation.
    /// The player has their resources modified based on the type of pickup that
    /// was collected.
    /// </summary>
    /// <param name="_pickup">The pickup to be "collected". Determines what resource
    /// the player recieves and how much.</param>
    private void PickupCollected(Pickup _pickup)
    {
        System.Random goldValue = new System.Random();

        PickupData pickupData = _pickup.mPickupData;
        switch (pickupData.GetPickupType())
        {
            case PickupType.Health:
                AudioManager.instance.Play("Collect Health");
                GameManager.Instance.mPlayerHealth.Heal(pickupData.Amount);
                break;
            case PickupType.Lume:
                AudioManager.PlayRandomSFX(_pickup.mPickupData.SFX);
                LumeInventory.TryAddLume((int)((LumePickupData)pickupData).LumeType, pickupData.Amount);
                break;
            case PickupType.Gold:
                CurrencyManager.Instance.AddGold(pickupData.Amount * goldValue.Next(1, 14));
                break;
            default:
#if UNITY_EDITOR
                Debug.LogError("Player has no logic to collect " + _pickup.mPickupData.GetPickupType());
#endif
                break;
        }

        _pickup.mPickupData.PickupCollected(gameObject, _pickup);
    }

    /// <summary>
    /// Implementation of abstract function that is called whenever a pickup detects
    /// the player has walked over it. This function handles determining whether
    /// or not the player can pick up the colliding pickup. If they can, a lerp
    /// animation is started. (See: <see cref="LerpPickup(Pickup)"/>)
    /// </summary>
    /// <param name="_pickup">The pickup that triggered this function call.</param>
    public override void PickUp(Pickup _pickup)
    {
        // Check if player can actually pick up the pickup
        if (!CanBePickedUp(_pickup))
            return;

        // begin lerp animation
        StartCoroutine(LerpPickup(_pickup));
    }

    /// <summary>
    /// Implementation of abstract function to determine whether a certain pickup
    /// can be picked up by the player.
    /// </summary>
    /// <param name="_pickup">The pickup to check.</param>
    /// <returns>True if the pickup can be picked up, False otherwise.</returns>
    public override bool CanBePickedUp(Pickup _pickup)
    {
        PickupData pickupData = _pickup.mPickupData;
        switch (pickupData.GetPickupType())
        {
            case PickupType.Health:
                PlayerHealth playerHealth = GameManager.Instance.mPlayerHealth;
                if (playerHealth.CurrentHealth >= playerHealth.MaxHealth && mTimesShownHealthWarning++ < mTimesToShowHealthWarning)
                {
                    NotificationManager.Instance.AddNotification("Health is full!", .4f);
                }
                return playerHealth.CurrentHealth < playerHealth.MaxHealth;
            case PickupType.Lume:
                LumePickupData lumeData = (LumePickupData)pickupData;
                int timesShown = lumeData.LumeType == Lume.CINOS ? mTimesShownCinosWarning 
                    : lumeData.LumeType == Lume.EDUR ? mTimesShownEdurWarning 
                    : mTimesShownSoleisWarning;
                int timesToShow = lumeData.LumeType == Lume.CINOS ? mTimesToShowCinosWarning 
                    : lumeData.LumeType == Lume.EDUR ? mTimesToShowEdurWarning 
                    : mTimesToShowSoleisWarning;
                
                if (LumeInventory.IsLumeTypeFull((int)((LumePickupData)pickupData).LumeType) && timesShown++ < timesToShow)
                {
                    NotificationManager.Instance.AddNotification(((LumePickupData)pickupData).LumeName + " capacity is full!", .4f);
                    if (lumeData.LumeType == Lume.CINOS)
                        mTimesShownCinosWarning = timesShown;
                    else if (lumeData.LumeType == Lume.EDUR)
                        mTimesShownEdurWarning = timesShown;
                    else
                        mTimesShownSoleisWarning = timesShown;
                }
                    
                return !LumeInventory.IsLumeTypeFull((int)((LumePickupData)pickupData).LumeType);
            case PickupType.Gold:
                AudioManager.instance.Play("Step on Gold");
                return true;
            default:
#if UNITY_EDITOR
                Debug.Log("Player has no logic to pick up " + _pickup.mPickupData.GetPickupType());
#endif
                return false;
        }
    }
}
