using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject DestroyedVersion;
    [SerializeField] private string[] mSfx = null;

    private void OnTriggerEnter(Collider other)
    {

        //checks if object is on the PlayerSpells layer
        if (other.gameObject.layer == 9)
        {
            GameManager.Instance.SpawnPickup(GameManager.Instance.GetRandomPickup(), transform.position, transform.position - Vector3.forward, 1f);
            Instantiate(DestroyedVersion, transform.position, transform.rotation);
           
            Destroy(gameObject);
            AudioManager.PlayRandomSFX(mSfx);
        }
    }


}
