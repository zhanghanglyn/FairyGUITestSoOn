using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    CharacterController characterController;
    Animator m_anitator;
    public Camera _camera;

    FSMMgr m_fsmmgr;
    FSMStateBase cur_state;

    //IK LOOKAT Test
    public Vector3 LookAtPosition = new Vector3(0,0,0);
    public GameObject lhandCube;
    public GameObject rhandCube;
    public GameObject leftFootCube;
    public GameObject rightFootCube;

    public Quaternion lhandRotation;
    public Quaternion rhandRotation;
    public Quaternion lfootRotation;
    public Quaternion rfootRotation;

    public float rotateSpeed = 0.1f;
    public float _speed = 0.02f;
    public float accelerateSpeed = 0.003f;

    // Use this for initialization
    void Start () {
        m_anitator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        InitState();
    }
	
	// Update is called once per frame
	void Update () {

        //更新状态
        if (m_fsmmgr != null)
            m_fsmmgr.UpdateState();


    }

    void InitState()
    {
        m_fsmmgr = new FSMMgr();

        //添加玩家站立状态
        StandState standState = new StandState(m_fsmmgr);
        standState.AddCondition(TransConditionID.C_PLAYER_RUN, StateID.STATE_PLAYER_RUN);
        standState.animator = m_anitator;
        standState.controller = characterController;

        //添加玩家跑步状态
        PlayerRunState playerRunState = new PlayerRunState(m_fsmmgr);
        playerRunState.AddCondition( TransConditionID.C_PLAYER_IDLE , StateID.STATE_PLAYER_IDLE );
        playerRunState.animator = m_anitator;
        playerRunState.controller = characterController;
        playerRunState.camera = _camera;
        playerRunState.rotateSpeed = rotateSpeed;
        playerRunState.obj = gameObject;
        playerRunState.speed = _speed;
        playerRunState.accelerateSpeed = accelerateSpeed;
        playerRunState.m_curSpeed = 0;

        m_fsmmgr.Init(StateID.STATE_PLAYER_IDLE, standState);
        m_fsmmgr.AddState(StateID.STATE_PLAYER_RUN, playerRunState);

        cur_state = standState;

        //给PlayerRunState注册一个动画回调
        AnimationCallMgr.GetInstance().RegistExitCall(m_anitator,  playerRunState.RunAnimationPlayerOver);
        AnimationCallMgr.GetInstance().RegistEnterCall(m_anitator, playerRunState.RunAnimationPlayerStart);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //m_anitator.SetLookAtPosition(LookAtPosition);
        //m_anitator.SetLookAtWeight(1);


        //获取手脚IK对应的位置
        Vector3 lhandPos = lhandCube.transform.position;
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
        m_anitator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    }
}
