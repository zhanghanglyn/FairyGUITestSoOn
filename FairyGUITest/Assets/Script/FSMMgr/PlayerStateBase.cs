using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制器使用的状态机基类，添加了对应按键break的条件
/// </summary>
public class PlayerStateBase : FSMStateBase {

    private Dictionary<KeyCode, TransConditionID> keyToState;
    private Dictionary<KeyCode, System.Func<bool>> keyTransConditionFun;

    public PlayerStateBase(FSMMgr _mgr) : base(_mgr)
    {
        keyToState = new Dictionary<KeyCode, TransConditionID>();
        keyTransConditionFun = new Dictionary<KeyCode, System.Func<bool>>();
    }

    public override void Update()
    {

    }

    /// <summary>
    /// 如果有根据按键退出的条件 19.06.21 还应该添加，转换条件的判断！
    /// </summary>
    public override void BreakCondition()
    {
        if (GetKeyTransState(InputMgr.GetInstance().GetCurKeyDown()) != TransConditionID.CONDITION_NULL)
        {
            System.Func<bool> temp_func = null;
            if (keyTransConditionFun.ContainsKey(InputMgr.GetInstance().GetCurKeyDown()))
                temp_func = keyTransConditionFun[InputMgr.GetInstance().GetCurKeyDown()];
            if (temp_func == null || temp_func() == true)
                fsmMgr.TransState(GetKeyTransState(InputMgr.GetInstance().GetCurKeyDown()));
        }
    }

    /// <summary>
    /// 加入按键对应的状态关系,什么按键能跳转到什么状态 , 新添加，需要再添加一个判断条件函数
    /// </summary>
    public virtual void AddKeyCondition(KeyCode _Key, TransConditionID _state , System.Func<bool> _conditionJudgeFun)
    {
        if (_Key == KeyCode.None)
        {
            Debug.Log("AddKeyCondition Nulll!!!!!!!!");
            return;
        }
        if (keyToState.ContainsKey(_Key))
            return;
        else
        {
            keyToState.Add(_Key, _state);
            keyTransConditionFun.Add(_Key, _conditionJudgeFun);
        }
            
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

        if (keyTransConditionFun.ContainsKey(_Key))
            keyTransConditionFun.Remove(_Key);
    }
}
