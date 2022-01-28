using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FindCover : IState
{
    private readonly Enemy mEnemy;
    private readonly EnemySettings mEnemySettings;


    public State_FindCover(Enemy enemy, EnemySettings settings)
    {
        mEnemy = enemy;
        mEnemySettings = settings;
    }

    public void Tick()
    {
        
    }
    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }
}
