using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class TestList : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TestInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TestInit()
    {
        UIObjectFactory.SetLoaderExtension(typeof(ULoader));

        //添加一个加载路径，因为是在resource中 
        UIPackage.AddPackage("FairyGUI/Package1");

        GameObject uiPanelObj = GameObject.Find("UIPanel");
        if (uiPanelObj != null)
        {
            UIPanel uiPanel = uiPanelObj.GetComponent<UIPanel>();
            GComponent main_component = uiPanel.ui;
            if (main_component != null)
            {
                GList testList = main_component.GetChild("List") as GList;
                if (testList != null && testList.name == "List")
                {
                    //加入一个图片
                    GImage aImage = UIPackage.CreateObject("Package1", "bg_title").asImage;
                    if (aImage != null)
                    {
                        testList.AddChild(aImage);
                    }

                    
                    GComponent item_test = UIPackage.CreateObject("Package1", "ListTestItem2") as GComponent;
                    if (item_test != null)
                    {
                        testList.AddChild(item_test);
                    }

                    //创建一个子项
                    GComponent item = UIPackage.CreateObject("Package1", "listItem") as GComponent;
                    if (item != null)
                    {
                        ULoader loader = item.GetChild("Bg") as ULoader;
                        if (loader != null)
                        {
                            loader.Load( "ttt" , "btn_dhs.png" );
                        }

                        //创建一个头像并且尝试使用裁剪
                        Object npcObj = Resources.Load("Role/npc");
                        GameObject npcGameOjb = GameObject.Instantiate(npcObj) as GameObject;

                        npcGameOjb.transform.localScale = new Vector3(180, 180, 180);
                        npcGameOjb.transform.localPosition = new Vector3(npcGameOjb.transform.localPosition.x,
                            npcGameOjb.transform.localPosition.y - 100, npcGameOjb.transform.localPosition.z);

                        GGraph headCom = new GGraph();
                        GoWrapper wrapper = new GoWrapper();
                        wrapper.SetWrapTarget(npcGameOjb, true);
                        //需要修改shader才能让其正确显示成UI！！！！
                        wrapper.supportStencil = true;
                        headCom.SetNativeObject(wrapper);
                        headCom.SetPosition(item.width - 100, item.height - 50, headCom.z);

                        item.AddChild(headCom);

                        testList.AddChild(item);
                    }

                }
            }
        }
    }
}
