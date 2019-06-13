using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle
{
    //打包出AssetBundle的路径
    public static string assetBundleBuildPath = Application.dataPath + "/Resources/AssetBundle";

    public static BuildTarget target = // BuildTarget.StandaloneWindows;
#if UNITY_ANDROID
                           BuildTarget.Android;
#elif UNITY_IPHONE
                           BuildTarget.iOS;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                           BuildTarget.StandaloneWindows;
#else
                           BuildTarget.StandaloneWindows;
#endif

    /// <summary>
    /// 点击后，所有设置了AssetBundle名称的资源会被 分单个打包出来
    /// </summary>
    [MenuItem("AssetBundle/Build(Single)")]
    static void BuildSingle()
    {
        BuildPipeline.BuildAssetBundles(assetBundleBuildPath, BuildAssetBundleOptions.None , target);

        //AssetDatabase.GetDependencies()

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 将所有的选定资源绑定一起打包出来
    /// </summary>
    [MenuItem("AssetBundle/Build(Collect)")]
    static void BuildCollect()
    {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = "TestBundle1";

        Object[] selectObjects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        string[] objectPath = new string[selectObjects.Length];
            
        if (selectObjects.Length <= 0)
        {
            Debug.Log( "selected none!!!!!!!" );
            return;
        }

        for (int i = 0; i< objectPath.Length;i++)
        {
            objectPath[i] = AssetDatabase.GetAssetPath(selectObjects[i]);
        }
        buildMap[0].assetNames = objectPath;

        BuildPipeline.BuildAssetBundles(assetBundleBuildPath, buildMap, BuildAssetBundleOptions.None, target);
        AssetDatabase.Refresh();
    }

    /////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 自动根据选中文件夹中资源名字设置打包后的assetbundle名字并且打包
    /// </summary>
    [MenuItem("AssetBundle/Auto Name Select Build All")]
    static void AutoBuildAndSetNameSelect()
    {
        string path = EditorUtility.OpenFolderPanel("选择要打包的文件夹", Application.dataPath,
                "");

        if (path == null || path.Equals(""))
        {
            return;
        }

        string assetPath = assetBundleBuildPath;

        //创建打包文件夹
        if (!Directory.Exists(assetPath))
            Directory.CreateDirectory(assetPath);

        SetAssetNameAndBuild(path );

        BuildPipeline.BuildAssetBundles(assetPath, BuildAssetBundleOptions.None, target);

        //AssetDatabase.GetDependencies()

        AssetDatabase.Refresh();
    }

    static void SetAssetNameAndBuild( string _assetPath )
    {
        DirectoryInfo dir = new DirectoryInfo(_assetPath);
        FileSystemInfo[] files = dir.GetFileSystemInfos();

        //assetPath的路劲有可能是\\分割的也可能是/分割的，做个判断
        string assetbundleName = "default";
        if (_assetPath.Contains("/"))
            assetbundleName = _assetPath.Substring(_assetPath.LastIndexOf(@"/") + 1);
        else if (_assetPath.Contains(@"\"))
            assetbundleName = _assetPath.Substring(_assetPath.LastIndexOf(@"\") + 1);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)  //如果是文件夹，则将文件夹内的所有文件设置成一个assetbundle
            {
                SetAssetNameAndBuild(files[i].FullName);
            }
            else if(!files[i].Name.EndsWith(".meta"))//如果是文件的话，则设置AssetBundleName，并排除掉.meta文件
            {
                SetFolderInAssetBundle(files[i].FullName, assetbundleName);
            }
        }

    }
    /// <summary>
    /// 将一个文件夹内的所有文件，设置为同一个assetBundle
    /// </summary>
    static void SetFolderInAssetBundle(string _assetPath , string _assetBundleName)
    {
        /*DirectoryInfo dir = new DirectoryInfo(_assetPath);
        FileSystemInfo[] files = dir.GetFileSystemInfos();

        string assetbundleName = _assetPath.Substring(_assetPath.LastIndexOf(@"\") + 1);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].GetType() != typeof(DirectoryInfo) && !files[i].Name.EndsWith(".meta")) //如果是文件的话，则设置AssetBundleName，并排除掉.meta文件
            {*/
            //string assetbundleName = _assetPath.Substring(_assetPath.LastIndexOf(@"\") + 1);

                string importerPath = "Assets" + _assetPath.Substring(Application.dataPath.Length);  //这个路径必须是以Assets开始的路径
                AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath);  //得到Asset

                assetImporter.assetBundleName = _assetBundleName;
                
         //   }
        //}

    }
    ////////////////////////////////////////////////////////////////

    [MenuItem("AssetBundle/Delete All AssetBundle Name")]
    static void DeleteAllBundleName()
    {
        //强制删除所有AssetBundle名称
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();

        for (int i = 0; i < abNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }
        return;
    }

    [MenuItem("AssetBundle/Clear All Load Asset")]
    static void ClearAllLoadAsset()
    {
        AssetbundleManager.GetInstance().UnloadAll();
    }
}
