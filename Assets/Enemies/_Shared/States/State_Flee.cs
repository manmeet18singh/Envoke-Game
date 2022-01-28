using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Flee : IState
{
    private readonly Enemy mEnemy;
    private readonly EnemySettings mEnemySettings;


    public State_Flee(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;
    }


    public void Tick()
    {
        throw new System.NotImplementedException();
    }
    public void OnEnter()
    {

    }

    public void OnExit()
    {
        
    }




}
