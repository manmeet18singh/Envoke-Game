using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStateDropBomb : IState
{
    // References
    private BomberGoblin mGoblin;
    private float mTimeToPlaceBomb;
    private GameObject mBomb;

    // State Variables
    private float mTimePlacingBomb;
    private bool mPlacedBomb;
    public bool BombPlaced { get => mPlacedBomb; }

    public GoblinStateDropBomb(BomberGoblin _goblin, float _timeToPlaceBomb, GameObject _bomb)
    {
        mGoblin = _goblin;
        mTimeToPlaceBomb = _timeToPlaceBomb;
        mBomb = _bomb;
    }

    public void OnEnter()
    {
        mTimePlacingBomb = 0;
        mPlacedBomb = false;
        mGoblin.mAnimator.SetTrigger("DropBomb");
    }

    public void OnExit()
    {
        mGoblin.mAnimator.ResetTrigger("DropBomb");
    }

    public void Tick()
    {
        mTimePlacingBomb += Time.deltaTime;

        if (!mPlacedBomb && mTimePlacingBomb > mTimeToPlaceBomb)
        {
            //mGoblin.BombPlaced();
            mPlacedBomb = true;
            Vector3 bombLocation = mGoblin.transform.position + (Random.insideUnitSphere * 1.5f);
            bombLocation.y += 0.5f;
            Vector3 initialPosition = new Vector3(mGoblin.transform.position.x, mGoblin.transform.position.y + 0.5f, mGoblin.transform.position.z);
            GameObject bomb = GameObject.Instantiate(mBomb, initialPosition, Quaternion.Euler(new Vector3(-90,0,0)));
            //bomb.GetComponentInChildren<BombExploder>().Goblin = mGoblin;
            GameManager.Instance.LerpObjectToPosition(bomb, bomb.transform.position, bombLocation, 1f);
        }
    }

    
}
