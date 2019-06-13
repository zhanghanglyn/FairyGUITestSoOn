using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传入初始值与要达到的值以及运行时间，不断update返回
/// </summary>
public class GradualValue{ 

    bool bStart = false;
    private float startTime = 0.0f;
    class ValueEnum<T>
    {
        public T orlVal;
        public T finalVal;
        public T curVal;
        public T aSpeed;    //加速度
    };

    private Dictionary<string , ValueEnum<int>> m_valueListI;
    private Dictionary<string ,ValueEnum<float>> m_valueListF;
    private List<string> deleteKeyList;

    public GradualValue()
    {
        m_valueListI = new Dictionary<string, ValueEnum<int>>();
        m_valueListF = new Dictionary<string, ValueEnum<float>>();
        deleteKeyList = new List<string>();
    }


    //放入调用函数的update中，根据标识来执行
    public void Update()
    {
        if (bStart == false)
            return;

        if (m_valueListI != null || m_valueListI.Count > 0)
        {
            /*for (int iCount = 0;iCount < m_valueListI.Count; iCount ++)
            {
                if (m_valueListI[iCount].curVal < m_valueListI[iCount].finalVal)
                    m_valueListI[iCount].curVal += m_valueListI[iCount].aSpeed;
                else
                {
                    m_valueListI[iCount].curVal += m_valueListI[iCount].aSpeed;
                } 
            }*/
            foreach ( var item in m_valueListI)
            {
                if (item.Value.aSpeed > 0)
                {
                    item.Value.curVal += item.Value.aSpeed;
                    if (item.Value.curVal > item.Value.finalVal)
                    {
                        item.Value.curVal = item.Value.finalVal;
                    }
                }
                else
                {

                    item.Value.curVal += item.Value.aSpeed;
                    if (item.Value.curVal < item.Value.finalVal)
                    {
                        item.Value.curVal = item.Value.finalVal;
                    }
                }
            }
        }

        if (m_valueListF != null || m_valueListF.Count > 0)
        {
            foreach (var item in m_valueListF)
            {
                if (item.Value.aSpeed > 0)
                {
                    item.Value.curVal += item.Value.aSpeed;
                    if (item.Value.curVal > item.Value.finalVal)
                    {
                        item.Value.curVal = item.Value.finalVal;
                    }
                }
                else
                {
                    
                    item.Value.curVal += item.Value.aSpeed;
                    if (item.Value.curVal < item.Value.finalVal)
                    {
                        item.Value.curVal = item.Value.finalVal;
                    }
                }
            }
        }

    }

    /// <summary>
    /// 添加一个渐变值
    /// </summary>
    /// <typeparam name="T">需要变化的类型，数值型</typeparam>
    /// <param name="_orlVal">起始值</param>
    /// <param name="_finalVal">结束值</param>
    /// <param name="_time">时间</param>
    public int AddGradualValue( string _name, int _orlVal , int _finalVal , int _time )
    {
        if (m_valueListI != null && m_valueListI.ContainsKey(_name))
        {
            return m_valueListI[_name].curVal;
        }

        ValueEnum<int> temp_item = new ValueEnum<int>();
        temp_item.orlVal = _orlVal;
        temp_item.finalVal = _finalVal;
        temp_item.curVal = _orlVal;
        temp_item.aSpeed = (_finalVal - _orlVal) / _time;

        if (m_valueListI != null)
            m_valueListI.Add(_name ,temp_item);

        return temp_item.curVal;
    }
    public float AddGradualValue(string _name,float _orlVal, float _finalVal, int _time)
    {
        if (m_valueListF != null && m_valueListF.ContainsKey(_name))
        {
            return m_valueListF[_name].curVal;
        }

        ValueEnum<float> temp_item = new ValueEnum<float>();
        temp_item.orlVal = _orlVal;
        temp_item.finalVal = _finalVal;
        temp_item.curVal = _orlVal;
        temp_item.aSpeed = (_finalVal - _orlVal) / _time;

        if (m_valueListF != null)
            m_valueListF.Add(_name, temp_item);

        return temp_item.curVal;
    }

    public void Start()
    {
        bStart = true;
    }

    public void Pause()
    {
        bStart = false;
    }

    public void Clear()
    {
        Pause();
        m_valueListI.Clear();
        m_valueListF.Clear();
    }

    public void ClearByKey(string _key)
    {
        if (m_valueListI != null && m_valueListI.ContainsKey(_key))
        {
            m_valueListI.Remove(_key);
        }
        if (m_valueListF != null && m_valueListF.ContainsKey(_key))
        {
            m_valueListF.Remove(_key);
        }
    }
}
