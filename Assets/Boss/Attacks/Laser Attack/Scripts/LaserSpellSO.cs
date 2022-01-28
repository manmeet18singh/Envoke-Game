using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Laser Spell Data", menuName = "Envoke/Boss Data/Laser Spell Data")]
public class LaserSpellSO : ScriptableObject
{
    [System.Serializable]
    public struct StartStop
    {
        public Vector3 StartPositionOffset;
        public Vector3 EndPositionOffset;
    }

    public enum LaserType { Circular, FollowPlayer, RandomRapidFire}

    [Header("Path Type")]
    [SerializeField] private LaserType mLaserType = LaserType.Circular;
    public LaserType LaserPathType { get => mLaserType; }

    [Header("Laser Properties")]
    [SerializeField] private List<StartStop> mStartStopPositions = null;
    public List<StartStop> LazerPositions { get => mStartStopPositions; }
    [SerializeField] private int mDamage = 20;
    public int Damage { get => mDamage; }
    [SerializeField] private float mBeamGrowthRate = 10;
    public float BeamGrowthRate { get => mBeamGrowthRate; }
    [SerializeField] private float mBeamDecayhRate = 10;
    public float BeamDecaythRate { get => mBeamDecayhRate; }
    [SerializeField] private bool mGrowBeamsBeforeMoving = true;
    public bool GrowBeamsBeforeMoving { get => mGrowBeamsBeforeMoving; }
    [SerializeField] private float mMovementSpeed = 100;
    public float MovementSpeed { get => mMovementSpeed; }
    [SerializeField] private float mAngleInterval = 15;
    public float AngleInterval { get => mAngleInterval; }

}
