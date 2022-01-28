using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS1_CastIceTorrent : BossState
{
    public BS1_CastIceTorrent(FinalBoss _boss) : base(_boss, "State_CastIceTorrent")
    { }

    public override void OnEnter()
    {
        base.OnEnter();
        mBoss.mIceTorrent.ResetSpell();
        mBoss.transform.LookAt(GameManager.Instance.mPlayer.transform);
        mBoss.mAnimator.SetTrigger("IceTorrentT");
    }

    public override void OnExit()
    {
        base.OnExit();
        mBoss.mAnimator.ResetTrigger("IceTorrentT");
        mBoss.mIceTorrent.CancelCast();
        mBoss.mTimeSinceLastCastIceTorrent = 0;
    }
}
