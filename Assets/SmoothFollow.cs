using UnityEngine;
using System.Collections;

namespace Colr {
    public class SmoothFollow : MonoBehaviour {


        public GameObject target;
        public bool debugSwitch = false;



        private float speed = 50;
        private float realSpeed;

        private Vector3 targetPPP;
        void Start() {
            Debug.Log(Vector3.Distance(Camera.main.transform.position, target.transform.position));


            float distance = Vector3.Distance(Camera.main.transform.position, target.transform.position);
            targetPPP = Vector3.Lerp(Camera.main.transform.position, target.transform.position, (distance - 10) / distance);
            if (distance < speed) {
                speed = distance;
            }
            realSpeed = speed;
            Debug.Log(targetPPP + " <<< TargetPPP" + realSpeed);
        }

        // Update is called once per frame
        void Update() {

            //target.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * 50);

        }

        void FixedUpdate() {
            Vector3 dir = target.transform.position - Camera.main.transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookRot, Time.deltaTime * 50);


            if (Camera.main.transform.position != targetPPP) {
                float distance = Vector3.Distance(Camera.main.transform.position, targetPPP);

                //slowDown
                if (distance < speed) { speed *= 0.9f; }

                Debug.Log(realSpeed + "  <<<<====" + distance + "   " + speed);

                //move
                Camera.main.transform.Translate(Vector3.forward * Time.deltaTime * speed);


                //lastFix
                if (distance <= Time.deltaTime) {
                    Camera.main.transform.position = targetPPP;
                }
                //Debug.Log(distance + " No Done!!!");
            } else {
                Debug.Log(targetPPP + " Is Done!!!");
            }
            Debug.DrawLine(Camera.main.transform.position, target.transform.position);
        }







    }
}