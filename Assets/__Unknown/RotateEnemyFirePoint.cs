using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnemyFirePoint : MonoBehaviour
{
    private Transform mParentTransform;

    private void Start()
    {
        mParentTransform = GetComponentInParent<Transform>();
    }

    private void Update()
    {
        transform.LookAt(GameManager.Instance.mPlayer.transform.position);
    }
}
