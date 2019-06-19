using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerStateBase
{

    Camera m_camera;
    Vector3 last_inputVec;
    GameObject obj;

    Animator m_animator;
    int speedID = 0;        //Speed的HashId
    int rotateID = 0;

    float rotateSpeed;

    private GradualValue m_graduaVal;
    private float cur_speed = 0;

    int StartRunGradualSpeed;  //开始跑到最快的加速时间
    int StopRunGradualSpeed;    //开始停止跑到站立的加速时间

    public PlayerWalkState(FSMMgr _mgr , GameObject _obj, Camera _camera, Animator _animator ,
        float _rotateSpeed ,int _StartRunGradualSpeed , int _StopRunGradualSpeed, int _speedID , int _rotateID) : base(_mgr)
    {
        m_statusID = StateID.NEW_PLAYER_WALK;
        m_camera = _camera;
        obj = _obj;
        m_animator = _animator;
        rotateSpeed = _rotateSpeed;
        speedID = _speedID;
        rotateID = _rotateID;
        StartRunGradualSpeed = _StartRunGradualSpeed;
        StopRunGradualSpeed = _StopRunGradualSpeed;


        m_graduaVal = new GradualValue();
    }

    public override void Update()
    {
        //循环一个值更替
        m_graduaVal.Update();

        Vector3 inputVec = InputMgr.GetInstance().GetVecCamera(m_camera);
        if (inputVec != Vector3.zero)
        {
            last_inputVec = inputVec;
            //如果左右方向没有按下，则切换为站立

            Quaternion towardRotate = Quaternion.LookRotation(inputVec);
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, towardRotate, rotateSpeed);

            m_graduaVal.ClearByKey("speedDown");
        }

        //float f = Quaternion.Dot(q1,q2)
        //大于0则面对，否则则背对着

        //Debug.Log("## towardRotate : " + towardRotate.x + " ,  " + towardRotate.y + " ,  " + towardRotate.z + " ,  ");
        //float diff = Vector3.Cross(obj.transform.forward, towardRotate.eulerAngles).y;

        if (m_animator != null)
        {
            m_graduaVal.Start();
            cur_speed = m_graduaVal.AddGradualValue("speed", cur_speed, 3.4f, StartRunGradualSpeed);

            m_animator.SetFloat(speedID, cur_speed);

            /*if (diff > 0)
            {
                m_animator.SetFloat(rotateID, 15f);
            }
            else if(diff < 0)
            {
                m_animator.SetFloat(rotateID, -15f);
            }*/
        }

    }
    /// <summary>
    /// 跳出当前状态的循环函数（条件判断）
    /// </summary>
    public override void BreakCondition()
    {
        Vector3 moveVec = InputMgr.GetInstance().GetVecCamera(m_camera);
        //如果左右方向没有按下，则切换为站立
        if (Mathf.Abs(moveVec.x) == 0 && Mathf.Abs(moveVec.z) == 0)
        {
            m_graduaVal.ClearByKey("speed");
            if (m_animator != null)
            {
                //Debug.Log("####  cur_speed :   " + cur_speed);
                float speed = m_graduaVal.AddGradualValue("speedDown", cur_speed, 0, StopRunGradualSpeed);
                //进行一次设置之后，就将原本的跑步速度置为0
                cur_speed = speed;

                m_animator.SetFloat(speedID, speed);

                if (speed <= 0)
                {
                    //Debug.Log("####  speed :   " + speed);
                    fsmMgr.TransState(TransConditionID.NEW_PLAYER_IDLE);
                    m_graduaVal.Clear();
                }

            }
        }

        //如果有切换动作按键被按下，则会切换状态
        if (GetKeyTransState(InputMgr.GetInstance().GetCurKeyDown()) != TransConditionID.CONDITION_NULL)
        {
            fsmMgr.TransState(GetKeyTransState(InputMgr.GetInstance().GetCurKeyDown()));
            m_graduaVal.Clear();
        }
    }

}
