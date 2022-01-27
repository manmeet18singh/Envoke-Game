using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    
    public GameObject Reloadable;
    public GameObject SpearLauncher;
    public bool ReloadOn;

    private GameObject Spear;
    private bool IsReloading;
    private Vector3 SpearPos;

   
    private void OnTriggerStay(Collider other)
    {
        FireCheck(other);
    }
    private void OnTriggerEnter(Collider other)
    {
        FireCheck(other);
    }

    private void Awake()
    {
        SpearPos = SpearLauncher.transform.position;
        IsReloading = false;
        Spear = Instantiate(Reloadable, SpearPos, transform.rotation, gameObject.transform);
    } 

    private IEnumerator Reload()
    {
        IsReloading = true;
        yield return new WaitForSeconds(3f);
        Spear = Instantiate(Reloadable, SpearPos, transform.rotation, gameObject.transform);
        Spear.GetComponent<BoxCollider>().enabled = false;
        IsReloading = false;
        AudioManager.instance.Play("SpearTrapReload");

    }

     private void FireCheck(Collider other)
    {
        if (other.tag == "Player" && IsReloading == false)
        {
            Spear.GetComponent<BoxCollider>().enabled = true;      
            Spear.GetComponent<Animation>().Play();
            

            if (IsReloading != true && ReloadOn)
                StartCoroutine(Reload());
        }
    }
}
