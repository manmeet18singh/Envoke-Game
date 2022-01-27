using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    Transform mPlayerTrans;
    Vector3 mTarget;
    Quaternion mTargetRot;


    private void Start()
    {
        mPlayerTrans = GameManager.Instance.mPlayer.transform;
    }


    private void LateUpdate()
    {
        mTarget = new Vector3(mPlayerTrans.position.x, transform.position.y, mPlayerTrans.position.z) - transform.position;
        mTargetRot = Quaternion.LookRotation(mTarget, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, mTargetRot, Time.deltaTime * 2.0f);
    }
}
