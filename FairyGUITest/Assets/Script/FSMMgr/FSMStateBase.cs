using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态基类，trans Condition转换条件，由该状态转化到另一个状态的条件，可能有多个
/// </summary>
public abstract class FSMStateBase {
    //当前的状态ID
    protected StateID m_statusID;
    public StateID StatusID { get { return m_statusID; } }
    // 该状态中，条件对应的状态关系
    protected Dictionary<TransConditionID, StateID> m_transConditionMap = new Dictionary<TransConditionID, StateID>();
    //状态控制器
    protected FSMMgr fsmMgr;

    public FSMStateBase(FSMMgr _fsmMgr)
    {
        fsmMgr = _fsmMgr;
    }

    /// <summary>
    /// 加入条件对应的状态关系,什么条件能跳转到什么状态
    /// </summary>
    public virtual void AddCondition( TransConditionID _condition , StateID _state)
    {
        if (_condition == TransConditionID.CONDITION_NULL)
        {
            Debug.Log("AddCondition Nulll!!!!!!!!");
            return;
        }
        if (m_transConditionMap.ContainsKey(_condition))
            return;
        else
            m_transConditionMap.Add(_condition, _state);
    }

    /// <summary>
    /// 根据传入的条件ID，返回该该条件变化后的状态
    /// </summary>
    /// <param name="_condition"></param>
    public virtual StateID GetTransState(TransConditionID _condition)
    {
        if (m_transConditionMap.ContainsKey(_condition))
            return m_transConditionMap[_condition];
        else
            return StateID.STATE_NULL;
    }
    
    /// <summary>
    /// 删除某个条件
    /// </summary>
    public virtual void DeleteState( TransConditionID _condition )
    {
        if (m_transConditionMap.ContainsKey(_condition))
            m_transConditionMap.Remove(_condition);
        else
            Debug.Log("The condition you wanna delete is null");
    }

    /// <summary>
    /// 有可能有些状态不需要做处理，可以不用重写,进入之前的逻辑
    /// </summary>
    public virtual void BeforeEnter(){ }
    public virtual void BeforeExit(){ }

    /// <summary>
    /// 在该状态时的循环
    /// </summary>
    public abstract void Update();
    /// <summary>
    /// 跳出当前状态的循环函数（条件判断）
    /// </summary>
    public abstract void BreakCondition();
	
}
