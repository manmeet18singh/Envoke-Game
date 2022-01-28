using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : IState
{
    private readonly string mStateName;
    public FinalBoss mBoss;

    public BossState(FinalBoss _boss, string _stateName)
    {
        mBoss = _boss;
        mStateName = _stateName;
    }

    public virtual void OnEnter()
    {
#if UNITY_EDITOR
        Debug.Log("Entering boss state: " + mStateName);
#endif
    }

    public virtual void OnExit()
    {
#if UNITY_EDITOR
        Debug.Log("Exiting boss state: " + mStateName);
#endif
    }

    public virtual void Tick()
    {}
}
