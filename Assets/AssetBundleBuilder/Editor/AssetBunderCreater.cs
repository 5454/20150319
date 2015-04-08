using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AssetBunderCreater : MonoBehaviour {

    [MenuItem("ZZZZZ/AssetBunderCreaterBySelected")]
    public static void ABCreater() {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        buildMap[0].assetBundleName = "CubeBundle";

        List<string> willBuildList = new List<string>();

        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            GameObject go = Selection.gameObjects[i];
            string assetPath = AssetDatabase.GetAssetPath(go);
            string[] dependList = AssetDatabase.GetDependencies(new string[] { assetPath });
            for (int j = 0; j < dependList.Length; j++) {
                willBuildList.Add(dependList[j]);
                Debug.Log("===> "+dependList[j]);
            }
            //willBuildList.Add(assetPath);
        }
        buildMap[0].assetNames = willBuildList.ToArray();

        foreach (string x in willBuildList) {
            //Debug.Log(x);
        }


        BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundleBuilder/ABs", buildMap, BuildAssetBundleOptions.None);
    }

    [MenuItem("ZZZZZ/AssetBunderCreaterByAutomatic")]
    public static void ABCreaterAutomatic() {
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundleBuilder/ABAs");
    }














}
