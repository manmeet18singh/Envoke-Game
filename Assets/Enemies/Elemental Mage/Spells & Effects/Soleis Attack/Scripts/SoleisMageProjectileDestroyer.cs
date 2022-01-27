using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoleisMageProjectileDestroyer : MonoBehaviour
{
    [SerializeField]
    private GameObject mHitEffect = null;
    [SerializeField]
    private GameObject mParent = null;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("DESTROY");
        Instantiate(mHitEffect, transform.position, Quaternion.identity);
        Destroy(mParent);
    }
}
