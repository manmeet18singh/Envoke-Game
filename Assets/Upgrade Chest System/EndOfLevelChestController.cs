using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelChestController : MonoBehaviour
{
    [SerializeField]
    Animator anim = null;

    private void Awake()
    {
        GameManager.Instance.onRoomCompleted += DropChest;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onRoomCompleted -= DropChest;
    }

    void DropChest()
    {
        anim.enabled = true;
    }

    public void DisableAnimator()
    {
        anim.enabled = false;
    }
}
