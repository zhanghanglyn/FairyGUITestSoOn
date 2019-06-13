using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//把这个脚本单独挂在模型上，用COMMANDBUFFER强制在affterEff之后再绘制模型，避免后期特效加在模型上
//提高效率

public class CBAfterEff : MonoBehaviour {

    Renderer m_renderer;
    CommandBuffer commandBuffer;
    public Material objMaterial;
    Camera m_main;

    private void OnEnable()
    {
        m_renderer = this.GetComponent<Renderer>();

        if (m_renderer != null)
        {
            commandBuffer = new CommandBuffer();
            commandBuffer.DrawRenderer(m_renderer, m_renderer.sharedMaterial);

            m_main = Camera.main;
            Camera.main.AddCommandBuffer(CameraEvent.AfterImageEffects, commandBuffer);

        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDisable()
    {
        if (m_renderer != null && m_main != null)
        {
            //m_renderer.enabled = true;
            m_main.RemoveCommandBuffer(CameraEvent.AfterImageEffects, commandBuffer);
            commandBuffer.Clear();
        }
    }
}
