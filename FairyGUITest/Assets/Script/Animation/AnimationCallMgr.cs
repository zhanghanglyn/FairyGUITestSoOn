using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 尝试
/// 所有动画的回调，其实都可以只回调一个统一的Behaviour，在管理器函数中注册每一个回调即可方便使用
/// 只回调整个动画控制器，具体是哪一个动画由外部进行自行判断
/// 并不会进行释放等工作，释放等需要由外部进行
/// </summary>
public class AnimationCallMgr
{
    public static AnimationCallMgr instance;

    //进入动画时、退出动画时、动画进行时的回调，key为动画控制器
    Dictionary<Animator, System.Action<AnimatorStateInfo>> m_EnterCall;
    Dictionary<Animator, System.Action<AnimatorStateInfo>> m_ExitCall;
    Dictionary<Animator, System.Action<AnimatorStateInfo>> m_UpdateCall;

    public static AnimationCallMgr GetInstance()
    {
        if (instance == null)
        {
            instance = new AnimationCallMgr();
            instance.Init();
        }
        return instance;
    }

    public void Init()
    {
        m_EnterCall = new Dictionary<Animator, System.Action<AnimatorStateInfo>>();
        m_ExitCall = new Dictionary<Animator, System.Action<AnimatorStateInfo>>();
        m_UpdateCall = new Dictionary<Animator, System.Action<AnimatorStateInfo>>();
    }

    /// <summary>
    /// 注册一个进入动画时的回调
    /// </summary>
    /// <param name="_animator">注册的动画Animator</param>
    /// <param name="_call">进入动画时的回调函数，会包含AnimatorStateInfo参数</param>
    public void RegistEnterCall(Animator _animator, System.Action<AnimatorStateInfo> _call)
    {
        if (m_EnterCall == null)
            return;
        if (!m_EnterCall.ContainsKey(_animator))
        {
            m_EnterCall.Add(_animator, _call);
        }
    }

    /// <summary>
    /// 删除一个进入动画的回调
    /// </summary>
    /// <param name="_animator">注册的动画Animator</param>
    /// <param name="_call">进入动画时的回调函数，会包含AnimatorStateInfo参数</param>
    public void DeleteEnterCall(Animator _animator, System.Action<AnimatorStateInfo> _call)
    {
        if (m_EnterCall == null)
            return;
        if (!m_EnterCall.ContainsKey(_animator))
        {
            m_EnterCall.Remove(_animator);
        }
    }

    public void EnterCall(Animator _animator, AnimatorStateInfo _animatorStateInfo)
    {
        if (m_EnterCall == null)
            return;
        foreach ( var item in m_EnterCall)
        {
            if (item.Value != null)
                item.Value(_animatorStateInfo);
        }
    }

    /// <summary>
    /// 注册一个退出动画时的回调
    /// </summary>
    /// <param name="_animator">注册的动画Animator</param>
    /// <param name="_call">进入动画时的回调函数，会包含AnimatorStateInfo参数</param>
    public void RegistExitCall(Animator _animator, System.Action<AnimatorStateInfo> _call)
    {
        if (m_ExitCall == null)
            return;
        if (!m_ExitCall.ContainsKey(_animator))
        {
            m_ExitCall.Add(_animator, _call);
        }
    }

    /// <summary>
    /// 删除一个退出动画的回调
    /// </summary>
    /// <param name="_animator">注册的动画Animator</param>
    /// <param name="_call">进入动画时的回调函数，会包含AnimatorStateInfo参数</param>
    public void DeleteExitCall(Animator _animator, System.Action<AnimatorStateInfo> _call)
    {
        if (m_ExitCall == null)
            return;
        if (!m_ExitCall.ContainsKey(_animator))
        {
            m_ExitCall.Remove(_animator);
        }
    }
    public void ExitCall(Animator _animator, AnimatorStateInfo _animatorStateInfo)
    {
        if (m_ExitCall == null)
            return;
        foreach (var item in m_ExitCall)
        {
            if (item.Value != null)
                item.Value(_animatorStateInfo);
        }
    }

    /// <summary>
    /// 注册一个动画更新时的回调
    /// </summary>
    /// <param name="_animator">注册的动画Animator</param>
    /// <param name="_call">进入动画时的回调函数，会包含AnimatorStateInfo参数</param>
    public void RegistUpdateCall(Animator _animator, System.Action<AnimatorStateInfo> _call)
    {
        if (m_UpdateCall == null)
            return;
        if (!m_UpdateCall.ContainsKey(_animator))
        {
            m_UpdateCall.Add(_animator, _call);
        }
    }

    /// <summary>
    /// 删除一个动画更新的回调
    /// </summary>
    /// <param name="_animator">注册的动画Animator</param>
    /// <param name="_call">进入动画时的回调函数，会包含AnimatorStateInfo参数</param>
    public void DeleteUpdateCall(Animator _animator, System.Action<AnimatorStateInfo> _call)
    {
        if (m_UpdateCall == null)
            return;
        if (!m_UpdateCall.ContainsKey(_animator))
        {
            m_UpdateCall.Remove(_animator);
        }
    }
    public void UpdateCall(Animator _animator, AnimatorStateInfo _animatorStateInfo)
    {
        if (m_UpdateCall == null)
            return;
        foreach (var item in m_UpdateCall)
        {
            if (item.Value != null)
                item.Value(_animatorStateInfo);
        }
    }

    public void ClearAllCall()
    {
        if (m_UpdateCall != null)
            m_UpdateCall.Clear();
        if (m_ExitCall != null)
            m_ExitCall.Clear();
        if (m_EnterCall != null)
            m_EnterCall.Clear();
    }
}
