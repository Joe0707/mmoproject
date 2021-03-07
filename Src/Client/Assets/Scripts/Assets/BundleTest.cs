
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class BundleTest:MonoBehaviour{
    void Start()
    {
        StartCoroutine(LoadCharacter("Archer"));
    }

    IEnumerator LoadCharacter(string assetBundleName)
    {
        string uri = "file:///Assets/AssetBundles/character/"+assetBundleName.ToLower()+".asset";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);
        yield return request.SendWebRequest();
        string uri2 = "file:///Assets/AssetBundles/character/"+assetBundleName.ToLower()+"_mat.asset";
        UnityWebRequest request2 = UnityWebRequestAssetBundle.GetAssetBundle(uri);
        yield return request2.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                AssetBundle bundle2 = DownloadHandlerAssetBundle.GetContent(request2);
        GameObject gameObject = bundle.LoadAsset<GameObject>(assetBundleName);
        Instantiate(gameObject);
    }
}