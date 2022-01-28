using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenCamera : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.mDoorCamPos = transform.position;
        GameManager.Instance.mDoorCamRot = transform.rotation;
        Destroy(gameObject);
    }
}
