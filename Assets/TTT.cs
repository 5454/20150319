using UnityEngine;
using System.Collections;

public class TTT : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("LoadText", "pathstr");
	}
	


    IEnumerator LoadText(string url) {
        WWW www = new WWW(url);
        yield return www;
        GUI.Button(new Rect(0,0,100,100),www.text);
    }


}
