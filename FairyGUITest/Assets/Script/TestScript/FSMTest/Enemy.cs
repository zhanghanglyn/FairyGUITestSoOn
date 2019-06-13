using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class Enemy : MonoBehaviour {

    public GameObject playerObj;
    public Camera camera;

    FSMMgr m_FSMMgr;
    EnemyCruiseState m_enemyCruiseState;
    EnemyChaseState m_enemyChaseState;
    EnemyBackState m_enemyBackState;

    //UI相关
    UIPanel m_uipanel;
    GameObject m_uiObj;
    GComponent m_talkCtl;
    GTextField m_wordCtl;
    float offset_y = 1f;

    string word_ctl_name = "word";
    float _showTime = 0;

    // Use this for initialization
    void Start () {
        Init();
        InitUI();

    }
	
	// Update is called once per frame
	void Update () {
        m_FSMMgr.UpdateState();

        if (m_talkCtl != null && _showTime != 0)
        {
            _showTime += Time.deltaTime;
            if (_showTime > 4)
            {
                m_talkCtl.visible = false;
                _showTime = 0;
            }
        }

        //SetCtlTowardCamera();
    }

    public void Init()
    {
        m_FSMMgr = new FSMMgr();

        //敌人巡逻状态
        m_enemyCruiseState = new EnemyCruiseState(m_FSMMgr);
        m_enemyCruiseState.enemyObj = this.gameObject;
        m_enemyCruiseState.SetInitPos(this.gameObject.transform.position);
        m_enemyCruiseState.playerObj = playerObj;
        m_enemyCruiseState.AddCondition(TransConditionID.C_CRUISE_T_CHASE, StateID.STATE_CHASE);

        //敌人追击状态
        m_enemyChaseState = new EnemyChaseState(m_FSMMgr);
        m_enemyChaseState.enemyObj = this.gameObject;
        m_enemyChaseState.playerObj = playerObj;
        m_enemyChaseState.AddCondition(TransConditionID.C_BACK_T_INIT, StateID.STATE_BACK);
        m_enemyChaseState.AddEnterCall(SetTalk);

        //敌人返回原位状态
        m_enemyBackState = new EnemyBackState(m_FSMMgr);
        m_enemyBackState.enemyObj = this.gameObject;
        m_enemyBackState.playerObj = playerObj;
        m_enemyBackState.m_startPos = this.gameObject.transform.position;
        m_enemyBackState.AddCondition(TransConditionID.C_INIT_TO_CRUISE, StateID.STATE_CRUISE);
        m_enemyBackState.AddCondition(TransConditionID.C_BACK_T_CHASE, StateID.STATE_CHASE);


        m_FSMMgr.Init(StateID.STATE_CRUISE, m_enemyCruiseState);
        m_FSMMgr.AddState(StateID.STATE_CHASE, m_enemyChaseState);
        m_FSMMgr.AddState(StateID.STATE_BACK, m_enemyBackState);
    }

    public void InitUI()
    {
        m_uiObj = gameObject.transform.Find("TalkBG").gameObject;
        m_uipanel = gameObject.GetComponentInChildren<UIPanel>();
        if (m_uipanel != null && m_uipanel.ui != null)
        {
            m_talkCtl = m_uipanel.ui;
            m_wordCtl = m_talkCtl.GetChild(word_ctl_name) as GTextField;
            GImage aa = m_talkCtl.GetChild("bg") as GImage;
            m_talkCtl.visible = false;

            /*float width = m_talkCtl.width;
            float height = m_talkCtl.height;

            if (m_uiObj != null)
            {
                m_uiObj.transform.position = new Vector3(m_uiObj.transform.position.x - width/2,
                    m_uiObj.transform.position.y + offset_y + height,
                    m_uiObj.transform.position.z );
            }*/
        }
    }

    public void SetTalk( string _word )
    {
        if (m_talkCtl != null && m_wordCtl != null && !string.IsNullOrEmpty(_word) )
        {
            m_talkCtl.visible = true;
            m_wordCtl.text = _word;
            _showTime = 0.01f;
        }
    }

    //设置board永远朝向摄像机
    public void SetCtlTowardCamera()
    {
        if (m_uiObj != null && camera != null)
        {
            Vector3 forward = camera.transform.position - m_uiObj.transform.position;
            Quaternion qua = Quaternion.LookRotation(forward);
            m_uiObj.transform.rotation = qua;

        }
    }

}
