using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : SpawnerUnweighted
{
    private bool mIsEnabled = false;
    public bool IsEnabled { get => mIsEnabled; set => mIsEnabled = value; }

    public override IEnumerator BeginSpawning()
    {
        while (!IsEnabled)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return base.BeginSpawning();
    }

    public override IEnumerator SpawnEnemy(GameObject _enemy)
    {
        Vector3 spawnLocation = GetSpawnLocation();
        Quaternion rotation = Quaternion.LookRotation(GameManager.Instance.mPlayer.transform.position - transform.position);
        // Instantiate the very first monster immediately as if it was always there
        if (mSpawnOnStart)
        {
            EnemyBasic enemy = Instantiate(_enemy, spawnLocation, rotation).GetComponent<EnemyBasic>();
            if (enemy != null)
                enemy.mSpawnedInBossRoom = true;
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
            EnemyBasic enemy = Instantiate(_enemy, spawnLocation, rotation).GetComponent<EnemyBasic>();
            if (enemy != null)
                enemy.mSpawnedInBossRoom = true;
            mSpawnOnStart = false;
        }
    }

}
