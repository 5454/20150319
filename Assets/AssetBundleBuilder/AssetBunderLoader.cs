using UnityEngine;
using System.Collections;

public class AssetBunderLoader : MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine(LoadRes());
    }

    IEnumerator LoadRes() {
        Debug.Log(Application.dataPath + "/AssetBundleBuilder/ABs/cubebundle");
        WWW loader = WWW.LoadFromCacheOrDownload("file://" + Application.dataPath + "/AssetBundleBuilder/ABs/cubebundle", 0);
        yield return loader;
        Debug.Log(loader.assetBundle);
        Instantiate(loader.assetBundle.LoadAsset("Cube1"));
    }


    // Update is called once per frame
    void Update() {

    }
}
