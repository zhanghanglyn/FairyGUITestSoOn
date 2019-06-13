using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 某一单独类的对象池
/// 创建和清除方法有两种，一是使用委托函数来创建和删除，二是规定放入对象池的对象必须实现Clear和Create函数？这里尝试一
/// ** 此对象池为借用模式，由使用者进行外部管理；
/// ** 必须传入设置Obj Active属性的函数委托
/// ** 目前并没有生成一个永不清除的GameObject来外挂控件，因为设计上来说可能有些非GameObject对象也会用到对象池
/// </summary>
/// <typeparam name="T">该对象池的类型</typeparam>
public class ObjPool<T> {

    //初始化该池对象的函数
    System.Func<T> m_create_func;
    //清除该对象池的函数  (想了想感觉没啥用，先留着吧)
    System.Action<T> m_clear_func;
    //设置对象的可视化（是否Active的函数)
    System.Action<T, bool> m_setActive_func;

    //该池的对象列表
    private List<T> m_poolList = new List<T>();
    //对象标记，会将所有取出的对象做一个标记，避免归还不是本对象池产生的对象
    private List<T> m_tag_list = new List<T>();

    private static ObjPool<T> m_instance;

    //初始化数量
    private int m_initNum = 0;
    //若当前池中数量不足，每次增加的数量
    private int m_upNum = 2;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_initNum">初次创建时创建的数量,只在该池不存在时使用</param>
    /// <returns></returns>
    public static ObjPool<T> GetInstance( )
    {
        if (m_instance == null)
        {
            m_instance = new ObjPool<T>();
        }
        return m_instance;
    }

    public void Clear()                    // 析构函数
    {
        if (m_poolList != null)
        {
            foreach( T item in m_poolList)
            {
                m_clear_func(item);
            }
            m_poolList.Clear();
        }
    }

    /// <summary>
    /// 初始化池，根据创建函数和清除函数以及初始数量生成一个初始池以及初始化对象
    /// </summary>
    /// <param name="_create_func">创建Obj函数</param>
    /// <param name="_clear_func">清除Obj函数</param>
    /// <param name="m_setActive_func">设置Obj可见性的函数</param>
    /// <param name="_upNum">若池中数量不足，则每次增长的数量</param>
    /// <typeparam _initNum="">初始创建数量</typeparam>
    public void InitObjPool(int _initNum , System.Func<T> _create_func, System.Action<T, bool> _setActive_func, System.Action<T> _clear_func = null, int _upNum = 5)
    {
        m_initNum = _initNum;
        m_create_func += _create_func;
        m_clear_func += _clear_func;
        m_setActive_func += _setActive_func;
        for (int init_num = 0; init_num <= m_initNum; init_num++)
        {
            CreateObj();
        }

    }

    /// <summary>
    /// 调用外部函数获取item,并且将该ITEM加入列表中
    /// </summary>
    /// <returns></returns>
    private T CreateObj()
    {
        T temp_item = m_create_func();
        m_poolList.Add(temp_item);
        m_setActive_func(temp_item, false);
        return temp_item;
    }

    /// <summary>
    /// 从池中获取一个该类型的OBJ
    /// </summary>
    // 设置可见性为true
    public T GetObj()
    {;
        //将第一个输出，并且设置第一个可见，并且从列表中移除
        if (m_poolList.Count > 0)
        {
            T returnObj = m_poolList[0];
            m_poolList.RemoveAt(0);
            MarkGetObj(returnObj);
            m_setActive_func(returnObj, true);
            return returnObj;
        }
        else
        {
            for (int iUpCount = 0; iUpCount < m_upNum;iUpCount ++)
            {
                CreateObj();
            }
            if (m_poolList.Count > 0)
            {
                T returnObj = m_poolList[0];
                m_poolList.RemoveAt(0);
                MarkGetObj(returnObj);
                m_setActive_func(returnObj, true);
                return returnObj;
            }
        }

        T Obj = CreateObj();
        m_setActive_func(Obj, true);
        MarkGetObj(Obj);
        m_setActive_func(Obj, true);
        return Obj;
    }

    /// <summary>
    /// 只负责归还，会调用归还后的隐藏，并且会判断归还的obj是否是属于该pool池中出去的对象,
    /// </summary>
    // 设置可见性为false 
    public void ReturnObj(T _obj)
    {
        if(!m_tag_list.Contains(_obj))
        {
            return;
        }
        if (m_poolList != null)
            m_poolList.Add(_obj);
        m_tag_list.Remove(_obj);
        m_setActive_func(_obj, false);
    }

    /// <summary>
    /// 对出池的对象做个标记，防止归还时归还并不属于池中的对象
    /// </summary>
    void MarkGetObj( T _obj)
    {
        if(!m_tag_list.Contains(_obj))
        {
            m_tag_list.Add(_obj);
        }
    }
}


/// <summary>
/// 所有OjbPool类的管理类，单例
/// </summary>
public class PoolManager
{
    public static PoolManager m_instance;

    //用来存储对象池的list
    private List<object> m_ObjPoolList = new List<object>();
    private Dictionary<object,int> m_TypeList = new Dictionary<object, int>();

    public static PoolManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new PoolManager();
        }
        return m_instance;
    }

    /// <summary>
    /// 创建一个类型的对象池
    /// </summary>
    /// <typeparam name="T">该对象池的类型</typeparam>
    /// <typeparam _initNum="">初始创建数量</typeparam>
    /// <param name="_create_func">创建Obj函数</param>
    /// <param name="_clear_func">清除Obj函数，暂时不需要</param>
    /// <param name="m_setActive_func">设置Obj可见性的函数</param> 
    /// <returns></returns>
    public ObjPool<T> CreateObjPool<T>( int _initNum , System.Func<T> _create_func, System.Action<T, bool> _setActive_func, System.Action<T> _clear_func = null , int _upNum = 2)
    {
        //if (!m_ObjPoolList.Contains(ObjPool<T>.GetInstance()))
        if (!m_TypeList.ContainsKey(typeof(T)))    //如果不包含这个类型，则加入
        {
            ObjPool<T>.GetInstance().InitObjPool(_initNum, _create_func, _setActive_func, _clear_func, _upNum);
            m_ObjPoolList.Add(ObjPool<T>.GetInstance());
            m_TypeList.Add(typeof(T), m_ObjPoolList.Count - 1);
        }

        return ObjPool<T>.GetInstance();
    }

    /// <summary>
    /// 清除一个类型的对象池
    /// </summary>
    /// <typeparam name="T">要清除的对象池的类型</typeparam>
    public void RemoveObjPool<T>()
    {
        if (m_TypeList.ContainsKey(typeof(T)))
        {
            m_ObjPoolList.RemoveAt(m_TypeList[typeof(T)]);
            ObjPool<T>.GetInstance().Clear();
            m_TypeList.Remove(typeof(T));
        }
    }
}
