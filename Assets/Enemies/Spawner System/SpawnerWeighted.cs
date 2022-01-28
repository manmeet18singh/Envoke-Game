using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWeighted : SpawnerBase, IComparer<GameObject>
{
    [SerializeField]
    protected float mTotalEnemyWeight = 25;
    protected List<GameObject> mEnemiesToSpawn = new List<GameObject>();

    // State variables
    private float mCurrentEnemyWeight = 0;

    override protected void Awake()
    {
        mEnemies.Sort(this);
        base.Awake();
        GameManager.Instance.onRoomCompleted += StopAllCoroutines;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onRoomCompleted -= StopAllCoroutines;
    }

    private void SetupEnemyList()
    {
        mEnemiesToSpawn = new List<GameObject>();
        float lowestWeight = mEnemies[0].GetComponent<Enemy>().mEnemySettings.mEnemyDifficulty;
        int numberOfEnemiesSpawned = 0;
        while (mCurrentEnemyWeight + lowestWeight <= mTotalEnemyWeight)
        {
            GameObject pickedEnemy = PickEnemy();
            if (pickedEnemy == null)
                break;

            mEnemiesToSpawn.Add(pickedEnemy);
            ++numberOfEnemiesSpawned;
            mCurrentEnemyWeight += pickedEnemy.GetComponent<Enemy>().mEnemySettings.mEnemyDifficulty;
        }
    }

    override public void Initialize()
    {
        SetupEnemyList();

        Instantiate(mEffect, GetSpawnLocation(), transform.rotation);
        StartCoroutine(BeginSpawning());
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

    private GameObject PickEnemy()
    {
        int endRange = mEnemies.Count;
        while (endRange >= 0)
        {
            GameObject pickedEnemy = mEnemies[Random.Range(0, endRange)];
            float enemyDifficulty = pickedEnemy.GetComponent<Enemy>().mEnemySettings.mEnemyDifficulty;
            if (mCurrentEnemyWeight + enemyDifficulty <= mTotalEnemyWeight)
            {
                mCurrentEnemyWeight += enemyDifficulty;
                return pickedEnemy;
            }
            if (endRange == 0)
                return null;

            endRange = Mathf.Clamp(endRange / 2, 0, endRange);
        }

        return null;
    }

    private IEnumerator BeginSpawning()
    {
        while (mEnemiesToSpawn.Count > 0)
        {
            yield return SpawnEnemy(mEnemiesToSpawn[0]);
            yield return new WaitForSeconds(mSpawnInterval);
        }  
    }

    public int Compare(GameObject _first, GameObject _second)
    {
        Enemy firstEnemy = _first.GetComponent<Enemy>();
        Enemy secondEnemy = _second.GetComponent<Enemy>();
        if (firstEnemy == null || secondEnemy == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Spawner list contains object that is not an enemy.");
            return 0;
#endif
        }

        return firstEnemy.mEnemySettings.mEnemyDifficulty - secondEnemy.mEnemySettings.mEnemyDifficulty;
    }
}
