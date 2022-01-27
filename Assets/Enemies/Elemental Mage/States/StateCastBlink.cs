using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCastBlink : EnemyState
{
    public static readonly string DefaultAnimationName = "";

    #region Properties
    private readonly float mCooldown;
    #endregion

    #region State Variables
    private float mTimeOnCooldown = 0;
    #endregion

    public bool BlinkOnCooldown { get => mTimeOnCooldown >= Time.deltaTime; }

    public StateCastBlink(Enemy _enemy, string _animationName = null, float _cooldown = 6) : base(_enemy, _animationName == null ? DefaultAnimationName : _animationName)
    {
        mCooldown = _cooldown;
    }

    public override void Tick()
    {
        // TODO - cast blink spell, and afterwards switch back to mNextState

        // TODO - set cooldown time for when blink can be cast again
        if (mTimeOnCooldown == 0)
            mTimeOnCooldown = Time.deltaTime + mCooldown;
    }

    public override void OnEnter()
    {
        mTimeOnCooldown = 0;
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
