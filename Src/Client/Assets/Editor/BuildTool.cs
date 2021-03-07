using UnityEditor;
using UnityEngine;
using System.IO;
//打包工具
public class BuildTool{
    [MenuItem("BuildTool/Clear AssetBundles")]
    static void ClearAllAssetBundles()
    {
        var allBundles = AssetDatabase.GetAllAssetBundleNames();
        foreach(var bundle in allBundles)
        {
            AssetDatabase.RemoveAssetBundleName(bundle,true);
            Debug.LogFormat("BuildTool:Remove Old Bundle:{0}",bundle);
        }
    }

    [MenuItem("BuildTool/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if(!Directory.Exists(assetBundleDirectory)){
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
    }
}