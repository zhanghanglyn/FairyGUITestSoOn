using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerStateBase {

    public Animator animator;
    private int jumpHash;

    public PlayerJumpState(FSMMgr _mgr , Animator _animator , int _jumpHash) : base(_mgr)
    {
        this.animator = _animator;
        jumpHash = _jumpHash;
        m_statusID = StateID.NEW_PLAYER_JUMP;

        AnimationCallMgr.GetInstance().RegistExitCall(animator, this.JumpAnimationPlayOver);
        AnimationCallMgr.GetInstance().RegistEnterCall(animator, this.JumpAnimationPlayEnter);
    }

    public override void Update()
    {
        animator.SetBool(jumpHash,true);
    }

    public override void BreakCondition()
    {
        
    }

    public void JumpAnimationPlayOver(AnimatorStateInfo animatorStateInfo)
    {
        animator.SetBool(jumpHash, false);
        fsmMgr.TransState(TransConditionID.NEW_PLAYER_WALK);
        AnimationCallMgr.GetInstance().DeleteExitCall( animator, this.JumpAnimationPlayOver);
    }

    //开始进入动作时回调
    public void JumpAnimationPlayEnter(AnimatorStateInfo animatorStateInfo)
    {
        //向前发射射线，并且获取射线碰撞的第一个collider，设置motionTarget位置

    }

}
