using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PausePlayerInput();
            InputManager.controls.Disable();
            GameManager.Instance.ChangeRoom();
        }
    }

}
