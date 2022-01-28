using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flame Wreath Data", menuName = "Envoke/Boss Data/Flame Wreath Data")]
public class FlameWreathSO : ScriptableObject
{
    public enum PathType { Circle, FollowPlayer }
    [Header("Spell Properties")]
    [SerializeField] public PathType mPathType = PathType.Circle;
    [SerializeField] public float mDistanceOffset = 3;
    [SerializeField] public float mMinFollowDistance = 3;
    [Range(0, 6.283f)] [SerializeField] public float mAngleInterval = 0.2618f;
    [SerializeField] public float mSpawnSpeed = 0.25f;

    [Header("Debug")]
    [SerializeField] public GameObject mOverrideTarget;
}
