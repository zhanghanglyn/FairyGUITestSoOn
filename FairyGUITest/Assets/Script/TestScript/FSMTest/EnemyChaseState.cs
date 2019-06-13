using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : FSMStateBase {

    public GameObject enemyObj;
    public GameObject playerObj;
    public CapsuleCollider enemyCollider;

    float e_p_distance = 0;
    float rotateSpeed = 3f;
    float chaseSpeed = 1.3f;

    //来一个委托测试
    System.Action<string> m_enterCall;

    string showWord = "你跑不掉了！";

    public EnemyChaseState(FSMMgr _fsmMgr) : base(_fsmMgr)
    {
        m_statusID = StateID.STATE_CHASE;
    }

    public override void Update()
    {
        if (playerObj != null && enemyObj != null)
        {
            if (enemyCollider == null)
                enemyCollider = enemyObj.GetComponent<CapsuleCollider>();

            Vector3 direction = playerObj.transform.position - enemyObj.transform.position;
            Quaternion qua = Quaternion.LookRotation(direction, Vector3.up);
            enemyObj.transform.rotation = Quaternion.Slerp(enemyObj.transform.rotation, qua, Time.deltaTime * rotateSpeed);

            e_p_distance = Vector3.Distance(playerObj.transform.position, enemyObj.transform.position);
            if (e_p_distance > enemyCollider.radius)
            {
                enemyObj.transform.Translate(Vector3.forward * Time.deltaTime * chaseSpeed);
            }
        }
    }

    public override void BreakCondition()
    {
        if (e_p_distance > 5)
        {
            fsmMgr.TransState(TransConditionID.C_BACK_T_INIT);
        }
    }

    //添加进入时的回调函数
    public void AddEnterCall( System.Action<string> _enterCall)
    {
        m_enterCall += _enterCall;
    }

    public void DeleteEnterCall(System.Action<string> _enterCall)
    {
        m_enterCall -= _enterCall;
    }

    public override void BeforeEnter()
    {
        base.BeforeEnter();

        if (m_enterCall != null)
        {
            m_enterCall(showWord);
        }
    }

}
