using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试敌人会上下上下走动
/// </summary>
public class EnemyCruiseState : FSMStateBase {

    public GameObject enemyObj;
    public GameObject playerObj;
    private Vector3 m_startPos;

    //玩家和敌人间的距离
    float e_p_distance = 0;

    private float m_time;
    private float m_stepZ = 0.02f;

    public EnemyCruiseState(FSMMgr _fsmMgr) : base(_fsmMgr)
    {
        m_time = 0;
    }

    public override void Update()
    {
        if (enemyObj != null)
        {
            m_time += Time.deltaTime;
            float offsetZ = Mathf.Sin(m_time) * m_stepZ;
            enemyObj.transform.position = new Vector3(enemyObj.transform.position.x ,
                enemyObj.transform.position.y , enemyObj.transform.position.z + offsetZ);

        }
    }

    public override void BreakCondition()
    {
        if (playerObj != null && enemyObj != null)
        {
            e_p_distance = Vector3.Distance(playerObj.transform.position, enemyObj.transform.position);
            if (e_p_distance < 5)
            {
                fsmMgr.TransState(TransConditionID.C_CRUISE_T_CHASE);
            }
        }
    }

    public void SetInitPos( Vector3 _startPos )
    {
        m_startPos = _startPos;
    }

}
