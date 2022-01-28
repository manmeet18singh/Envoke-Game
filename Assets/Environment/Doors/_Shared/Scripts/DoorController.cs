using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    Animator mAnimator = null;

    private void Start()
    {
        GameManager.Instance.onRoomCompleted += OpenDoor;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onRoomCompleted -= OpenDoor;
    }

    void OpenDoor()
    {
        mAnimator.enabled = true;
    }
}
