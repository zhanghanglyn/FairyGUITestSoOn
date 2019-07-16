using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//该文件为枚举定义等

public struct IKPosRotation
{
    public Vector3 pos;
    public Quaternion rotate;

    public IKPosRotation(Vector3 _pos, Quaternion _rotate)
    {
        pos = _pos;
        rotate = _rotate;
    }
}