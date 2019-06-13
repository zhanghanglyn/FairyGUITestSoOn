using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 19.2.6
/// Assetbundle管理类，会根据平台的不同选择对应的加载路径；
/// 根据是打包资源还是下载资源确定加载方式为本地streamingAssetsPath或是更新资源Application.persistentDataPath
/// 应该会有一个资源对应表 但目前并没有设计
/// </summary>
public class AssetbundleManager : MonoBehaviour{

    static AssetbundleManager instance;
    static GameObject AssetbundleObj;

    public string bundleStreamingPath;  //streaming加载路径
    public string bundleStreamingWWWPath;  //streaming加载WWW路径

    public string bundlePersistentDataPath;//persistentDataPath加载路径
    public string bundlePersistentDataWWWPath;//persistentDataWWWPath加载路径

    //System.Action<AssetBundle> m_LoadCallBack;  //加载完毕后的回调

    private Dictionary<string, AssetBundle> m_AssetBundleList; //已经加载过的assetBundle字典

    public static AssetbundleManager GetInstance()
    {
        if (AssetbundleObj == null)
        {
            AssetbundleObj = new GameObject("AssetbundleManager");
            DontDestroyOnLoad(AssetbundleObj);
            if (instance == null)
            {
                instance = AssetbundleObj.AddComponent<AssetbundleManager>();
                instance.init();
            }
        }
        return instance;
    }

    private void OnStop()
    {
        //清除所有AssetBundle镜像
        if (m_AssetBundleList != null)
        {
            foreach (var item in m_AssetBundleList)
            {
                if (item.Value != null)
                {
                    item.Value.Unload(false);
                }
            }
            m_AssetBundleList.Clear();
        }
    }

    void init()
    {
        //streaming加载loadFromFile路径
        bundleStreamingPath =
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
        bundleStreamingWWWPath =
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
        bundlePersistentDataWWWPath =
#if UNITY_ANDROID
                           "file://" + Application.persistentDataPath + "/" + PathSetting.assetBundlePath;
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/" + PathSetting.assetBundlePath;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                            Application.dataPath + "/" + PathSetting.assetBundlePath;
#else
                            string.Empty;  
#endif

        bundlePersistentDataPath =
#if UNITY_ANDROID
                           Application.persistentDataPath + "/" + PathSetting.assetBundlePath;
#elif UNITY_IPHONE
                            Application.dataPath + "/Raw/" + PathSetting.assetBundlePath;  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                            Application.dataPath + "/" + PathSetting.assetBundlePath;
#else
                            string.Empty;  
#endif

        m_AssetBundleList = new Dictionary<string, AssetBundle>();
    }

    /// <summary>
    /// 异步加载assetBundle资源，如果有依赖资源的话，异步加载就没法一次性控制，比较麻烦
    /// <param name="_bReLoad">是否重新加载</param>
    /// </summary>
    public void LoadAsynchronous(string _assetName , System.Action<AssetBundle> _callBack , bool _bReLoad = false)
    {
        if (_bReLoad == true)
        {
            Unload(_assetName);
        }

#if UNITY_ANDROID
        StartCoroutine(LoadByStreamingAssetsPathWWW(_assetName , _callBack));
#elif UNITY_IPHONE
        StartCoroutine(LoadByStreamingAssetsPathWWW(_assetName , _callBack));
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        StartCoroutine(LoadByStreamingAssetsPathWWW(_assetName, _callBack));
#else
        
#endif
    }

    /// <summary>
    /// 一开始即打入包中的资源，从streamingAssetsPath中读取,先去本地查找，如果没有，则去打包后的persistent中查找;
    /// </summary>
    IEnumerator LoadByStreamingAssetsPathWWW( string _assetPath, System.Action<AssetBundle> _callBack)
    {
        //如果已经加载过，则直接返回
        if (CheckBundleList(_assetPath))
        {
            AssetBundle bundle = m_AssetBundleList[_assetPath];
            if (bundle != null)
            {
                _callBack(bundle);
                yield break;
            }
        }


        WWW asset = new WWW(bundleStreamingWWWPath + _assetPath);
        yield return asset;
        
        if (asset.isDone)
        {
            AssetBundle bundle = asset.assetBundle;
            if (bundle != null)
            {
                //为保存过的加载入字典中
                m_AssetBundleList.Add(_assetPath, bundle);
                if (_callBack != null)
                {
                    _callBack(bundle);
                }
            }
            else
            {
                Debug.Log("AssetBundle is null!!!!!!!");
                _callBack(null);
                StartCoroutine(LoadByPersistentPathWWW(_assetPath, _callBack));
            }
        }
    }

    IEnumerator LoadByPersistentPathWWW(string _assetPath, System.Action<AssetBundle> _callBack)
    {
        //如果已经加载过，则直接返回
        if (CheckBundleList(_assetPath))
        {
            AssetBundle bundle = m_AssetBundleList[_assetPath];
            if (bundle != null)
            {
                _callBack(bundle);
                yield break;
            }
        }

        WWW asset = new WWW(bundlePersistentDataWWWPath + _assetPath);
        yield return asset;

        if (asset.isDone)
        {
            AssetBundle bundle = asset.assetBundle;
            if (bundle != null)
            {
                m_AssetBundleList.Add(_assetPath, bundle);

                if (_callBack != null)
                {
                    _callBack(bundle);
                }
            }
            else
            {
                Debug.Log("AssetBundle is null!!!!!!!");
                _callBack(null);
                //ReadAssetTest.debug_word2 = bundlePersistentDataWWWPath + _assetPath;
            }
        }
    }
    /////////////////异步加载资源END///////////////////
    //////////////////////////////////////////////////

    /// <summary>
    /// 同步加载AssetBundle资源
    /// </summary>
    /// <param name="_assetName"></param>
    /// <param name="_bReLoad">是否重新加载</param>
    /// <returns></returns>

    public AssetBundle Load(string _assetName , bool _bReLoad = false)
    {
        if (_bReLoad == true)
        {
            Unload(_assetName);
        }

        AssetBundle result = 
#if UNITY_ANDROID
        LoadByStreamingAssetsPath(_assetName);
#elif UNITY_IPHONE
        LoadByStreamingAssetsPath(_assetName);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        LoadByStreamingAssetsPath(_assetName);
#else
        LoadByStreamingAssetsPath(_assetName);
#endif
        return result;
    }

    /// <summary>
    /// 一开始即打入包中的资源，从streamingAssetsPath中读取,先去本地查找，如果没有，则去打包后的persistent中查找;
    /// </summary>
    AssetBundle LoadByStreamingAssetsPath(string _assetPath)
    {
        //如果已经加载过，则直接返回
        if (CheckBundleList(_assetPath))
        {
            AssetBundle bundle = m_AssetBundleList[_assetPath];
            if (bundle != null)
            {
                return bundle;
            }
        }

        if (System.IO.File.Exists(bundleStreamingPath + _assetPath))
        {
            var temp_asset = AssetBundle.LoadFromFile(bundleStreamingPath + _assetPath);
            AssetBundle asset = null;
            if (temp_asset != null)
            {
                asset = (AssetBundle)temp_asset;
            }

            if (asset != null)
            {
                m_AssetBundleList.Add(_assetPath, asset);
                return asset;
            }
            else
            {
                //Debug.Log("AssetBundle is null!!!!!!!");
                return LoadByPersistentPath(_assetPath);
            }
        }
        else
        {
            //Debug.Log("AssetBundle is null!!!!!!!");
            return LoadByPersistentPath(_assetPath);
        }
    }

    AssetBundle LoadByPersistentPath(string _assetPath)
    {
        //如果已经加载过，则直接返回
        if (CheckBundleList(_assetPath))
        {
            AssetBundle bundle = m_AssetBundleList[_assetPath];
            if (bundle != null)
            {
                return bundle;
            }
        }

        TestUI.out_log2 = "path ::   " + bundlePersistentDataPath + _assetPath;

        //AssetBundle asset = AssetBundle.LoadFromFile(bundlePersistentDataPath + _assetPath) as AssetBundle;
        if (System.IO.File.Exists(bundlePersistentDataPath + _assetPath))
        {
            var temp_asset = AssetBundle.LoadFromFile(bundlePersistentDataPath + _assetPath);
            AssetBundle asset = null;
            if (temp_asset != null)
            {
                asset = (AssetBundle)temp_asset;
            }

            if (asset != null)
            {
                m_AssetBundleList.Add(_assetPath, asset);
                return asset;
            }
        }

        return null;
    }

    /// <summary>
    /// 会去判断该key是否存在，如果存在但是AssetBundle镜像却已经卸载了，则清除该keyval
    /// </summary>
    /// <param name="_key"></param>
    bool CheckBundleList( string _key )
    {
        if (m_AssetBundleList.ContainsKey(_key))
        {
            if (m_AssetBundleList[_key])
            {
                return true;
            }
            else
            {
                m_AssetBundleList.Remove(_key);
                return false;
            }
        }
        return false;
    }

    /// <summary>
    ///  同步加载，根据传入的Manifest文件名和要加载的assetBundle名加载Bundle以及依赖的bundle
    /// </summary>
    /// <param name="_assetPath">要加载的assetbundle路径,类型于xxxx/name ，最终会取出name的依赖</param>
    /// <param name="_manifestPath">包含manifest的主文件assetbundle路径</param>
    /// <param name="_dependenPath">默认依赖文件路径</param>
    /// <returns></returns>
    public AssetBundle LoadAssetDependence( string _assetPath , string _manifestPath ,string _dependenPath)
    {
        if (_assetPath == null || _assetPath.Equals("") || _manifestPath == null || _manifestPath.Equals(""))
        {
            return null;
        }

        string assetName = _assetPath.Substring(_assetPath.LastIndexOf(@"/") + 1);

        AssetBundle temp_MainFest = Load(_manifestPath);
        if (temp_MainFest)
        {
            AssetBundleManifest manifest = temp_MainFest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (manifest != null)
            {
                string[] dependence = manifest.GetAllDependencies(assetName);
                foreach (string name in dependence)
                {
                    //Debug.Log("####   : " + name);
                    if (_dependenPath == null || _dependenPath.Equals(""))
                        Load( name);
                    else
                        Load(_dependenPath + "/" + name);
                }
                return Load(_assetPath);
            }
        }

        return null;
    }

    /// <summary>
    /// 根据传入的AssetBundle名称，获取其中的lua文件并将其写入_luaPath中的路径，供之后的文件加载
    /// 重启后读取该lua依然存在，不需要重新冲assetBundle中获取
    /// 同步加载
    /// </summary>
    /// <param name="_assetName">AssetBundle名称</param>
    /// <param name="_luaName">需要从中加载的lua Name,为xxx.lua.bytes格式</param>
    /// <param name="_luaPath">要将lua文件写入的路径（该路径也要加入SearchPath后才可以加载记得）</param>
    /// <param name="_bReLoad">是否重新加载</param>
    public void LoadLuaAndCreateFile(string _assetName, string _luaName, string _luaPath, bool _bReLoad = false)
    {
        AssetBundle temp_bunder = Load(_assetName, _bReLoad);
        if (temp_bunder != null)
        {
            TextAsset temp = temp_bunder.LoadAsset<TextAsset>(_luaName);
            if (temp != null && temp.bytes != null)
            {
                string save_path = _luaPath;
                if ( string.IsNullOrEmpty(save_path) == false)
                {
                    if (!Directory.Exists(save_path))
                        Directory.CreateDirectory(save_path);

                    //截取lua名
                    string saveName = _luaName.Substring(0, _luaName.LastIndexOf(@"."));

                    FileInfo fileInfo = new FileInfo(save_path + "/" + saveName);
                    Stream stream = fileInfo.Create();
                    stream.Write(temp.bytes, 0, temp.bytes.Length);
                    stream.Close();
                }
            }
        }
    }

    /// <summary>
    /// 获取对应的依赖信息
    /// </summary>
    /// <param name="_manifestPath">包含manifest的主文件assetbundle路径</param>
    /// <param name="_assetName">要取得信息的assetBundle名字</param>
    /// <returns></returns>
    public string[] GetDependence(string _manifestPath , string _assetName)
    {
        if (_manifestPath == null || _manifestPath.Equals(""))
        {
            return null;
        }

        AssetBundle temp_MainFest = Load(_manifestPath);
        if (temp_MainFest)
        {
            AssetBundleManifest manifest = temp_MainFest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (manifest != null)
            {
                return manifest.GetAllDependencies(_assetName);
            }
        }

        return null;
    }

    /// <summary>
    /// 释放对应的assetBundle镜像资源
    /// </summary>
    /// <param name="_assetPath"></param>
    /// <param name="_bUnloadAll">是否清除Load加载出来的Asset</param>
    public void Unload(string _assetPath , bool _bUnloadAll = false)
    {
        if (m_AssetBundleList.ContainsKey(_assetPath) && m_AssetBundleList[_assetPath]!=null)
        {
            m_AssetBundleList[_assetPath].Unload(_bUnloadAll);

            //并且把没有使用的已加载Asset释放  比较消耗内存
            //Resources.UnloadUnusedAssets();
        }
    }

    public void UnloadAll ()
    {
        foreach(var item in m_AssetBundleList)
        {
            item.Value.Unload(false);

            //并且把没有使用的已加载Asset释放  比较消耗内存
            //Resources.UnloadUnusedAssets();
        }
        m_AssetBundleList.Clear();
    }

}
