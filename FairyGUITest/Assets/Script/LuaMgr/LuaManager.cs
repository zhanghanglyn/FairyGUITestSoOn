using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement;
#endif

//自定义的lua管理器
public class LuaManager : LuaClient {

    //该路径一般在初始话时进行设置
    public string MyLuaFilePath;

    static GameObject LuaManagerObj;
    static LuaManager luaMInstance;

    public static LuaManager GetInstance()
    {
        if (LuaManagerObj == null)
            LuaManagerObj = new GameObject("LuaManagerObj");

        DontDestroyOnLoad(LuaManagerObj);

        if (LuaManagerObj != null && luaMInstance == null)
        {
            luaMInstance = LuaManagerObj.AddComponent<LuaManager>();
        }

        return luaMInstance;
    }

    new void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
    new void Awake()
    {
        base.Awake();
    }

    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    //屏蔽，例子不需要运行
    protected override void CallMain() { }

    /// <summary>
    /// ///////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="filePath"></param>

    //额外加一个设置启动lua的方法以及加载lua的方法
    public void LoadFile(string filePath)
    {
        luaState.DoFile(filePath);
    }

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();
    }

    protected override void StartMain()
    {
        luaState.DoFile(ConstSetting.LuaMainName);
        levelLoaded = luaState.GetFunction("OnLevelWasLoaded");
        CallMain();
    }

    /// <summary>
    /// 添加Lua搜索路径
    /// </summary>
    /// <param name="_path"></param>
    public void AddSearchPath(string _path)
    {
        luaState.AddSearchPath(_path);
    }

    //根据函数名获取Lua函数
    public LuaFunction GetFuncByString( string _funcName )
    {
        LuaFunction func = luaState.GetFunction(_funcName);
        if (func == null)
            return null;
        return func;
    }

    /// <summary>
    /// 获取luatable
    /// </summary>
    /// <returns></returns>
    public LuaTable GetLuaTableByName(string _tableName)
    {
        LuaTable table = luaState.GetTable(_tableName);
        return table;
    }

    protected new void OnDestroy()
    {
        base.Destroy();
        
    }
}
