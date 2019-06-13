using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : FSMStateBase {

    public CharacterController controller;
    public Animator animator;
    public GameObject obj;

    public float speed = 0.01f;
    public Camera camera;
    public float rotateSpeed = 0.1f;

    public float m_curSpeed = 0;
    public float accelerateSpeed = 0.003f;

    private Vector3 last_inputVec;  //会保存最后一个的输入
    bool bStart = false;

    public PlayerRunState(FSMMgr _mgr) : base(_mgr)
    {
        m_statusID = StateID.STATE_PLAYER_RUN;
    }

    public override void Update()
    {
        //if (bStart == false)
            //return;
        Vector3 inputVec = InputMgr.GetInstance().GetVecCamera(camera);
        if (inputVec != Vector3.zero)
            last_inputVec = inputVec;
        //如果左右方向没有按下，则切换为站立
        if (controller!= null)
        {
            //设置转向以及移动  //添加一个加速度
            if (m_curSpeed < speed)
                m_curSpeed += accelerateSpeed;
            else
                m_curSpeed = speed;

            controller.Move(inputVec * m_curSpeed);

            Quaternion towardRotate = Quaternion.LookRotation(inputVec);
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, towardRotate, rotateSpeed);
        }
    }

    public override void BreakCondition()
    {
        Vector3 moveVec = InputMgr.GetInstance().GetVecCamera(camera);
        //如果左右方向没有按下，则切换为站立
        if (Mathf.Abs(moveVec.x) == 0 && Mathf.Abs(moveVec.z) == 0)
        {
            fsmMgr.TransState(TransConditionID.C_PLAYER_IDLE);  //这一步交给动画回调来完成
            animator.SetBool("isRun", false);
            animator.Play("Stand");
        }
    }

    /// <summary>
    /// 动画播放完毕的回调
    /// </summary>
    public void RunAnimationPlayerOver(AnimatorStateInfo animatorStateInfo)
    {
        Debug.Log("RunAnimationPlayerOver");
    }

    public void RunAnimationPlayerStart(AnimatorStateInfo animatorStateInfo)
    {
        Debug.Log("RunAnimationPlayerStart");
    }
}
