using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public Camera m_camera;
    Animator m_animator;
    FSMMgr m_fsmmgr;
    FSMStateBase m_curState;

    public int StartRunGradualSpeed = 150;  //开始跑到最快的加速时间
    public int StopRunGradualSpeed = 150;    //开始停止跑到站立的加速时间

    // Use this for initialization
    void Start () {
        if (m_camera == null)
            m_camera = Camera.main;

        m_animator = GetComponent<Animator>();

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

        PlayerIdleState playerIdleState = new PlayerIdleState(m_fsmmgr);
        playerIdleState.AddCondition(TransConditionID.NEW_PLAYER_WALK, StateID.NEW_PLAYER_WALK);

        PlayerWalkState playerWalkState = new PlayerWalkState(m_fsmmgr, gameObject, m_camera,
            m_animator, 0.04f, StartRunGradualSpeed , StopRunGradualSpeed, Animator.StringToHash("speed"), Animator.StringToHash("angle"));
        playerWalkState.AddCondition(TransConditionID.NEW_PLAYER_IDLE , StateID.NEW_PLAYER_IDLE);

        m_curState = playerIdleState;

        m_fsmmgr.Init(StateID.NEW_PLAYER_IDLE, playerIdleState);
        m_fsmmgr.AddState(StateID.NEW_PLAYER_WALK, playerWalkState);

    }
}
