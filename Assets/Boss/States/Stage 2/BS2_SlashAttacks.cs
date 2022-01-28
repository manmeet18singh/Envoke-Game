using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS2_SlashAttacks : BossState
{
    public bool HasFinishedSlashing { get => mAnimationComplete; set => mAnimationComplete = value; }
    private bool mAnimationComplete;

    public BS2_SlashAttacks(FinalBoss _boss) : base(_boss, "State_FrenzySlashAttacks")
    { }

    public override void OnEnter()
    {
        base.OnEnter();
        mBoss.mAnimator.SetTrigger("SlashingT");
        Vector3 playerPos = GameManager.Instance.mPlayer.transform.position;
        mBoss.transform.LookAt(new Vector3(playerPos.x, mBoss.transform.position.y, playerPos.z));
        mBoss.EnableSword(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mAnimator.ResetTrigger("SlashingT");
        mAnimationComplete = false;
        mBoss.EnableSword(false);
    }

}