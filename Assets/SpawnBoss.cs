using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [Header("Spawn Properties")]
    [SerializeField] GameObject mSpawnLocation;
    [SerializeField] List<GameObject> mPortalSpawnPoints = null;
    [Header("Other Properties")]
    [SerializeField] string mBossMusic = "BossBG";
    [Header("References")]
    [SerializeField] GameObject mBossObject;
    



    void Start()
    {
        GameObject bossObj = Instantiate(mBossObject, mSpawnLocation.transform.position, Quaternion.identity);
        GameManager.Instance.UpdateEnemiesRemaining(1);

        FinalBoss finalBoss = bossObj.GetComponent<FinalBoss>();
        finalBoss.PortalSpawnPoints = mPortalSpawnPoints;
        finalBoss.SpawnPoint = transform.position;
        finalBoss.gameObject.transform.position = transform.position;
        //GameManager.Instance.mBoss.SetActive(true);
        GameManager.Instance.mBossUI.SetActive(true);

        // Start Boss Music
        AudioManager.ChangeBackgroundMusic(mBossMusic);
    }

}
