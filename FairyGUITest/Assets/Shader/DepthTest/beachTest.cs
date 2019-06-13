using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beachTest : MonoBehaviour {

    public Camera m_camera;

    private void Awake()
    {
        if (m_camera == null)
        {
            m_camera = FindObjectOfType<Camera>();
        }
        m_camera.depthTextureMode = DepthTextureMode.Depth;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
