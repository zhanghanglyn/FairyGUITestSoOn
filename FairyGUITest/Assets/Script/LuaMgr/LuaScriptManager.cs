using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

//负责load加载lua等功能
public class LuaScriptManager {

    //大路径一般不用设置！只需要设置相对路径即可
    public static void LoadFile(string fileName)
    {
        if (fileName == null || string.Equals(fileName, ""))
        {
            return;
        }
        //string filePath = LuaManager.Instance.getCurLuaPath() + fileName;
        LuaManager.GetInstance().LoadFile(fileName);
    }


}
