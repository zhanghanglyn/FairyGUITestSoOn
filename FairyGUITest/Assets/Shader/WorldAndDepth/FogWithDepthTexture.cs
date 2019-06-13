using UnityEngine;
using System.Collections;

public class FogWithDepthTexture : PostEffectsBase {

	public Shader fogShader;
	private Material fogMaterial = null;

	public Material material {  
		get {
			fogMaterial = CheckShaderAndCreateMaterial(fogShader, fogMaterial);
			return fogMaterial;
		}  
	}

	private Camera myCamera;
	public Camera camera {
		get {
			if (myCamera == null) {
				myCamera = GetComponent<Camera>();
			}
			return myCamera;
		}
	}

	private Transform myCameraTransform;
	public Transform cameraTransform {
		get {
			if (myCameraTransform == null) {
				myCameraTransform = camera.transform;
			}

			return myCameraTransform;
		}
	}

	[Range(0.0f, 3.0f)]
	public float fogDensity = 1.0f;

	public Color fogColor = Color.white;

	public float fogStart = 0.0f;
	public float fogEnd = 2.0f;

	void OnEnable() {
		camera.depthTextureMode |= DepthTextureMode.Depth;
	}
	
	void OnRenderImage (RenderTexture src, RenderTexture dest) {
		if (material != null) {
			/*Matrix4x4 frustumCorners = Matrix4x4.identity;

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
			frustumCorners.SetRow(3, topLeft);*/

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
            RayLT*= scale;
            RayLB.Normalize();
            RayLB *= scale;
            RayRT.Normalize();
            RayRT *= scale;
            RayRB.Normalize();
            RayRB *= scale;

            Matrix4x4 rayMatrix = Matrix4x4.identity;
            /*rayMatrix.SetRow(0, RayLB);
            rayMatrix.SetRow(1, RayRB);
            rayMatrix.SetRow(2, RayRT);
            rayMatrix.SetRow(3, RayLT);*/

            rayMatrix.SetRow(0, RayLT);
            rayMatrix.SetRow(1, RayLB);
            rayMatrix.SetRow(2, RayRT);
            rayMatrix.SetRow(3, RayRB);

            material.SetMatrix("_RayMatrix", rayMatrix);

			/*material.SetFloat("_FogDensity", fogDensity);
			material.SetColor("_FogColor", fogColor);
			material.SetFloat("_FogStart", fogStart);
			material.SetFloat("_FogEnd", fogEnd);*/

			Graphics.Blit (src, dest, material);
		} else {
			Graphics.Blit(src, dest);
		}
	}
}
