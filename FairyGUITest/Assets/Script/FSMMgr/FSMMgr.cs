using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态机管理类，可以为任意一个对象添加一个状态机，也可以在主循环中进行，多种用法故不使用单例
/// </summary>
public class FSMMgr {
    //当前状态机的状态ID
    protected StateID m_curStateID = StateID.STATE_NULL;
    public StateID CurStateID { get { return m_curStateID; } }
    //当前的状态
    protected FSMStateBase m_curFSMState;

    //保存每个状态ID对应的状态类
    protected Dictionary<StateID, FSMStateBase> m_StateMap = new Dictionary<StateID, FSMStateBase>();

    /// <summary>
    /// 创建一个状态机并且设置其初始状态
    /// </summary>
    /// <returns></returns>
    public void Init(StateID _stateID, FSMStateBase _fsmState)
    {
        AddState(_stateID, _fsmState);
        SetCurFSMState(_stateID);
    }

    /// <summary>
    /// 添加状态
    /// </summary>
    public void AddState( StateID _stateID, FSMStateBase _fsmState )
    {
        if (m_StateMap == null)
        {
            Debug.Log("Error!!! StateMap is null!!!");
            return;
        }
        //再判断下不为空状态和空条件
        if (_stateID == StateID.STATE_NULL)
            return;

        if (m_StateMap.ContainsKey(_stateID))
            Debug.Log("State Already Exist!");
        else
            m_StateMap.Add(_stateID, _fsmState);
    }

    /// <summary>
    /// 删除一个状态ID对应的状态
    /// </summary>
    /// <param name="_stateID"></param>
    public void DeleteState( StateID _stateID )
    {
        if (m_StateMap == null)
        {
            Debug.Log("Error!!! StateMap is null!!!");
            return;
        }
        if (m_StateMap.ContainsKey(_stateID))
            m_StateMap.Remove(_stateID);
        else
            Debug.Log("State not in this FSM !!");
    }

    /// <summary>
    /// 循环，在此对当前状态进行循环
    /// </summary>
    public void UpdateState()
    {
        if (m_curFSMState != null)
        {
            m_curFSMState.Update();
            m_curFSMState.BreakCondition();
        }
            
    }

    /// <summary>
    /// 根据传入的条件ID转换状态并且返回转换后的状态ID
    /// </summary>
    public StateID TransState( TransConditionID _conditionID )
    {
        if (m_curFSMState == null)
        {
            //throw new Exception("XXXX");
            Debug.Log("This FSMMgr has No State!");
            return StateID.STATE_NULL;
        }

        StateID tempStateID = m_curFSMState.GetTransState(_conditionID);
        if (tempStateID == StateID.STATE_NULL)  //该条件并不能使该状态转换，返回无状态
            return StateID.STATE_NULL;

        //如果表中查找不到转换状态
        if (!m_StateMap.ContainsKey(tempStateID))
            return StateID.STATE_NULL;

        FSMStateBase fsmState = m_StateMap[tempStateID];
        if (fsmState == null)
        {
            //throw new Exception("FSMStateBase is null!!!!!!!!!!!!!!");
            return StateID.STATE_NULL;
        }
            
        //当前状态执行退出回调
        m_curFSMState.BeforeExit();
        //下一个状态执行进入回调
        fsmState.BeforeEnter();
        m_curFSMState = fsmState;

        return tempStateID;
    }

    /// <summary>
    /// 传入状态ID设置当前的状态，内部初始化使用
    /// </summary>
    /// <param name="_stateID"></param>
    private void SetCurFSMState( StateID _stateID)
    {
        //如果表中查找不到转换状态
        if (!m_StateMap.ContainsKey(_stateID))
            return;

        FSMStateBase fsmState = m_StateMap[_stateID];
        if (fsmState == null)
        {
            //throw new Exception("FSMStateBase is null!!!!!!!!!!!!!!");
            return ;
        }

        m_curFSMState = fsmState;
    }
}
