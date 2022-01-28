using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossHealthStats", menuName = "Enemies/BossHealth")]
public class BossHealthData : ScriptableObject
{
    public int[] mHealthThresholds = null;
    public Color[] mHealthBarColors;
}
