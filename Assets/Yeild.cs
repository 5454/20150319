using UnityEngine;
using System.Collections;

public class Yeild : MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine(TTT());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator TTT() {
        for (int i = 0; i < 10; i++) {
            Debug.Log("===" + i + new WaitForSeconds(0.001f));
        }
        Debug.Log(Time.frameCount);
        yield return null;
    }



}
