using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickState : PlayerStateBase {

    PlayerControl m_playerControl;

    Animator m_animator;
    int pickPosHash;
    int picVal;
    int speedHash;
    GameObject gameObject;

    private GameObject pickObj;

    private GameObject _RighthandPickObj;      //右手物体附加OBJ

    int pickRayDis = 1;         //判断周边范围是否有可拾取物体的距离
    private float playerHeight = 0;
    GradualValue m_graduaVal;
    float weightSpeed = 0;

    public PlayerPickState( FSMMgr _mgr , PlayerControl _control, Animator _animator , int _pickPosHash ,int _picVal ,int _SpeedHash ,
        GameObject _gameObject , int _pickRayDis , GameObject RighthandPickObj) : base (_mgr)
    {
        m_playerControl = _control;
        m_animator = _animator;
        pickPosHash = _pickPosHash;
        picVal = _picVal;
        speedHash = _SpeedHash;
        gameObject = _gameObject;
        pickRayDis = _pickRayDis;
        _RighthandPickObj = RighthandPickObj;

        //计算一个人物的高度存储
        playerHeight = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.y;
        //
        m_graduaVal = new GradualValue();
    }

    public override void BeforeEnter()
    {
        m_animator.SetBool(pickPosHash , true);

        AnimationCallMgr.GetInstance().RegistExitCall(m_animator, this.PickAnimationPlayOver);
    }

    public override void Update()
    {
        //向着物体的右边marge点去进行查找
        if (pickObj != null)
        {
            IKPosRotation? iKPosRotation = m_playerControl.GetIKPosRotationByType(pickObj , AvatarTarget.RightHand);
            if (iKPosRotation != null)
            {
                m_animator.MatchTarget(((IKPosRotation)iKPosRotation).pos, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0.28f, 0.69f);
            }
        }

        //当动画运行到捡起这个动作时间时，将碰撞到的物体吸附在手部放置物体的位置
        if ( m_animator.GetFloat(picVal) >= 0.2f && pickObj != null && _RighthandPickObj != null)
        {
            pickObj.transform.position = _RighthandPickObj.transform.position;
            pickObj.transform.rotation = _RighthandPickObj.transform.rotation;
            pickObj.transform.parent = _RighthandPickObj.transform;
        }
        if (m_animator.GetFloat(picVal) >= 0.25f && pickObj != null && _RighthandPickObj != null)
        {
            //添加一个逐渐增加层级的变量
            m_graduaVal.Start();
            weightSpeed = m_graduaVal.AddGradualValue("weight", (int)weightSpeed, 1, 50000);
        }

        //循环一个值更替
        m_graduaVal.Update();
    }

    public override void BreakCondition()
    {
        if (weightSpeed >= 1)
        {
            fsmMgr.TransState(TransConditionID.NEW_PLAYER_IDLE);
            m_graduaVal.Clear();
            //并且设置一个手部IK位置

        }
        else
            m_playerControl.OpenPartMask(1,weightSpeed);
    }

    public override void BeforeExit()
    {
        base.BeforeExit();
        m_playerControl.OpenPartMask(1);
    }

    public void PickAnimationPlayOver(AnimatorStateInfo animatorStateInfo)
    {
        m_animator.SetBool(pickPosHash, false);
        m_animator.SetFloat(speedHash, 0); 
        //并不在此退出，而是等举起层级慢慢合并之后在退出
        //fsmMgr.TransState(TransConditionID.NEW_PLAYER_IDLE);
        AnimationCallMgr.GetInstance().DeleteExitCall(m_animator, this.PickAnimationPlayOver);

        //添加一个逐渐增加层级的变量
        //m_graduaVal.Start();
        //weightSpeed = m_graduaVal.AddGradualValue("weight", (int)weightSpeed, 1, 50000);
    }

    //判断是否可以触发捡起动作,用来在捡起按键按下时触发
    public bool JudgeCanPickObj()
    {
        RaycastHit pickObjCastHit = new RaycastHit();
        //判断是否有触碰的物体并且该物体为可以互动的TAG
        
        Vector3 rayPosition = new Vector3( gameObject.transform.position.x, gameObject.transform.position.y + playerHeight / 3, gameObject.transform.position.z);
        Debug.DrawRay(rayPosition, (gameObject.transform.forward) * pickRayDis, Color.red);//绘制射线
        if (Physics.Raycast(rayPosition, gameObject.transform.forward, out pickObjCastHit, pickRayDis))
        {
            if (pickObjCastHit.collider.tag == "InteractiveObj")
            {
                pickObj = pickObjCastHit.transform.gameObject;
                return true;
            }
        }

        return false;
    }
}
