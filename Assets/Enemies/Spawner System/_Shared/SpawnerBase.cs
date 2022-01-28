using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerBase : MonoBehaviour  
{
    // List of enemies that can spawn
    [SerializeField]
    protected List<GameObject> mEnemies;
    // Interval between spawn ticks
    [SerializeField]
    protected float mSpawnInterval = 3;
    // Area which enemies can randomly spawn in
    [SerializeField]
    protected float mSpawnRadius = 4;
    // Whether to start spawning at game start
    [SerializeField]
    protected bool mSpawnOnStart = true;

    [SerializeField] protected ParticleSystem mEffect;
    [SerializeField] protected string[] mSfx = null;

    protected virtual void Awake()
    {
        if (mSpawnOnStart)
            Initialize();
    }

    public abstract IEnumerator SpawnEnemy(GameObject _enemy);

    public abstract void Initialize();

    public virtual Vector3 GetSpawnLocation()
    {
        Vector3 randomOffset = Random.insideUnitSphere* mSpawnRadius;
        randomOffset.y = 0;
        return transform.position + randomOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, mSpawnRadius);
    }
}
