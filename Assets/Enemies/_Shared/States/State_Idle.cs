using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : IState

{
    private readonly Enemy mEnemy;
    private readonly EnemySettings mEnemySettings;


    public State_Idle(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;
    }

    public void OnEnter()
    {
        //mEnemy.mIsAttacking = false;
        //mEnemy.StartCoroutine(mEnemy.IdleForSomeTime());
        // idle animation stuff here
    }

    public void Tick() { }
    public void OnExit() { }
}
