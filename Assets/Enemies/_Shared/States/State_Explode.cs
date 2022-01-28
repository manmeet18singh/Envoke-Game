using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Explode : StateAttacking
{
    private readonly EnemySettings mEnemySettings;


    public State_Explode(Enemy enemy, EnemySettings settings) : base(enemy, StateAttacking.DefaultAnimationName)
    {
        mEnemySettings = settings;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        mEnemy.mEnemyAttackBehavior.Attack();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Tick()
    {

    }

}
