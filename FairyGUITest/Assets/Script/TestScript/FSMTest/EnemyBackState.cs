using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回到原位
/// </summary>
public class EnemyBackState : FSMStateBase {

    public GameObject enemyObj;
    protected CapsuleCollider enemyCollider;

    public GameObject playerObj;
    public Vector3 m_startPos;
    //与起始点的距离
    protected float m_distance;
    //玩家与敌人的距离
    protected float e_p_distance;
    //转身速率
    protected float m_turnSpeed = 3f;
    protected float chaseSpeed = 1f;

    public EnemyBackState(FSMMgr _fsmMgr) : base(_fsmMgr)
    {
        m_statusID = StateID.STATE_BACK;
    }

    public override void Update()
    {
        if (enemyObj == null && m_startPos == null)
            return;

        m_distance = Vector3.Distance(enemyObj.transform.position , m_startPos);

        Vector3 forwardVector = m_startPos - enemyObj.transform.position;

        
        Quaternion quaternion = Quaternion.LookRotation(forwardVector , Vector3.up);
        enemyObj.transform.rotation = Quaternion.Lerp(enemyObj.transform.rotation, quaternion, Time.deltaTime * m_turnSpeed);

        if (m_distance > 0.01f)//(!Vector3.Equals(enemyObj.transform.position, m_startPos))
        {
            enemyObj.transform.Translate(Vector3.forward * Time.deltaTime * chaseSpeed);
        }
    }

    public override void BreakCondition()
    {
        if (enemyObj == null && m_startPos == null)
            return;

        if (m_distance < 0.02f)
        {
            fsmMgr.TransState(TransConditionID.C_INIT_TO_CRUISE);
        }

        //进入追击距离的话，开始追击
        e_p_distance = Vector3.Distance(playerObj.transform.position, enemyObj.transform.position);
        if (enemyCollider == null)
            enemyCollider = enemyObj.GetComponent<CapsuleCollider>();
        if (e_p_distance < enemyCollider.radius*4)
        {
            fsmMgr.TransState(TransConditionID.C_BACK_T_CHASE);
        }
    }

}
