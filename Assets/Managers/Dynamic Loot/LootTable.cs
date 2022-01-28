using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    [Serializable]
    public class Drop
    {
        public PickupData mDrop;
        public int mWeight;
    }

    public Drop[] table;

}
