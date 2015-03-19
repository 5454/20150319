using UnityEngine;
using System.Collections;

    public class DragTree : MonoBehaviour {

        public Camera dragCamera;
        public Transform dragContainer;
        private Vector3 posDirection;
        private float posOffset;
        private bool isDrifting = false;
        private float drifingDistance = 10f;
        private Vector3 lastMousePos;

        void Start() {
            SuperDebug.EnableGUI = true;
        }

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                isDrifting = true;
            }
            if (Input.GetMouseButtonUp(0)) {
                posDirection = lastMousePos - dragContainer.position;
                posOffset = Vector3.Distance(dragContainer.position, lastMousePos);
            }



            SuperDebug.Log4GUI(
                SuperDebug.Info("dragContainerPos", dragCamera.WorldToScreenPoint(dragContainer.position)),
                SuperDebug.Info("inputPos", lastMousePos),
                SuperDebug.Info("posDirection", posDirection),
                SuperDebug.Info("posOffset", posOffset)
            );
            if (isDrifting == true) {

                if (Input.GetMouseButton(0)) {
                    lastMousePos = dragCamera.ScreenToWorldPoint(Input.mousePosition);
                    posDirection = lastMousePos - dragContainer.position;
                    posOffset = Vector3.Distance(dragContainer.position, lastMousePos);
                    posDirection.Normalize();
                    dragContainer.position += posDirection.normalized * Time.deltaTime;
                }

                if (posOffset < 0.05f) {
                    isDrifting = false;
                    posDirection = Vector3.zero;
                    posOffset = 0f;
                }


            }

        }

        void Drifting(float speed = 0f) {
            posDirection.Normalize();
            dragContainer.position += posDirection.normalized * Time.deltaTime;
        }


    }



