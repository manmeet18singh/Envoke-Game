using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrap : MonoBehaviour
{

    bool IsRoutineOn = false;
    List<GameObject> objects = new List<GameObject>();

   
    private void OnTriggerStay(Collider other)
    {

        if(!IsRoutineOn)
            StartCoroutine(LavaDamage(other));
 
    }

    private void OnTriggerExit(Collider other)
    {
        if (objects.Contains(other.gameObject))
        {
            objects.Remove(other.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!objects.Contains(other.gameObject))
        {
            objects.Add(other.gameObject);
        }
    
    }

    IEnumerator LavaDamage(Collider other)
    {
        IsRoutineOn = true;
        checkObjectLayer();
        yield return new WaitForSeconds(2f);
        IsRoutineOn = false;
    }

    void checkObjectLayer()
    {
        foreach(GameObject obj in objects)
        {
            if(obj == null)
            {
                objects.Remove(obj);
                return;
            }
            if (obj.gameObject.layer == 11)
            {
                obj.gameObject.GetComponent<EnemyHealth>().DamageOverTime(5, 5, AttackFlags.Fire);
            }
            else if (obj.gameObject.layer == 7)
            {
                obj.GetComponent<PlayerHealth>().DamageOverTime(5, 5, AttackFlags.Fire);
                AudioManager.instance.Play("LavaHit");
            }
        }
       

    }
}
