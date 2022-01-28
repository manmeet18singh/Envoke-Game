using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice Torrent Data", menuName = "Envoke/Boss Data/Ice Torrent Data")]
public class IceTorrentSO : ScriptableObject
{
    public enum PathType { Straight, FollowPlayer }
    [Header("Spell Properties")]
    [SerializeField] public PathType mPathType = PathType.Straight;
    [SerializeField] public float mDistanceBetweenTorrents = 3;
    [SerializeField] public float mDistanceVariation = 1;
    [SerializeField] public float mMinFollowDistance = 3;
    [Range(0, 6.283f)] [SerializeField] public float mAngleInterval = 0.2618f;
    [SerializeField] public float mSpawnSpeed = 0.25f;

    [Header("Debug")]
    [SerializeField] public GameObject mOverrideTarget;
}
