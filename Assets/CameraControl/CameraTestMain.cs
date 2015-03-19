using UnityEngine;
using System.Collections;

namespace Colr {
    public class CameraTestMain : MonoBehaviour {

        CameraControl cc;

        void Start() {
            cc = this.GetComponent<CameraControl>();
            for (int i = 0; i < 2; i++) {
                GameObject go = Instantiate(Resources.Load("Robot")) as GameObject;
                go.GetComponent<RandomRun>().ground = GameObject.Find("Ground");
                go.name = "Robot_" + i;
                go.transform.parent = GameObject.Find("World").transform;
                cc.targets.Add(go);
            }
            cc.controlGround = GameObject.Find("Ground").transform;
            cc.StartFollow = true;
        }

        void Update() {

        }



    }
}
