using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{

    public Animator animator;
    public int SpeedHash;

    public PlayerIdleState(FSMMgr _mgr , int _SpeedHash , Animator _animator) : base(_mgr)
    {
        m_statusID = StateID.NEW_PLAYER_IDLE;
        SpeedHash = _SpeedHash;
        animator = _animator;
    }

    public override void BeforeEnter()
    {
        base.BeforeEnter();

        //animator.SetFloat(SpeedHash,0);
    }

    public override void Update()
    {

    }

    public override void BreakCondition()
    {
        base.BreakCondition();

        Vector3 moveVec = InputMgr.GetInstance().GetMoveVec();
        //只要左右方向有东西按下，则切换状态
        if (Mathf.Abs(moveVec.x) > 0 || Mathf.Abs(moveVec.z) > 0)
        {
            fsmMgr.TransState(TransConditionID.NEW_PLAYER_WALK);
        }

    }
}
