using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RBWorldDepth : MonoBehaviour {

    Camera m_camera;
    public Material material;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        if (m_camera != null)
            m_camera.depthTextureMode = DepthTextureMode.Depth;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_camera != null && material != null)
        {
            //需要计算一个当前的VP矩阵的逆矩阵，把NDC中的坐标返回计算世界坐标
            Matrix4x4 viewMatrix = m_camera.worldToCameraMatrix;
            Matrix4x4 projectionMatrix = m_camera.projectionMatrix;
            Matrix4x4 viewProjectInverseMatrix = projectionMatrix * viewMatrix;
            viewProjectInverseMatrix = viewProjectInverseMatrix.inverse;

            material.SetMatrix("viewProjectInverseMatrix", viewProjectInverseMatrix);

            Graphics.Blit(source, destination, material);
        }
        else
            Graphics.Blit(source, destination);

    }
}
