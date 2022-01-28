using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlinkKnockback : MonoBehaviour
{
    [SerializeField]
    private LayerMask mEnemies = 0;

    private BlinkData mSpellData;

    [SerializeField]
    private string[] mHitSFX;

    private Transform mPlayer;

    private void Awake()
    {
        mPlayer = GameManager.Instance.mPlayer.transform;
    }

    public void Initialize(BlinkData _data)
    {
        mSpellData = _data;
        StartCoroutine(ApplyKnockback());
    }


    IEnumerator ApplyKnockback()
    {
        Collider[] colliders = Physics.OverlapSphere(mPlayer.position, mSpellData.DamageRadius, mEnemies);
        foreach (Collider collider in colliders)
        {
            NavMeshAgent navAgent;
            navAgent = collider.gameObject.GetComponent<NavMeshAgent>();
            navAgent.speed = 10;
            navAgent.angularSpeed = 0;
            navAgent.acceleration = 20;
        }

        yield return new WaitForSeconds(.2f);

        foreach (Collider collider in colliders)
        {
            NavMeshAgent navAgent;
            navAgent = collider.gameObject.GetComponent<NavMeshAgent>();
            navAgent.speed = 3;
            navAgent.angularSpeed = 180;
            navAgent.acceleration = 10;
        }
    }
}
