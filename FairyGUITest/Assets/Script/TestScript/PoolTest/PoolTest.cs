using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour {

    ObjPool<TestBullet> pool;

    List<TestBullet> m_bullet_list = new List<TestBullet>();

    List<TestBullet> delete_list = new List<TestBullet>();

    bool bClose = false;
    bool b_start = false;

    // Use this for initialization
    void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update () {

        if (b_start == false)
            return;

        if (bClose == false)
        {
            //从对象池中获取一个OBJ并且让他向上飞
            if (pool != null && m_bullet_list.Count < 15)
            {
                TestBullet bullet = pool.GetObj();
                //TestBullet bullet = TestBullet.CreateBullet();

                bullet.m_bullet.transform.position = new Vector3(bullet.m_bullet.transform.position.x,
                    4 - m_bullet_list.Count * 0.5f, bullet.m_bullet.transform.position.z);
                m_bullet_list.Add(bullet);
            }

            foreach (TestBullet item in m_bullet_list)
            {
                item.m_bullet.transform.position = new Vector3(item.m_bullet.transform.position.x,
                    item.m_bullet.transform.position.y + 0.01f, item.m_bullet.transform.position.z);

                if (item.m_bullet.transform.position.y > 5)
                {
                    delete_list.Add(item);
                }
            }

            foreach (TestBullet item in delete_list)
            {
                m_bullet_list.Remove(item);
                pool.ReturnObj(item);
            }
            delete_list.Clear();
        }
        else
        {
            if (m_bullet_list.Count > 0)
            {
                //pool = null;
                foreach (TestBullet item in m_bullet_list)
                {
                    delete_list.Add(item);
                }
                foreach (TestBullet item in delete_list)
                {
                    m_bullet_list.Remove(item);
                    pool.ReturnObj(item);
                }
                delete_list.Clear();
                m_bullet_list.Clear();

                PoolManager.GetInstance().RemoveObjPool<TestBullet>();
                pool = null;
            }
        }

    }

    public void init()
    {
        if (pool == null)
            pool = PoolManager.GetInstance().CreateObjPool<TestBullet>(10, TestBullet.CreateBullet,TestBullet.SetBulletActive,TestBullet.Clear);
        
    }

    private void OnGUI()
    {
        if(GUI.Button( new Rect(100,100,200,100), "dddddddd"))
        {
            if (bClose == true)
                bClose = false;
            else
                bClose = true;
        }

        if (GUI.Button(new Rect(100, 200, 200, 100), "start"))
        {
            b_start = true;
        }
        if (GUI.Button(new Rect(100, 300, 200, 100), "init"))
        {
            init();
        }
    }
}
