using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraEffTest : MonoBehaviour {

    public Material screenMaterial;
    Camera camera;

    private void Start()
    {
        if (camera == null)
            camera = GetComponent<Camera>();

        camera.depthTextureMode |= DepthTextureMode.Depth;
        camera.depthTextureMode |= DepthTextureMode.DepthNormals;
    }

    /*private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (camera == null)
            camera = GetComponent<Camera>();

        camera.depthTextureMode = DepthTextureMode.Depth;

        if (screenMaterial != null)
            Graphics.Blit( source , destination, screenMaterial );

    }*/

}
