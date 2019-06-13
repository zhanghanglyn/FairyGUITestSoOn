using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class ULoader : GLoader {

    string _assetName = null;   //在设置了AB的url以后，还需要texture的name进行加载

    /// <summary>
    /// 外部可以直接调用Load传入AssetBunlde的name以及资源的名字进行加载
    /// </summary>
    /// <param name="_abName">AB包的路径</param>
    /// <param name="_assetName">需要加载的资源名称</param>
    public void Load( string _abName , string _assetName )
    {
        this._assetName = _assetName;
        url = _abName;
    }

    override protected void LoadExternal()
    {
        /*
        开始外部载入，地址在url属性
        载入完成后调用OnExternalLoadSuccess
        载入失败调用OnExternalLoadFailed
        注意：如果是外部载入，在载入结束后，调用OnExternalLoadSuccess或OnExternalLoadFailed前，
        比较严谨的做法是先检查url属性是否已经和这个载入的内容不相符。
        如果不相符，表示loader已经被修改了。
        这种情况下应该放弃调用OnExternalLoadSuccess或OnExternalLoadFailed。
        */
        AssetBundle temp_ab = AssetbundleManager.GetInstance().Load(url);
        if (temp_ab == null)
        {
            onExternalLoadFailed();
        }
        else
        {
            Texture2D tex = temp_ab.LoadAsset<Texture2D>(_assetName);
            if (tex != null)
                onExternalLoadSuccess(new NTexture(tex));
            else
                onExternalLoadFailed();
        }
    }

    override protected void FreeExternal(NTexture texture)
    {
        //释放外部载入的资源
    }

}
