using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来存放各种配置路径等
/// </summary>
public class PathSetting
{

    //在加载AssetBundle时的路径，基于Application.dataPath + "!assets/类似之后的路径
    public static string assetBundlePath =
#if UNITY_ANDROID
                           "AssetBundle/";
#elif UNITY_IPHONE
                           "AssetBundle/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                           "Resources/AssetBundle/";
#else
                            string.Empty;  
#endif

    public static string bundleStreamingPath =
#if UNITY_ANDROID
                           Application.dataPath + "!assets/"+ PathSetting.assetBundlePath;
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/"+ PathSetting.assetBundlePath;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                            Application.dataPath + "/" + PathSetting.assetBundlePath;
#else
                            string.Empty;  
#endif
    //streaming加载WWW路径
    public static string bundleStreamingWWWPath =
#if UNITY_ANDROID
                           Application.streamingAssetsPath + "/" + PathSetting.assetBundlePath;
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/" + PathSetting.assetBundlePath;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                            Application.dataPath + "/" + PathSetting.assetBundlePath;
#else
                            string.Empty;  
#endif

    //persistentDataWWWPath加载路径，该路径在电脑端无效
    public static string bundlePersistentDataWWWPath =
#if UNITY_ANDROID
                           "file://" + Application.persistentDataPath + "/" + PathSetting.assetBundlePath;
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/" + PathSetting.assetBundlePath;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                            Application.dataPath + "/" + PathSetting.assetBundlePath;
#else
                            string.Empty;  
#endif

    public static string bundlePersistentDataPath =
#if UNITY_ANDROID
                           Application.persistentDataPath + "/" + PathSetting.assetBundlePath;
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/" + PathSetting.assetBundlePath;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                            Application.dataPath + "/" + PathSetting.assetBundlePath;
#else
                            string.Empty;  
#endif

    //lua文件的路径
    public static string LuaMainPath =
#if UNITY_ANDROID
                           Application.dataPath + "/!assets/" + "Lua/";
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/Lua/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                           Application.dataPath + "/Resources/Lua/";
#else
                            string.Empty;  
#endif

    //下载的lua文件的路径
    public static string LuaMainPeristPath =
#if UNITY_ANDROID
                           Application.persistentDataPath + "/Lua/";
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/Lua/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                           Application.dataPath + "/Resources/Lua/";
#else
                            string.Empty;  
#endif

    public static string LuaToLuaDir =
#if UNITY_ANDROID
                           Application.streamingAssetsPath + "/Lua/";
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/Lua/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                           Application.dataPath + "/ToLua/Lua";
#else
                            string.Empty;  
#endif

    //UI路径相关
    public static string UIPrefabPath = "ui/uiprefeb";
    public static string CanvasName = "UCanvas";
    public static string LabelName = "ULabel";
    public static string ListName = "UList";
    public static string ComponentName = "UComponent";
    public static string ImageName = "UImage";


}
