using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandState : FSMStateBase {

    public CharacterController controller;
    public Animator animator;

    public StandState( FSMMgr _mgr ) : base(_mgr)
    {
        m_statusID = StateID.STATE_PLAYER_IDLE;
    }

    public override void Update()
    {

    }

    public override void BreakCondition()
    {
        Vector3 moveVec = InputMgr.GetInstance().GetMoveVec();
        //只要左右方向有东西按下，则切换状态
        if ( Mathf.Abs(moveVec.x) > 0 || Mathf.Abs(moveVec.z) > 0)
        {
            fsmMgr.TransState(TransConditionID.C_PLAYER_RUN);
            animator.SetBool("isRun", true);
            animator.Play("Run");
        }
    }

}
