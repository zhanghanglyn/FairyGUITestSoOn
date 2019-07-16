using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerJumpState : PlayerStateBase {

    GameObject gameObj;
    public Animator animator;
    private int jumpHash;

    private RaycastHit m_hit;
    public float RayDistance;

    private Vector3 handPos;
    private bool matchTag = false;

    //曲线动画下降所影响的值
    private int jumpCurveHash = 0;

    public PlayerJumpState(FSMMgr _mgr , Animator _animator , int _jumpHash , GameObject _obj ,
        float _RayDistance , int _jumpCurveHash) : base(_mgr)
    {
        gameObj = _obj;
        this.animator = _animator;
        jumpHash = _jumpHash;
        m_statusID = StateID.NEW_PLAYER_JUMP;
        RayDistance = _RayDistance;
        jumpCurveHash = _jumpCurveHash;
    }

    public override void BeforeEnter()
    {
        AnimationCallMgr.GetInstance().RegistExitCall(animator, this.JumpAnimationPlayOver);
        animator.SetBool(jumpHash, true);
        //向前发射射线，并且获取射线碰撞的第一个collider，设置motionTarget位置
        if (Physics.Raycast(gameObj.transform.position, gameObj.transform.forward, out m_hit, RayDistance))
        {
            //如果是与环境物体互动
            if (m_hit.collider.tag == "EnvironmentInteraction")
            {
                handPos = new Vector3(m_hit.point.x , m_hit.collider.bounds.size.y , m_hit.point.z);

                //animator.MatchTarget(handPos, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(new Vector3(1,1,1),0) , 0.1f , 0.4f);
                matchTag = true;

                Debug.Log(" POS : " + handPos);

                gameObj.GetComponent<CharacterController>().enabled = false;
            }
        }

        

    }

    public override void Update()
    {

        //判断曲线修改的值 曲线应该是让被跳跃的物体boxcollider为不可见才对,有多个物体跳不过也是正常的
        if (matchTag == true)
            animator.MatchTarget(handPos, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0.01f, 0.4f);

        //在曲线下滑的时候，就应该设置Controller为true了
        if (animator.GetFloat(jumpCurveHash) >= 0.1f)
        {
            gameObj.GetComponent<CharacterController>().enabled = true;
        }

    }

    public override void BreakCondition()
    {
        
    }

    public void JumpAnimationPlayOver(AnimatorStateInfo animatorStateInfo)
    {
        animator.SetBool(jumpHash, false);
        fsmMgr.TransState(TransConditionID.NEW_PLAYER_WALK);
        AnimationCallMgr.GetInstance().DeleteExitCall( animator, this.JumpAnimationPlayOver);
        gameObj.GetComponent<CharacterController>().enabled = true;
        matchTag = false;
    }

    //判断是否可以触发跳过动作,用来在捡起按键按下时触发
    public bool JudgeCanJump()
    {
        RaycastHit pickObjCastHit = new RaycastHit();
        //判断是否有触碰的物体并且该物体为可以互动的TAG
        if (Physics.Raycast(gameObj.transform.position, gameObj.transform.forward, out pickObjCastHit, RayDistance))
        {
            //如果是与环境物体互动
            if (pickObjCastHit.collider.tag == "EnvironmentInteraction")
            {
                return true;
            }
        }

        return false;
    }
}
