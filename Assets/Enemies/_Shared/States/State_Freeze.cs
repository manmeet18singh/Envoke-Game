using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Freeze : IState
{
    private StateMachine mStateMachine;
    private float mDuration;
    private IState mNextState;
    private Animator mAnim;
    //bool mStateSet;

    //TODO: Might needed frozen animation
    public State_Freeze(StateMachine _stateMachine, IState _nextState, Animator _anim, float _duration)
    {
        mStateMachine = _stateMachine;
        mDuration = _duration;
        mNextState = _nextState;
        mAnim = _anim;
    }

    public void OnEnter()
    {
        //mStateSet = false;
        if(mAnim != null)
        {
            mAnim.enabled = false;
        }
    }
    public void Tick()
    {
        mDuration -= Time.deltaTime;

        if(mDuration <= 0)
        {
            //mStateSet = true;
            mStateMachine.SetState(mNextState);
        }
    }

    public void OnExit()
    {
        if (mAnim != null)
        {
            mAnim.enabled = true;
        }
    }

}
