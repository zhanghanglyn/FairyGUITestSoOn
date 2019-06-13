using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthScan : MonoBehaviour {

    Camera m_camera;
    public Material m_material;
    Shader m_shader;
    [Range (0,1)]
    public float _ScanDepth = 0.1f;
    [Range(0, 0.5f)]
    public float _Width = 0.01f;
    [Range(0, 0.5f)]
    public float _Warp = 0.01f;


    private void Awake()
    {
        m_camera = GetComponent<Camera>();
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
        if (m_material != null)
        {
            m_material.SetFloat("_ScanDepth",_ScanDepth);
            m_material.SetFloat("_Width", _Width);
            m_material.SetFloat("_Warp", _Warp);
            Graphics.Blit(source, destination, m_material);
        }
    }
}
