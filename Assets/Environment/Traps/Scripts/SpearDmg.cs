using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearDmg : MonoBehaviour
{

    
    private PlayerHealth mPlayerHealth;
   

    private void Awake()
    {    
        mPlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mPlayerHealth.TakeDamage(15, 0);
            Destroy(gameObject);
            AudioManager.instance.Play("SpearTrapHit");
        }
        else if(other.gameObject.layer == 14 || other.gameObject.layer == 8)
        {
            Destroy(gameObject); 
        }
       
    }

    public void Death()
    {
        Destroy(gameObject);
    }

}

