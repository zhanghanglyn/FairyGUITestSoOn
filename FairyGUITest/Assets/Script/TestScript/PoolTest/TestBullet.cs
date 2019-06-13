using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试池用子弹测试类
/// </summary>
public class TestBullet{

    public GameObject m_bullet;

	public static TestBullet CreateBullet()
    {
        GameObject bullet = Resources.Load("ttt/bullet") as GameObject;
        TestBullet bu = new TestBullet();
        if (bullet != null)
            bu.m_bullet = GameObject.Instantiate(bullet);

        return bu;
    }

    public static void SetBulletActive( TestBullet bullet ,bool _bActive)
    {
        if (bullet.m_bullet != null)
        {
            bullet.m_bullet.SetActive(_bActive);
        }
    }

    public static void Clear(TestBullet _bullet)
    {
        GameObject.Destroy(_bullet.m_bullet);
        _bullet = null;
    }
}
