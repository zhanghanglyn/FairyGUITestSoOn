using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandBufferTest : MonoBehaviour {
    //渲染目标的obj
    public GameObject targetObject;
    Renderer targetRenderer;
    RenderTexture renderT;
    CommandBuffer commandBuffer;
    public Material targetMaterial;
    public Material afterEff;
    RenderTexture afterBuffer;
    Camera m_main;

    public float power = 1.3f;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        if (targetObject == null)
            return;

        //获取当前目标的高度宽度
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        float width = meshFilter.mesh.bounds.size.x * gameObject.transform.localScale.x;
        float height = meshFilter.mesh.bounds.size.z * gameObject.transform.localScale.z;

        targetRenderer = targetObject.GetComponent<Renderer>();
        renderT = RenderTexture.GetTemporary( 512, 512, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default,4);

        //设置COMMANDBuffer
        commandBuffer = new CommandBuffer();
        commandBuffer.SetRenderTarget(renderT);
        //初始颜色设置为灰色
        commandBuffer.ClearRenderTarget(true, true, Color.gray);
        Material materail = targetMaterial == null? targetObject.GetComponent<Renderer>().sharedMaterial : targetMaterial;

        commandBuffer.DrawRenderer(targetRenderer, materail);
        
        gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture = renderT;

        //加一个commandBuffer后期特效
        if (afterEff != null)
        {
            afterEff.SetFloat("_Power", power);

            afterBuffer = new RenderTexture(renderT);
            commandBuffer.Blit( renderT, afterBuffer, afterEff);
            gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture = afterBuffer;
        }

        //直接加入相机的CommandBuffer事件队列中
        Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);
        m_main = Camera.main;
    }

    // Update is called once per frame
    void Update () {
        afterEff.SetFloat("_Power", power);

    }

    private void OnDisable()
    {
        if (m_main != null)
            m_main.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);
        commandBuffer.Clear();
        renderT.Release();
        if (afterBuffer != null)
            afterBuffer.Release();
    }
}
