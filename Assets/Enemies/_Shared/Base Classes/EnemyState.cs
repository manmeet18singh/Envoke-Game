using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : IState
{
    // References
    protected readonly Enemy mEnemy;
    protected readonly string mAnimationBool;

    public EnemyState(Enemy _enemy, string _animationBool = "")
    {
        mEnemy = _enemy;
        mAnimationBool = _animationBool;
    }

    virtual public void OnEnter()
    {
        if (mAnimationBool != "")
            mEnemy.mAnimator.SetBool(mAnimationBool, true);
    }

    virtual public void OnExit()
    {
        if (mAnimationBool != "")
            mEnemy.mAnimator.SetBool(mAnimationBool, false);
    }

    public abstract void Tick();
}
