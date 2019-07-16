using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject followObj;
    public float lookAtOffsetY = 3;     //看向头部或是看向腿部的offset
    public float rotateSpeed = 30.0f;   //绕轴旋转速度
    public float lookDistance = 3;      //镜头与跟随OBJ的距离

    private Vector3 m_lastPos;          //用来存储上一次移动前的followObj位置，用来计算移动向量
    private Vector3 m_calculatePos;     //与offset进行处理后的计算位置（头部或是腿部）

    public enum TURNTYPE 
    {
        TURN_LEFT = 1,
        TURN_RIGHT = 2,
    }

	// Use this for initialization
	void Start () {
        if (followObj == null)
            return;

        m_calculatePos = new Vector3(followObj.transform.position.x, followObj.transform.position.y + lookAtOffsetY, followObj.transform.position.z);

        //先让摄像机看向物体，再根据摄像机与物体距离进行计算
        gameObject.transform.LookAt(m_calculatePos);
        Vector3 followObjToCamera = (m_calculatePos - gameObject.transform.position);
        float diff = lookDistance - followObjToCamera.magnitude;

        //根据向量摸进行处理
        Vector3 distancelocalTrans = gameObject.transform.InverseTransformVector(-followObjToCamera.normalized * diff);
        gameObject.transform.Translate(distancelocalTrans);
        gameObject.transform.LookAt(m_calculatePos);

        m_lastPos = m_calculatePos;
    }

    // Update is called once per frame
    void Update () {
        if (followObj == null)
            return;

        //LookAt
        m_calculatePos = new Vector3(followObj.transform.position.x, followObj.transform.position.y + lookAtOffsetY, followObj.transform.position.z);

        //先让摄像机看向物体，再根据摄像机与物体距离进行计算
        gameObject.transform.LookAt(m_calculatePos);
        Vector3 followObjToCamera = (m_calculatePos - gameObject.transform.position);
        float diff = lookDistance - followObjToCamera.magnitude;

        //根据向量摸进行处理
        Vector3 distancelocalTrans = gameObject.transform.InverseTransformVector(-followObjToCamera.normalized * diff);
        gameObject.transform.Translate(distancelocalTrans);
        
        //根据物体前行的方向移动跟随
        if (m_lastPos != null && m_lastPos != m_calculatePos)
        {
            Vector3 transVec = m_calculatePos - m_lastPos;
            Vector3 localTrans = gameObject.transform.InverseTransformVector(new Vector3(transVec.x, transVec.y, transVec.z));
            gameObject.transform.Translate(localTrans);
        }

        m_lastPos = m_calculatePos;

        //gameObject.transform.LookAt(m_calculatePos);
    }

    /// <summary>
    /// 围绕跟随物体旋转
    /// </summary>
    /// <param name="_turnType"></param>
    public void RotateAround(TURNTYPE _turnType  )
    {
        if (followObj == null)
            return;
        //左转，右转
        if (_turnType == TURNTYPE.TURN_LEFT)
        {
            gameObject.transform.RotateAround(followObj.transform.position, Vector3.up, rotateSpeed);
        }
        else
        {
            gameObject.transform.RotateAround(followObj.transform.position, Vector3.up, -rotateSpeed);
        }
    }
}
