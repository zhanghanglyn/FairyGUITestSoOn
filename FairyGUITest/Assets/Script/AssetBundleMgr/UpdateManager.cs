using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Application.persistentDataPath 
/// </summary>
public class UpdateManager : MonoBehaviour{

    static UpdateManager instance;
    static GameObject updateObj;

    public static UpdateManager GetInstance()
    {
        if (updateObj == null)
        {
            updateObj = new GameObject("UpdateManager");
            if (instance == null)
            {
                instance = updateObj.AddComponent<UpdateManager>();
            }
        }
        return instance;
    }

	public void StartDown( string _url , string _savePath , string _fileName = "")
    {
        StartCoroutine(DownSource(_url, _savePath, _fileName));
    }

    public void StartDownByList( Dictionary<string , string> _url_path)
    {


    }
    
    /// <summary>
    /// 根据url下载
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_savePath"></param>
    /// <param name="_fileName"></param>
    /// <returns></returns>
    IEnumerator DownSource(string _url, string _savePath, string _fileName)
    {

        WWW www = new WWW(_url);
        yield return www;

        if (www.isDone)
        {
            if (www != null && www.bytes != null)
            {
                byte[] source = www.bytes;
                //判断本地文件夹是否存在,如果不存在，创建文件夹
                if (!Directory.Exists(_savePath))
                    Directory.CreateDirectory(_savePath);

                FileInfo fileInfo = new FileInfo(_savePath + "/" + _fileName);
                Stream stream = fileInfo.Create();
                stream.Write(source, 0, source.Length);
                
                stream.Close();
                stream.Dispose();
            }
        }
    }

}
