using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 与AnimationCallMgr配合使用，添加到所有动画中的回调，会分发给所有注册过的动画机
/// </summary>
public class AnimationBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        AnimationCallMgr.GetInstance().EnterCall(animator, animatorStateInfo);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        AnimationCallMgr.GetInstance().UpdateCall(animator, animatorStateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        AnimationCallMgr.GetInstance().ExitCall(animator, animatorStateInfo);
    }

}
