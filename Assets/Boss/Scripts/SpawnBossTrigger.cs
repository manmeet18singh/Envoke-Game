using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossTrigger : MonoBehaviour
{
    [SerializeField] Vector3 mSpawnLocation;
    [SerializeField] GameObject mBossObject;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(mBossObject, mSpawnLocation, Quaternion.identity);
            //GameManager.Instance.mBoss.SetActive(true);
            GameManager.Instance.mBossUI.SetActive(true);
            GameManager.Instance.UpdateEnemiesRemaining(1);
            Destroy(gameObject);
        }
    }
       
}
