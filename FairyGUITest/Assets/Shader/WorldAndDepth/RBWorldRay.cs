using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBWorldRay : MonoBehaviour {

    Camera m_camera;
    public Material material;

    private void Awake()
    {
        if (m_camera == null)
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
        if (material != null && m_camera != null)
        {
            /*float fov = m_camera.fieldOfView;
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

            Matrix4x4 rayMatrix = Matrix4x4.identity;
            rayMatrix.SetRow(0, RayLT);
            rayMatrix.SetRow(1, RayLB);
            rayMatrix.SetRow(2, RayRT);
            rayMatrix.SetRow(3, RayRB);
            material.SetMatrix( "_RayMatrix", rayMatrix);*/

            Camera camera = m_camera;
            Transform cameraTransform = camera.transform;
            Matrix4x4 frustumCorners = Matrix4x4.identity;

            float fov = camera.fieldOfView;
            float near = camera.nearClipPlane;
            float aspect = camera.aspect;

            float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            Vector3 toRight = cameraTransform.right * halfHeight * aspect;
            Vector3 toTop = cameraTransform.up * halfHeight;

            Vector3 topLeft = cameraTransform.forward * near + toTop - toRight;
            float scale = topLeft.magnitude / near;

            topLeft.Normalize();
            topLeft *= scale;

            Vector3 topRight = cameraTransform.forward * near + toRight + toTop;
            topRight.Normalize();
            topRight *= scale;

            Vector3 bottomLeft = cameraTransform.forward * near - toTop - toRight;
            bottomLeft.Normalize();
            bottomLeft *= scale;

            Vector3 bottomRight = cameraTransform.forward * near + toRight - toTop;
            bottomRight.Normalize();
            bottomRight *= scale;

            frustumCorners.SetRow(0, bottomLeft);
            frustumCorners.SetRow(1, bottomRight);
            frustumCorners.SetRow(2, topRight);
            frustumCorners.SetRow(3, topLeft);

            material.SetMatrix("_RayMatrix", frustumCorners);

            Graphics.Blit(source, destination, material);

        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }
}
