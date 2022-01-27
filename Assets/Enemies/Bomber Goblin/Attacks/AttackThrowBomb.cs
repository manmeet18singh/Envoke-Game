using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackThrowBomb : MonoBehaviour
{
    [Header("Attack Properties")]
    [SerializeField] private float mThrowOffset = 1.5f;
    [SerializeField] private float mThrowTime = 1f;

    [Header("Model Correction Properties")]
    [SerializeField] private float mBombYOffset = 0.6f;
    [SerializeField] private Vector3 mBombRotationEuler = new Vector3(-90, 0, 0);

    [Header("References")]
    [SerializeField] BomberGoblin mGoblin;
    [SerializeField] GameObject mBomb;

    public void Attack()
    {
        Vector3 bombLocation = GameManager.Instance.mPlayer.transform.position + (Random.insideUnitSphere * mThrowOffset);
        bombLocation.y += mBombYOffset;

        Vector3 initialPosition = new Vector3(mGoblin.transform.position.x, mGoblin.transform.position.y + mBombYOffset, mGoblin.transform.position.z);
        GameObject bomb = GameObject.Instantiate(mBomb, initialPosition, Quaternion.Euler(mBombRotationEuler));
        bomb.GetComponentInChildren<BombExploder>().BombTriggered();
        GameManager.Instance.LerpObjectToPosition(bomb, bomb.transform.position, bombLocation, mThrowTime);
    }
}
