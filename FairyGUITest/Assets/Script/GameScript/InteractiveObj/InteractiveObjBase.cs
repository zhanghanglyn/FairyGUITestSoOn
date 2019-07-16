using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在物体上的互动物体base,包含拾取等状态的IK对应部位以及旋转，POS等
/// 
/// </summary>
public class InteractiveObjBase : MonoBehaviour {
    
    Dictionary<AvatarTarget, IKPosRotation> m_IKParam;

    public GameObject leftHandPos;
    public GameObject rightHandPos;
    public GameObject leftFootPos;
    public GameObject rightFootPos;

	// Use this for initialization
	void Start () {
        m_IKParam = new Dictionary<AvatarTarget, IKPosRotation>();
        if (leftHandPos != null)
            m_IKParam.Add(AvatarTarget.LeftHand, new IKPosRotation( leftHandPos.transform.position , leftHandPos.transform.rotation));
        if (rightHandPos != null)
            m_IKParam.Add(AvatarTarget.RightHand, new IKPosRotation(rightHandPos.transform.position, rightHandPos.transform.rotation));
        if (leftFootPos != null)
            m_IKParam.Add(AvatarTarget.LeftFoot, new IKPosRotation(leftFootPos.transform.position, leftFootPos.transform.rotation));
        if (rightFootPos != null)
            m_IKParam.Add(AvatarTarget.RightFoot, new IKPosRotation(rightFootPos.transform.position, rightFootPos.transform.rotation));


    }

    // Update is called once per frame
    void Update () {
		
	}

    //获取对应部位的IK位置以及旋转
    public IKPosRotation? GetIKPosByAvatarTarget( AvatarTarget _target )
    {
        if (m_IKParam.ContainsKey(_target))
        {
            return m_IKParam[_target];
        }

        return null;
    }




}
