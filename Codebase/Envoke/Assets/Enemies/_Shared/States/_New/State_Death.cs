using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Death : IState
{
    private Enemy mEnemy;

    public State_Death(Enemy _enemy)
    {
        mEnemy = _enemy;
    }

    public void OnEnter()
    {

        if (mEnemy.mAnimator != null)
        {
            //mEnemy.mAnimator.SetBool("Dead", true);
            mEnemy.mAnimator.SetTrigger("Dead");
        }
    }

    public void OnExit()
    {
        //if (mEnemy.mAnimator != null)
        //{
        //    mEnemy.mAnimator.SetBool("Dead", false);
        //}
    }

    public void Tick()
    {}
}
