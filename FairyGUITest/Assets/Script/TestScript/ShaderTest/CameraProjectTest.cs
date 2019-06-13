using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraProjectTest : MonoBehaviour {

    public Camera projectorCamera;
    public Material material;

    private void Awake()
    {
        projectorCamera = GetComponent<Camera>();
        
    }

    // Use this for initialization
    void Start () {
        //init();

    }
	
	// Update is called once per frame
	void Update () {
        init();
    }

    private void init()
    {
        Matrix4x4 projectionMatrix = projectorCamera.projectionMatrix;
        //摄像机的空间的投影矩阵，GL方法是排出OPENGL和DX的起始坐标点影响
        projectionMatrix = GL.GetGPUProjectionMatrix(projectionMatrix, false);
        Matrix4x4 worldMatrix = projectorCamera.worldToCameraMatrix;
        //创建一个由世界坐标到摄像机投影坐标的矩阵  从右往左计算效果！！！
        Matrix4x4 materailMatrix = projectionMatrix * worldMatrix;

        material.SetMatrix("worldToCameraProjectionMatrix", materailMatrix);

    }
}
