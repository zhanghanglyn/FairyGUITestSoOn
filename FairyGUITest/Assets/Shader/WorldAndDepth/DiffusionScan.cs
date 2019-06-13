using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionScan : MonoBehaviour {

    Camera camera;
    public Material material;
    public Vector3 scanCenter;
    public float _ScanSpeed;
    public float _ScanWidth;
    public Color _Color;


    private void Awake()
    {
        if (camera == null)
            camera = GetComponent<Camera>();

        camera.depthTextureMode |= DepthTextureMode.Depth;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null && camera != null)
        {

            Camera m_camera = camera;
            float fov = m_camera.fieldOfView;
            float near = m_camera.nearClipPlane;
            float far = m_camera.farClipPlane;
            float aspect = m_camera.aspect;
            float halfH = Mathf.Tan(fov / 2 * Mathf.Deg2Rad) * near;
            float halfW = halfH * aspect;

            Vector3 up = m_camera.transform.up * halfH;
            Vector3 right = halfW * m_camera.transform.right;
            Vector3 left = halfW * -m_camera.transform.right;
            Vector3 down = -m_camera.transform.up * halfH;
            Vector3 forward = m_camera.transform.forward * near;

            //四个角的射线
            Vector3 RayLT = forward + up - right;
            Vector3 RayLB = forward - up - right;
            Vector3 RayRT = forward + right + up;
            Vector3 RayRB = forward + right - up;

            float scale = RayLT.magnitude / near;

            RayLT.Normalize();
            RayLT *= scale;
            RayLB.Normalize();
            RayLB *= scale;
            RayRT.Normalize();
            RayRT *= scale;
            RayRB.Normalize();
            RayRB *= scale;

            Matrix4x4 rayMatrix = Matrix4x4.identity;

            rayMatrix.SetRow(0, RayLT);
            rayMatrix.SetRow(1, RayLB);
            rayMatrix.SetRow(2, RayRT);
            rayMatrix.SetRow(3, RayRB);

            material.SetMatrix("_RayMatrix", rayMatrix);

            material.SetVector("_ScanCenter", scanCenter);

            material.SetFloat("_ScanSpeed", _ScanSpeed);
            material.SetFloat("_ScanWidth", _ScanWidth);
            material.SetColor("_Color", _Color);

            Graphics.Blit(source, destination , material);
        }
        else
            Graphics.Blit(source, destination);

    }
}
