using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制器使用的状态机基类，添加了对应按键break的条件
/// </summary>
public class PlayerStateBase : FSMStateBase {

    private Dictionary<KeyCode, TransConditionID> keyToState;

    public PlayerStateBase(FSMMgr _mgr) : base(_mgr)
    {
        keyToState = new Dictionary<KeyCode, TransConditionID>();
    }

    public override void Update()
    {

    }

    public override void BreakCondition()
    {
    }

    /// <summary>
    /// 加入按键对应的状态关系,什么按键能跳转到什么状态
    /// </summary>
    public virtual void AddKeyCondition(KeyCode _Key, TransConditionID _state)
    {
        if (_Key == KeyCode.None)
        {
            Debug.Log("AddKeyCondition Nulll!!!!!!!!");
            return;
        }
        if (keyToState.ContainsKey(_Key))
            return;
        else
            keyToState.Add(_Key, _state);
    }

    /// <summary>
    /// 根据传入的条件ID，返回该该条件变化后的状态
    /// </summary>
    /// <param name="_condition"></param>
    public virtual TransConditionID GetKeyTransState(KeyCode _Key)
    {
        if (keyToState.ContainsKey(_Key))
            return keyToState[_Key];
        else
            return TransConditionID.CONDITION_NULL;
    }

    /// <summary>
    /// 删除某个条件
    /// </summary>
    public virtual void DeleteKeyState(KeyCode _Key)
    {
        if (keyToState.ContainsKey(_Key))
            keyToState.Remove(_Key);
        else
            Debug.Log("The key condition you wanna delete is null");
    }
}
