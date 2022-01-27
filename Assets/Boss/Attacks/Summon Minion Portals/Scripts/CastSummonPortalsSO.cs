using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Portal Data", menuName = "Envoke/Boss Data/Summon Portal Data")]
public class CastSummonPortalsSO : ScriptableObject
{
    [Header("Portal Locations")]
    [SerializeField] private bool mIsRandom = false; 
    public bool IsRandom { get => mIsRandom; }
    //[SerializeField] private List<GameObject> mSummonLocations;
    //public List<Transform> SummonLocations { get => GetLocationsFromObjects(mSummonLocations); }
    [SerializeField] private float mMaxDistance = 35;
    public float MaxPortalDistance { get => mMaxDistance; }

    [Header("Summoning Properties")]
    [SerializeField] private int mNumberOfRandomPortals = 3;
    public int NumberOfRandomPortals { get => mNumberOfRandomPortals; }
    [SerializeField] private float mTimeBetweenPortalSummons = 2;
    public float TimeBetweenPortalSummons { get => mTimeBetweenPortalSummons; }
    [SerializeField] private bool mShouldTeleportBeforeSummon = true;
    public bool ShouldTeleportBeforeSummon { get => mShouldTeleportBeforeSummon; }

    [Header("Portal Properties")]
    [SerializeField] private List<GameObject> mPortals = null;
    public List<GameObject> Portals { get => mPortals; }

    //public static List<Transform> GetLocationsFromObjects(List<GameObject> _summonLocations)
    //{
    //    List<Transform> locations = new List<Transform>();
    //    foreach (GameObject locationObject in _summonLocations)
    //    {
    //        locations.Add(locationObject.transform);
    //    }

    //    return locations;
    //}
}
