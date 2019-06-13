using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    Camera camera;

    private void Awake()
    {
        if (camera == null)
            camera = GetComponent<Camera>();

        camera.depthTextureMode |= DepthTextureMode.Depth;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
