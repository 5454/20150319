using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenCapture : MonoBehaviour {

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    void OnGUI() { 
        
    }

    IEnumerator GetCapture() {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);
        byte[] imagebytes = tex.EncodeToPNG();//转化为png图
        tex.Compress(false);//对屏幕缓存进行压缩
        //image.mainTexture = tex;//对屏幕缓存进行显示（缩略图）
        File.WriteAllBytes(Application.dataPath + "/screencapture.png", imagebytes);//存储png图
    }


}
