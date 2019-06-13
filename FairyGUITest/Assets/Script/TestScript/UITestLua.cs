using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class UITestLua : MonoBehaviour {

	// Use this for initialization
	void Start () {
        init();

    }
	
	// Update is called once per frame
	void Update () {

    }

    private void init()
    {
        UIObjectFactory.SetLoaderExtension(typeof(ULoader));

        LuaManager.GetInstance().LoadFile("testFairyGUI.lua");
    }
    
}
