using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 外部函数调用都在该Control中调用并且包一层，保证所有子状态不会过度耦合
/// </summary>

public class PlayerControl : MonoBehaviour {

    public Camera m_camera;
    Animator m_animator;
    FSMMgr m_fsmmgr;
    FSMStateBase m_curState;

    public int StartRunGradualSpeed = 150;  //开始跑到最快的加速时间
    public int StopRunGradualSpeed = 150;    //开始停止跑到站立的加速时间

    public float jumpHitDistance = 6f;

    public int picHitDistance = 3;       //可捡起东西的最小距离

    public GameObject RighthandPickObj;      //右手物体附加OBJ
    public GameObject LefthandPickObj;       //左手物体附加OBJ

    public float playerHeight = 0;

    // Use this for initialization
    void Start () {
        if (m_camera == null)
            m_camera = Camera.main;

        m_animator = GetComponent<Animator>();

        playerHeight = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.y;

        InitState();
        
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 rayPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + playerHeight/3, gameObject.transform.position.z);
        Debug.DrawRay(rayPosition, (gameObject.transform.forward) * picHitDistance, Color.red);//绘制射线

        //更新状态
        if (m_fsmmgr != null)
            m_fsmmgr.UpdateState();
	}

    private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("LayerIndex : " + layerIndex);
        //获取手脚IK对应的位置
        /*Vector3 lhandPos = lhandCube.transform.position;
        lhandPos.y += lhandCube.GetComponent<BoxCollider>().bounds.size.y;
        Vector3 rhandPos = rhandCube.transform.position;
        rhandPos.y += rhandCube.GetComponent<BoxCollider>().bounds.size.y;
        //Vector3 lFootPos = leftFootCube.transform.position;
        //Vector3 rFootPos = rightFootCube.transform.position;

        //设置手脚试试
        m_anitator.SetIKPosition(AvatarIKGoal.LeftHand, lhandPos);
        m_anitator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

        m_anitator.SetIKRotation(AvatarIKGoal.LeftHand, lhandRotation);
        m_anitator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        m_anitator.SetIKPosition(AvatarIKGoal.RightHand, rhandPos);
        m_anitator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

        m_anitator.SetIKRotation(AvatarIKGoal.RightHand, rhandRotation);
        m_anitator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);*/
    }

    void InitState()
    {
        m_fsmmgr = new FSMMgr();

        PlayerPickState playerPickState = new PlayerPickState(m_fsmmgr, this, m_animator, Animator.StringToHash("bPickUp"),
            Animator.StringToHash("pickVal"), Animator.StringToHash("speed"), gameObject, picHitDistance , RighthandPickObj);
        playerPickState.AddCondition(TransConditionID.NEW_PLAYER_WALK, StateID.NEW_PLAYER_WALK);
        playerPickState.AddCondition(TransConditionID.NEW_PLAYER_IDLE, StateID.NEW_PLAYER_IDLE);

        PlayerJumpState playerJumpState = new PlayerJumpState(m_fsmmgr, m_animator, Animator.StringToHash("bJump"),
        gameObject, jumpHitDistance, Animator.StringToHash("JumpCurve"));
        playerJumpState.AddCondition(TransConditionID.NEW_PLAYER_WALK, StateID.NEW_PLAYER_WALK);

        PlayerIdleState playerIdleState = new PlayerIdleState(m_fsmmgr , Animator.StringToHash("speed"),m_animator);
        playerIdleState.AddCondition(TransConditionID.NEW_PLAYER_WALK, StateID.NEW_PLAYER_WALK);
        playerIdleState.AddCondition(TransConditionID.NEW_PLAYER_PICK, StateID.NEW_PLAYER_PICK);
        playerIdleState.AddKeyCondition((KeyCode)KeyEnum.KeyPick, TransConditionID.NEW_PLAYER_PICK , playerPickState.JudgeCanPickObj);

        PlayerWalkState playerWalkState = new PlayerWalkState(m_fsmmgr, gameObject, m_camera,
            m_animator, 0.04f, StartRunGradualSpeed , StopRunGradualSpeed, Animator.StringToHash("speed"), Animator.StringToHash("angle"));
        playerWalkState.AddCondition(TransConditionID.NEW_PLAYER_IDLE , StateID.NEW_PLAYER_IDLE);
        playerWalkState.AddCondition(TransConditionID.NEW_PLAYER_JUMP, StateID.NEW_PLAYER_JUMP);
        playerWalkState.AddCondition(TransConditionID.NEW_PLAYER_PICK, StateID.NEW_PLAYER_PICK);
        //添加跳跃按钮的状态
        playerWalkState.AddKeyCondition((KeyCode)KeyEnum.KeyJump, TransConditionID.NEW_PLAYER_JUMP , playerJumpState.JudgeCanJump);
        playerWalkState.AddKeyCondition((KeyCode)KeyEnum.KeyPick, TransConditionID.NEW_PLAYER_PICK , playerPickState.JudgeCanPickObj);


        /*PlayerPickState playerPickState = new PlayerPickState(m_fsmmgr, m_animator, Animator.StringToHash("bPickUp"),
            Animator.StringToHash("pickVal"), Animator.StringToHash("speed"));
        playerPickState.AddCondition(TransConditionID.NEW_PLAYER_WALK, StateID.NEW_PLAYER_WALK);
        playerPickState.AddCondition(TransConditionID.NEW_PLAYER_IDLE, StateID.NEW_PLAYER_IDLE);*/





        m_curState = playerIdleState;

        m_fsmmgr.Init(StateID.NEW_PLAYER_IDLE, playerIdleState);
        m_fsmmgr.AddState(StateID.NEW_PLAYER_WALK, playerWalkState);
        m_fsmmgr.AddState(StateID.NEW_PLAYER_JUMP, playerJumpState);
        m_fsmmgr.AddState(StateID.NEW_PLAYER_PICK, playerPickState);
    }

    //根据部位类型查找一个物体的IK点
    public IKPosRotation? GetIKPosRotationByType(GameObject obj , AvatarTarget _target )
    {
        InteractiveObjBase objBase = obj.GetComponentInChildren<InteractiveObjBase>();
        if (objBase != null)
            return objBase.GetIKPosByAvatarTarget(_target);

        return null;
    }

    /// <summary>
    /// 启用layer层并且设定所需VALUE值（设置特定动作）
    /// </summary>
    /// <param name="_layerName">需要开启的层级name</param>
    /// <param name="weight">需要开启的层级权重</param>
    public void OpenPartMask( string _layerName, float _weight = 1)
    {
        SetLayerWeiMask(m_animator.GetLayerIndex(_layerName) , _weight);
    }
    /// <summary>
    /// 启用layer层并且设定所需VALUE值（设置特定动作）
    /// </summary>
    /// <param name="_layerName">需要开启的层级name</param>
    /// <param name="weight">需要开启的层级权重</param>
    public void OpenPartMask(int _layerIndex ,float _weight = 1)
    {
        SetLayerWeiMask(_layerIndex, _weight);
    }
    void SetLayerWeiMask( int _layerIndex , float _weight = 1)
    {
        m_animator.SetLayerWeight(_layerIndex, _weight);
    }

}
