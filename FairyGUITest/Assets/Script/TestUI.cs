using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class TestUI : MonoBehaviour {

    string out_log = "";
    public static string out_log2 = "";

    GComponent storyPanel;

    // Use this for initialization
    void Start () {
        //Init();
        down();
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnGUI()
    {
        if( GUI.Button( new Rect( 100,100,150,100 ),"尝试尝试" ))
        {
            Init();
        }

        GUI.Label(new Rect(100, 250, 150, 100), out_log);
        GUI.Label(new Rect(100, 350, 150, 100), out_log2);
    }

    private void Init()
    {

        AssetBundle desc = AssetbundleManager.GetInstance().Load("11");
        AssetBundle resource = AssetbundleManager.GetInstance().Load("22");

        if (desc != null && resource != null)
        {
            UIPackage.AddPackage(desc, resource);

            GameObject uiPanelObj = GameObject.Find("UIPanel");
            if (uiPanelObj != null)
            {
                UIPanel uiPanel = uiPanelObj.GetComponent<UIPanel>();
                //GComponent gComponent = uiPanel.ui;
                if (uiPanel != null)
                {
                    storyPanel = UIPackage.CreateObject("Package1", "Component1").asCom;

                    GRoot.inst.AddChild(storyPanel);

                    storyPanel.onTouchBegin.Add(ComponentTouchBegin);

                    setTouch();

                    Add3D();
                }
            }
            else
            {
                out_log = "uiPanelObj is null!!!!!!!!!!";
            }
        }
        else
        {
            out_log = "AssetBundle is null!!!!!!!!!!      ";
        }
        
    }

    private void down()
    {
        string test_url = "http://192.168.101.69/";
        string save_path = Application.persistentDataPath;

        UpdateManager.GetInstance().StartDown(test_url + "desc_bundle", save_path + "/" + PathSetting.assetBundlePath, "11");
        UpdateManager.GetInstance().StartDown(test_url + "package1resource", save_path + "/" + PathSetting.assetBundlePath, "22");

        UIConfig.defaultFont = "fzcyjt";
        //Font myFont = myBundle.LoadAsset<Font>(name);
        //FontManager.RegisterFont(new DynamicFont("字体名称", myFont), "字体名称");
    }

    void Add3D()
    {
        Object npcObj = Resources.Load("Role/npc");
        GameObject npcGameOjb = GameObject.Instantiate(npcObj) as GameObject;

        npcGameOjb.transform.localPosition = new Vector3(61, -89, 1000);
        npcGameOjb.transform.localScale = new Vector3(180, 180, 180);
        npcGameOjb.transform.localEulerAngles = new Vector3(0, 100, 0);

        if (storyPanel != null)
        {
            GGraph holder = new GGraph();
            storyPanel.AddChild(holder);
            /*holder.width = 100;
            holder.height = 100;
            holder.SetSize(100, 100);
            holder.onTouchBegin.Add(HolderTouchBegin);*/

            GGraph touchHolder = new GGraph();
            touchHolder.DrawRect(150, 150, 1, Color.black, Color.green);
            touchHolder.sortingOrder = 200;
            storyPanel.AddChild(touchHolder);
            touchHolder.xy = new Vector2(200, 500);
            touchHolder.onTouchBegin.Add(HolderTouchBegin);
            touchHolder.alpha = 100;

            GoWrapper wrapper = new GoWrapper();
            wrapper.SetWrapTarget(npcGameOjb, true);
            holder.SetNativeObject(wrapper);
            holder.SetPosition(200, 500,0);
        }
    }

    //////////////////
    ///触摸相关////////
    void setTouch()
    {
        Stage.inst.onTouchBegin.Add(OntouchBegin);
    }

    void OntouchBegin( EventContext _data )
    {
        Debug.Log("Touch Begin!!!");
        
    }

    void ComponentTouchBegin(EventContext _data)
    {
        Debug.Log("Component TouchBegin @!!");
    }

    void HolderTouchBegin(EventContext _data)
    {
        Debug.Log("HolderTouch Begin!!!!");
    }
}
