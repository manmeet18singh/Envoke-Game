using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{


    private PlayerHealth mPlayerHealth;
    private Collider col;

    public GameObject Spikes;



    private void Awake()
    {
        
        mPlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            col = other;
            Spikes.GetComponent<Animation>().Play();
            mPlayerHealth.TakeDamage(15, 0);
            AudioManager.instance.Play("SpikeTrapHit");
            StartCoroutine(PlayerStandingOnTrap());
        }       
    }
    

    IEnumerator PlayerStandingOnTrap()
    {
        while (col.tag == "Player")
        {
            yield return new WaitForSeconds(1f);
            if (col.tag == "Player")
            {
                Spikes.GetComponent<Animation>().Play();
                mPlayerHealth.TakeDamage(15, 0);
                AudioManager.instance.Play("SpikeTrapHit");
            }          
        }     
    }


}
