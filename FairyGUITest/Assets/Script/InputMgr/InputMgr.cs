using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr{

    private static InputMgr instance;

    public static InputMgr GetInstance()
    {
        if (instance == null)
            instance = new InputMgr();

        return instance;
    }


    /*#if UNITY_ANDROID
        Application.dataPath + "!assets/"+ PathSetting.assetBundlePath;
    #elif UNITY_IPHONE
                                Application.dataPath + "/Raw/"+ PathSetting.assetBundlePath;  
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                                Application.dataPath + "/" + PathSetting.assetBundlePath;
    #else
                                string.Empty;  
    #endif */

    public Vector3 GetMoveVec()
    {
        float vec_x = 0;
        float vec_y = 0;
        float vec_z = 0;
#if UNITY_ANDROID
        //Touch touchInfo = Input.GetTouch(0);
        if (Input.GetKey((KeyCode)KeyEnum.KeyLeft))
            vec_x = -1;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyRight))
            vec_x = 1;
        else
            vec_x = 0;

        if (Input.GetKey((KeyCode)KeyEnum.KeyUp))
            vec_z = 1;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyDown))
            vec_z = -1;
        else
            vec_y = 0;

#elif UNITY_IPHONE
        Touch touchInfo = Input.GetTouch(0);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetKey((KeyCode)KeyEnum.KeyLeft))
            vec_x = -1;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyRight))
            vec_x = 1;
        else
            vec_x = 0;

        if (Input.GetKey((KeyCode)KeyEnum.KeyUp))
            vec_z = 1;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyDown))
            vec_z = -1;
        else
            vec_y = 0;
#else
        string.Empty;  
#endif


        return new Vector3(vec_x,vec_y,vec_z);
    }

    /// <summary>
    /// 获取根据摄像机方向获得的输入移动方向,不会返回向上的方向，向上的方向由使用者自己赋予
    /// </summary>
    /// <param name="_camera"></param>
    /// <returns></returns>
    public Vector3 GetVecCamera(Camera _camera)
    {
        //获取摄像机的前方向
        Vector3 forward = _camera.transform.forward.normalized;
        forward.y = 0;
        Vector3 left = Vector3.Cross(forward, Vector3.up.normalized);
        Vector3 right = Vector3.Cross( Vector3.up.normalized, forward);
        Vector3 back = -forward;

        Vector3 finalVec = new Vector3();

#if UNITY_ANDROID
        //Touch touchInfo = Input.GetTouch(0);
        if (Input.GetKey((KeyCode)KeyEnum.KeyUp))
            finalVec = forward;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyDown))
            finalVec = finalVec + back;

        if (Input.GetKey((KeyCode)KeyEnum.KeyLeft))
            finalVec = finalVec + left;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyRight))
            finalVec = finalVec + right;

#elif UNITY_IPHONE
        Touch touchInfo = Input.GetTouch(0);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetKey((KeyCode)KeyEnum.KeyUp))
            finalVec = forward;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyDown))
            finalVec = finalVec + back;

        if (Input.GetKey((KeyCode)KeyEnum.KeyLeft))
            finalVec = finalVec + left;
        else if (Input.GetKey((KeyCode)KeyEnum.KeyRight))
            finalVec = finalVec + right;
#else
        string.Empty;  
#endif


        return finalVec.normalized;
    }

    /// <summary>
    /// 获取当前按下的按键,需要放在update中
    /// </summary>
    /// <returns></returns>
    public KeyCode GetCurKeyDown()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in KeyEnum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    return keyCode;
                }
            }
        }

        return KeyCode.None;
    }
}
