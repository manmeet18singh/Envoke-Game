using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerUnweighted : SpawnerBase
{
    // Properties
    [SerializeField] protected int mNumberOfEnemies = 5;

    protected List<GameObject> mEnemiesToSpawn = new List<GameObject>();

    protected bool IsInfiniteSpawner { get => mNumberOfEnemies < 0; }

    public override void Initialize()
    {
        //Debug.Log("Starting...");
        if (!IsInfiniteSpawner)
            SetupEnemyList();

        StartCoroutine(BeginSpawning());
        GameManager.Instance.onRoomCompleted += StopAllCoroutines;
    }

    private void SetupEnemyList()
    {
        for (int i = 0; i < mNumberOfEnemies; ++i)
        {
            mEnemiesToSpawn.Add(mEnemies[Random.Range(0, mEnemies.Count)]);
        }
        GameManager.Instance.UpdateEnemiesRemaining(mNumberOfEnemies);
    }

    private void OnDestroy()
    {
        GameManager.Instance.onRoomCompleted -= StopAllCoroutines;
    }

    public override IEnumerator SpawnEnemy(GameObject _enemy)
    {
        Vector3 spawnLocation = GetSpawnLocation();
        Quaternion rotation = Quaternion.LookRotation(GameManager.Instance.mPlayer.transform.position - transform.position);
        // Instantiate the very first monster immediately as if it was always there
        if (mSpawnOnStart)
        {
            Instantiate(_enemy, spawnLocation, rotation);
            mSpawnOnStart = false;
            yield return null;
        }
        else
        {
            // Spawn in with a special effect, if there is one!
            if (mEffect != null)
            {
                Instantiate(mEffect, new Vector3(spawnLocation.x, spawnLocation.y + .25f, spawnLocation.z), rotation);
                mEffect.Play(true);
                AudioManager.PlayRandomSFX(mSfx);
                yield return new WaitForSeconds(3f);
            }
            Instantiate(_enemy, spawnLocation, rotation);
        }
    }

    virtual public IEnumerator BeginSpawning()
    {
        if (IsInfiniteSpawner)
            while(true)
            {
                yield return SpawnEnemy(mEnemies[Random.Range(0, mEnemies.Count)]);
                yield return new WaitForSeconds(mSpawnInterval);
            }
        else
            for (int i = 0; i < mNumberOfEnemies; ++i)
            {
                yield return SpawnEnemy(mEnemiesToSpawn[0]);
                mEnemiesToSpawn.RemoveAt(0);
                yield return new WaitForSeconds(mSpawnInterval);
            }
    }
}
