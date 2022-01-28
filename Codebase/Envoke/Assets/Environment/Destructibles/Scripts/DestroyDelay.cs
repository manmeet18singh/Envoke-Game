using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{


    [SerializeField]
    float TimeBeforeDestroy = 5f;

    private void Awake()
    {
        StartCoroutine(DelayedDestroy());

    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(TimeBeforeDestroy);
        Destroy(gameObject);

    }
}
